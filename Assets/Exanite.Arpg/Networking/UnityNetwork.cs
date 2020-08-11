using System;
using System.Net;
using System.Net.Sockets;
using Exanite.Arpg.Networking.Client;
using Exanite.Arpg.Networking.Server;
using LiteNetLib;
using LiteNetLib.Utils;
using UnityEngine;

namespace Exanite.Arpg.Networking
{
    /// <summary>
    /// Base class used by <see cref="UnityClient"/> and <see cref="UnityServer"/>
    /// </summary>
    public abstract class UnityNetwork : MonoBehaviour, INetEventListener
    {
        [Header("Debug:")]
        [SerializeField] private bool enableDebug = false;
        [SerializeField] private int minLatency = 25;
        [SerializeField] private int maxLatency = 100;
        [SerializeField] private int packetLoss = 0;

        protected NetManager netManager;
        protected NetPacketProcessor netPacketProcessor;

        protected NetDataWriter writer = new NetDataWriter();

        protected abstract bool IsReady { get; }

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

        public void SendPacket<T>(NetPeer peer, T packet, DeliveryMethod deliveryMethod) where T : class, IPacket, new()
        {
            if (!IsReady)
            {
                return;
            }

            writer.Reset();

            netPacketProcessor.WriteNetSerializable(writer, packet);

            peer.Send(writer, deliveryMethod);
        }

        public void RegisterPacketReceiver<T>(EventHandler<NetPeer, T> receiver) where T : class, IPacket, new()
        {
            if (receiver == null)
            {
                throw new ArgumentNullException(nameof(receiver));
            }

            ClearPacketReceiver<T>();

            netPacketProcessor.SubscribeNetSerializable<T, NetPeer>((packet, sender) =>
            {
                receiver.Invoke(sender, packet);
            });
        }

        public void ClearPacketReceiver<T>() where T : class, IPacket, new()
        {
            netPacketProcessor.RemoveSubscription<T>();
        }

        protected virtual void OnPeerConnected(NetPeer peer) { }

        protected virtual void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo) { }

        protected virtual void OnNetworkError(IPEndPoint endPoint, SocketError socketError) { }

        protected virtual void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod) { }

        protected virtual void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType) { }

        protected virtual void OnNetworkLatencyUpdate(NetPeer peer, int latency) { }

        protected virtual void OnConnectionRequest(ConnectionRequest request) { }

        void INetEventListener.OnPeerConnected(NetPeer peer)
        {
            OnPeerConnected(peer);
        }

        void INetEventListener.OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            OnPeerDisconnected(peer, disconnectInfo);
        }

        void INetEventListener.OnNetworkError(IPEndPoint endPoint, SocketError socketError)
        {
            OnNetworkError(endPoint, socketError);
        }

        void INetEventListener.OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            OnNetworkReceive(peer, reader, deliveryMethod);
        }

        void INetEventListener.OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {
            OnNetworkReceiveUnconnected(remoteEndPoint, reader, messageType);
        }

        void INetEventListener.OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {
            OnNetworkLatencyUpdate(peer, latency);
        }

        void INetEventListener.OnConnectionRequest(ConnectionRequest request)
        {
            OnConnectionRequest(request);
        }
    }
}
