using System;
using System.Net;
using System.Net.Sockets;
using Exanite.Arpg.Logging;
using LiteNetLib;
using LiteNetLib.Utils;
using UniRx.Async;
using UnityEngine;
using Zenject;

namespace Exanite.Arpg.Networking.Client
{
    public class UnityClient : MonoBehaviour, ISerializationCallbackReceiver, INetEventListener
    {
        [SerializeField] private string address = IPAddress.Loopback.ToString();
        [SerializeField] private ushort port = Constants.DefaultPort;

        private bool isConnecting;
        private bool isConnected;
        private NetPeer server;

        private IPAddress ipAddress;
        private DisconnectInfo previousDisconnectInfo;

        private NetManager netClient;
        private NetPacketProcessor netPacketProcessor;

        private NetDataWriter writer = new NetDataWriter();

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

        private void Awake()
        {
            netClient = new NetManager(this);
            netPacketProcessor = new NetPacketProcessor();
        }

        private void FixedUpdate()
        {
            netClient.PollEvents();
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

            netClient.Start();
            netClient.Connect(new IPEndPoint(IPAddress, Port), Constants.ConnectionKey);

            await UniTask.WaitUntil(() => !IsConnecting);

            return new ConnectResult(IsConnected, previousDisconnectInfo.Reason.ToString());
        }

        public void Disconnect()
        {
            netClient.Stop();
        }

        public void SendPacket<T>(T packet, DeliveryMethod deliveryMethod) where T : class, IPacket, new()
        {
            if (!IsConnected)
            {
                return;
            }

            writer.Reset();

            netPacketProcessor.WriteNetSerializable(writer, packet);

            Server.Send(writer, deliveryMethod);
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

        void INetEventListener.OnPeerConnected(NetPeer peer)
        {
            ConnectedEvent?.Invoke(this, new ConnectedEventArgs(peer));

            IsConnecting = false;
            IsConnected = true;

            Server = peer;
        }

        void INetEventListener.OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
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
            request.Reject();
        }
    }
}
