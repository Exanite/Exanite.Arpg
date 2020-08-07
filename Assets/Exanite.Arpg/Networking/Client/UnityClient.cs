using System;
using System.Net;
using Exanite.Arpg.Logging;
using LiteNetLib;
using UniRx.Async;
using UnityEngine;
using Zenject;

namespace Exanite.Arpg.Networking.Client
{
    public class UnityClient : UnityNetBase, ISerializationCallbackReceiver
    {
        [Header("Settings:")]
        [SerializeField] private string address = IPAddress.Loopback.ToString();
        [SerializeField] private ushort port = Constants.DefaultPort;

        private bool isConnecting;
        private bool isConnected;
        private NetPeer server;

        private IPAddress ipAddress;
        private DisconnectInfo previousDisconnectInfo;

        private ILog log;

        [Inject]
        public void Inject(ILog log)
        {
            this.log = log;
        }

        public event EventHandler<UnityClient, ConnectedEventArgs> ConnectedEvent;

        public event EventHandler<UnityClient, DisconnectedEventArgs> DisconnectedEvent;

        /// <summary>
        /// The IP Address of the server the client will connect to
        /// </summary>
        public IPAddress IPAddress
        {
            get
            {
                return ipAddress;
            }

            set
            {
                ipAddress = value;
                address = value.ToString();
            }
        }

        /// <summary>
        /// The port on the server the client will connect to
        /// </summary>
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

        /// <summary>
        /// Is the client currently connecting to the server?
        /// </summary>
        public bool IsConnecting
        {
            get
            {
                return isConnecting;
            }

            private set
            {
                isConnecting = value;
            }
        }

        /// <summary>
        /// Is the client connected to the server?
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return isConnected;
            }

            private set
            {
                isConnected = value;
            }
        }

        public NetPeer Server
        {
            get
            {
                return server;
            }

            private set
            {
                server = value;
            }
        }

        protected override bool IsReady
        {
            get
            {
                return IsConnected;
            }
        }

        private void OnDestroy()
        {
            Disconnect();
        }

        public async UniTask<ConnectResult> ConnectAsync()
        {
            if (IsConnected)
            {
                throw new InvalidOperationException("Client is already connected.");
            }
            else if (IsConnecting)
            {
                throw new InvalidOperationException("Client is already connecting.");
            }

            IsConnecting = true;

            netManager.Start();
            netManager.Connect(new IPEndPoint(IPAddress, Port), Constants.ConnectionKey);

            await UniTask.WaitUntil(() => !IsConnecting);

            return new ConnectResult(IsConnected, previousDisconnectInfo.Reason.ToString());
        }

        public void Disconnect()
        {
            netManager?.Stop();
        }

        public void SendPacketToServer<T>(T packet, DeliveryMethod deliveryMethod) where T : class, IPacket, new()
        {
            SendPacket(Server, packet, deliveryMethod);
        }

        protected override void OnPeerConnected(NetPeer peer)
        {
            ConnectedEvent?.Invoke(this, new ConnectedEventArgs(peer));

            IsConnecting = false;
            IsConnected = true;

            Server = peer;
        }

        protected override void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            if (IsConnected)
            {
                DisconnectedEvent?.Invoke(this, new DisconnectedEventArgs(peer, disconnectInfo));
            }

            IsConnecting = false;
            IsConnected = false;

            Server = null;
            previousDisconnectInfo = disconnectInfo;
        }

        protected override void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            netPacketProcessor.ReadAllPackets(reader, peer);
            reader.Recycle();
        }

        protected override void OnConnectionRequest(ConnectionRequest request)
        {
            request.Reject();
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (IPAddress != null)
            {
                address = IPAddress.ToString();
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            IPAddress = IPAddress.Parse(address);
        }
    }
}
