﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Exanite.Arpg.Logging;
using LiteNetLib;
using LiteNetLib.Utils;
using UnityEngine;
using Zenject;

namespace Exanite.Arpg.Networking.Server
{
    public class UnityServer : MonoBehaviour, INetEventListener
    {
        [Header("Settings:")]
        [SerializeField] private ushort port = Constants.DefaultPort;

        [Header("Debug:")]
        [SerializeField] private bool enableDebug = false;
        [SerializeField] private int minLatency = 25;
        [SerializeField] private int maxLatency = 100;
        [SerializeField] private int packetLoss = 0;

        private bool isCreated = false;
        private List<NetPeer> connectedClients = new List<NetPeer>();

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
            netManager = new NetManager(this);
            netPacketProcessor = new NetPacketProcessor();

            netManager.SimulateLatency = enableDebug;
            netManager.SimulatePacketLoss = enableDebug;
            netManager.SimulationMinLatency = minLatency;
            netManager.SimulationMaxLatency = maxLatency;
            netManager.SimulationPacketLossChance = packetLoss;
        }

        private void FixedUpdate()
        {
            netManager.PollEvents();
        }

        private void OnDestroy()
        {
            Close();
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

        public void SendPacket<T>(NetPeer peer, T packet, DeliveryMethod deliveryMethod) where T : class, IPacket, new()
        {
            if (!IsCreated)
            {
                return;
            }

            writer.Reset();

            netPacketProcessor.WriteNetSerializable(writer, packet);

            peer.Send(writer, deliveryMethod);
        }

        public void SubscribePacketReceiver<T>(EventHandler<NetPeer, T> receiver) where T : class, IPacket, new()
        {
            if (receiver == null)
            {
                throw new ArgumentNullException(nameof(receiver));
            }

            netPacketProcessor.SubscribeNetSerializable<T, NetPeer>((packet, sender) =>
            {
                receiver.Invoke(sender, packet);
            });
        }

        public void ClearPacketReceievers<T>() where T : class, IPacket, new()
        {
            netPacketProcessor.RemoveSubscription<T>();
        }

        void INetEventListener.OnPeerConnected(NetPeer peer)
        {
            connectedClients.Add(peer);

            ClientConnectedEvent?.Invoke(this, new ClientConnectedEventArgs(peer));
        }

        void INetEventListener.OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            connectedClients.Remove(peer);

            ClientDisconnectedEvent?.Invoke(this, new ClientDisconnectedEventArgs(peer, disconnectInfo));
        }

        void INetEventListener.OnNetworkError(IPEndPoint endPoint, SocketError socketError)
        {

        }

        void INetEventListener.OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            netPacketProcessor.ReadAllPackets(reader, peer);
            reader.Recycle();
        }

        void INetEventListener.OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {

        }

        void INetEventListener.OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {

        }

        void INetEventListener.OnConnectionRequest(ConnectionRequest request)
        {
            request.AcceptIfKey(Constants.ConnectionKey);
        }
    }
}
