using System;
using System.Net;
using Cysharp.Threading.Tasks;
using Exanite.Arpg.Logging;
using LiteNetLib;
using UnityEngine;
using Zenject;

namespace Exanite.Arpg.Networking.Client
{
    /// <summary>
    /// Client that can be used to connect to a <see cref="Server.UnityServer"/>
    /// </summary>
    public class UnityClient : UnityNetwork, ISerializationCallbackReceiver
    {
        [Header("Client:")]
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

        /// <summary>
        /// Event fired when the <see cref="UnityClient"/> is connected to the server
        /// </summary>
        public event EventHandler<UnityClient, ConnectedEventArgs> ConnectedEvent;

        /// <summary>
        /// Event fired when the client is disconnected from the server
        /// </summary>
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

        /// <summary>
        /// The <see cref="NetPeer"/> representing the server
        /// </summary>
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
            Disconnect(false);
        }

        /// <summary>
        /// Connects to the server asynchronously
        /// </summary>
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

        /// <summary>
        /// Disconnects from the server
        /// </summary>
        public void Disconnect()
        {
            Disconnect(true);
        }

        /// <summary>
        /// Sends a packet to the server
        /// </summary>
        public void SendPacketToServer<T>(T packet, DeliveryMethod deliveryMethod) where T : class, IPacket, new()
        {
            SendPacket(Server, packet, deliveryMethod);
        }

        /// <summary>
        /// Disconnects from the server
        /// </summary>
        /// <param name="pollEvents">
        /// Should events be polled?<para/>
        /// Note: Should be <see langword="false"/> if called when the <see cref="Application"/> is quitting
        /// </param>
        protected void Disconnect(bool pollEvents)
        {
            netManager.DisconnectAll();

            if (pollEvents)
            {
                netManager.PollEvents();
            }

            netManager.Stop();

            isConnected = false;
            isConnecting = false;
        }

        protected override void OnPeerConnected(NetPeer server)
        {
            ConnectedEvent?.Invoke(this, new ConnectedEventArgs(server));

            IsConnecting = false;
            IsConnected = true;

            Server = server;
        }

        protected override void OnPeerDisconnected(NetPeer server, DisconnectInfo disconnectInfo)
        {
            if (IsConnected)
            {
                DisconnectedEvent?.Invoke(this, new DisconnectedEventArgs(server, disconnectInfo));
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
