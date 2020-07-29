using System;
using System.Net;
using Exanite.Arpg.Logging;
using Exanite.Arpg.Networking.Client;
using Exanite.Arpg.NewNetworking.Shared;
using LiteNetLib;
using LiteNetLib.Utils;
using UniRx.Async;
using UnityEngine;
using Zenject;

namespace Exanite.Arpg.NewNetworking.Client
{
    public class UnityClient : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField] private string address = IPAddress.Loopback.ToString();
        [SerializeField] private ushort port = Constants.DefaultPort;

        private bool isConnecting;
        private bool isConnected;

        private IPAddress ipAddress;
        private NetPeer server;
        private DisconnectInfo previousDisconnectInfo;

        private EventBasedNetListener netListener;
        private NetManager netClient;
        private NetPacketProcessor netPacketProcessor;

        private NetDataWriter writer = new NetDataWriter();

        private ILog log;

        [Inject]
        public void Inject(ILog log)
        {
            this.log = log;
        }

        public event EventHandler<ConnectedEventArgs> ConnectedEvent;

        public event EventHandler<DisconnectedEventArgs> DisconnectedEvent;

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

        private void Awake()
        {
            netListener = new EventBasedNetListener();
            netClient = new NetManager(netListener);
            netPacketProcessor = new NetPacketProcessor();

            netListener.PeerConnectedEvent += UnityClient_PeerConnectedEvent;
            netListener.PeerDisconnectedEvent += UnityClient_PeerDisconnectedEvent;
        }

        private void Start() // ! temp
        {
            ConnectAsync().ContinueWith(x =>
            {
                if (x.IsSuccess)
                {
                    log.Information("Connected to {IP} on port {Port}", IPAddress, Port);
                }
                else
                {
                    log.Information("Failed to connect to {IP} on port {Port}. Reason: {FailReason}", IPAddress, Port, x.FailReason);
                }
            })
            .Forget();
        }

        private void FixedUpdate()
        {
            netClient.PollEvents();
        }

        public async UniTask<ConnectResult> ConnectAsync()
        {
            if (IsConnected)
            {
                throw new InvalidOperationException("Client is already connected.");
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

        public void SendPacket(IPacket packet, DeliveryMethod deliveryMethod)
        {
            if (!IsConnected)
            {
                return;
            }

            writer.Reset();

            netPacketProcessor.WriteNetSerializable(writer, packet);

            server.Send(writer, deliveryMethod);
        }

        private void UnityClient_PeerConnectedEvent(NetPeer peer)
        {
            ConnectedEvent?.Invoke(this, new ConnectedEventArgs());

            IsConnecting = false;
            IsConnected = true;

            server = peer;
        }

        private void UnityClient_PeerDisconnectedEvent(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            if (IsConnected)
            {
                DisconnectedEvent?.Invoke(this, new DisconnectedEventArgs(disconnectInfo));
            }

            IsConnecting = false;
            IsConnected = false;

            server = null;
            previousDisconnectInfo = disconnectInfo;
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
