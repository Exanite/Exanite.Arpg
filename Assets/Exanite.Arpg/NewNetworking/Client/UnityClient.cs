using Exanite.Arpg.Logging;
using Exanite.Arpg.Networking.Client;
using LiteNetLib;
using LiteNetLib.Utils;
using UniRx.Async;
using UnityEngine;
using Zenject;

namespace Exanite.Arpg.NewNetworking.Client
{
    public class UnityClient : MonoBehaviour
    {
        public string ipAddress = "127.0.0.1";
        public ushort port = Constants.DefaultPort;

        public bool isConnected;

        public NetPeer server;
        private DisconnectInfo previousDisconnectInfo;

        private EventBasedNetListener netListener;
        private NetManager netClient;
        private NetPacketProcessor netPacketProcessor;

        private ILog log;

        [Inject]
        public void Inject(ILog log)
        {
            this.log = log;
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
                    log.Information("Connected to {IP} on port {Port}", ipAddress, port);
                }
                else
                {
                    log.Information("Failed to connect to {IP} on port {Port}. Reason: {FailReason}", ipAddress, port, x.FailReason);
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
            NetPeer peer;

            netClient.Start();
            peer = netClient.Connect(ipAddress, port, Constants.ConnectionKey);

            await UniTask.WaitUntil(() => peer.ConnectionState != ConnectionState.Outgoing);

            return new ConnectResult(isConnected, previousDisconnectInfo.Reason.ToString());
        }

        public void Disconnect()
        {
            netClient.Stop();
        }

        private void UnityClient_PeerConnectedEvent(NetPeer peer)
        {
            isConnected = true;

            server = peer;
        }

        private void UnityClient_PeerDisconnectedEvent(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            isConnected = false;

            server = null;
            previousDisconnectInfo = disconnectInfo;
        }
    }
}
