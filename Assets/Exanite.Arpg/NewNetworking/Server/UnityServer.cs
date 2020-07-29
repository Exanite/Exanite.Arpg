using System;
using System.Collections.Generic;
using Exanite.Arpg.Logging;
using Exanite.Arpg.NewNetworking.Shared;
using LiteNetLib;
using LiteNetLib.Utils;
using UnityEngine;
using Zenject;

namespace Exanite.Arpg.NewNetworking.Server
{
    public class UnityServer : MonoBehaviour
    {
        private ushort port = Constants.DefaultPort;

        private bool isCreated = false;
        private List<NetPeer> connectedClients = new List<NetPeer>();

        private EventBasedNetListener netListener;
        private NetManager netManager;
        private NetPacketProcessor netPacketProcessor;

        private NetDataWriter writer = new NetDataWriter();

        private ILog log;

        [Inject]
        public void Inject(ILog log)
        {
            this.log = log;
        }

        public event EventHandler<UnityServer, ClientConnectedEventArgs> ClientConnectedEvent;

        public event EventHandler<UnityServer, ClientDisconnectedEventArgs> ClientDisconnectedEvent;

        public ushort Port
        {
            get
            {
                return port;
            }

            set
            {
                port = value;
            }
        }

        public bool IsCreated
        {
            get
            {
                return isCreated;
            }

            private set
            {
                isCreated = value;
            }
        }

        public IReadOnlyList<NetPeer> ConnectedClients
        {
            get
            {
                return connectedClients;
            }
        }

        private void Awake()
        {
            netListener = new EventBasedNetListener();
            netManager = new NetManager(netListener);
            netPacketProcessor = new NetPacketProcessor();

            netListener.ConnectionRequestEvent += UnityServer_ConnectionRequestEvent;
            netListener.PeerConnectedEvent += UnityServer_PeerConnectedEvent;
            netListener.PeerDisconnectedEvent += UnityServer_PeerDisconnectedEvent;
        }

        private void Start() // ! temp
        {
            Create();
        }

        private void FixedUpdate()
        {
            netManager.PollEvents();
        }

        private void OnDestroy()
        {
            Close();

            netListener.ConnectionRequestEvent -= UnityServer_ConnectionRequestEvent;
            netListener.PeerConnectedEvent -= UnityServer_PeerConnectedEvent;
            netListener.PeerDisconnectedEvent -= UnityServer_PeerDisconnectedEvent;
        }

        public void Create()
        {
            if (IsCreated)
            {
                throw new InvalidOperationException("Server has already been created.");
            }

            netManager.Start(Port);

            IsCreated = true;
        }

        public void Close()
        {
            if (!IsCreated)
            {
                return;
            }

            netManager.DisconnectAll();
            netManager.Stop();

            IsCreated = false;
        }

        public void SendPacket(NetPeer peer, IPacket packet, DeliveryMethod deliveryMethod)
        {
            if (!IsCreated)
            {
                return;
            }

            writer.Reset();

            netPacketProcessor.WriteNetSerializable(writer, packet);

            peer.Send(writer, deliveryMethod);
        }

        private void UnityServer_ConnectionRequestEvent(ConnectionRequest request)
        {
            request.AcceptIfKey(Constants.ConnectionKey);
        }

        private void UnityServer_PeerConnectedEvent(NetPeer peer)
        {
            connectedClients.Add(peer);

            ClientConnectedEvent?.Invoke(this, new ClientConnectedEventArgs(peer));
        }

        private void UnityServer_PeerDisconnectedEvent(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            connectedClients.Remove(peer);

            ClientDisconnectedEvent?.Invoke(this, new ClientDisconnectedEventArgs(peer, disconnectInfo));
        }
    }
}
