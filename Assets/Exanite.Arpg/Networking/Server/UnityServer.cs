using System;
using System.Collections.Generic;
using Exanite.Arpg.Logging;
using LiteNetLib;
using UnityEngine;
using Zenject;

namespace Exanite.Arpg.Networking.Server
{
    /// <summary>
    /// Server that can accept connections from <see cref="Client.UnityClient"/>s
    /// </summary>
    public class UnityServer : UnityNetwork
    {
        [Header("Settings:")]
        [SerializeField] private ushort port = Constants.DefaultPort;

        private bool isCreated = false;
        private List<NetPeer> connectedPeers = new List<NetPeer>();

        private ILog log;

        [Inject]
        public void Inject(ILog log)
        {
            this.log = log;
        }

        /// <summary>
        /// Event fired when a <see cref="NetPeer"/> connects to the server
        /// </summary>
        public event EventHandler<UnityServer, PeerConnectedEventArgs> ClientConnectedEvent;

        /// <summary>
        /// Event fired when a <see cref="NetPeer"/> disconnects from the server
        /// </summary>
        public event EventHandler<UnityServer, PeerDisconnectedEventArgs> ClientDisconnectedEvent;

        /// <summary>
        /// Port the server will listen on
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
        /// Is the server created and ready for connections?
        /// </summary>
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

        /// <summary>
        /// List of all the <see cref="NetPeer"/>s connected to the server
        /// </summary>
        public IReadOnlyList<NetPeer> ConnectedPeers
        {
            get
            {
                return connectedPeers;
            }
        }

        protected override bool IsReady
        {
            get
            {
                return IsCreated;
            }
        }

        private void OnDestroy()
        {
            Close(false);
        }

        /// <summary>
        /// Creates the server
        /// </summary>
        public void Create()
        {
            if (IsCreated)
            {
                throw new InvalidOperationException("Server has already been created.");
            }

            netManager.Start(Port);

            IsCreated = true;
        }

        /// <summary>
        /// Closes the server
        /// </summary>
        public void Close()
        {
            Close(true);
        }

        /// <summary>
        /// Closes the server
        /// </summary>
        /// <param name="pollEvents">
        /// Should events be polled?<para/>
        /// Note: Should be <see langword="false"/> if called when the <see cref="Application"/> is quitting
        /// </param>
        protected void Close(bool pollEvents)
        {
            if (!IsCreated)
            {
                return;
            }

            netManager.DisconnectAll();

            if (pollEvents)
            {
                netManager.PollEvents();
            }

            netManager.Stop();

            IsCreated = false;
        }

        protected override void OnPeerConnected(NetPeer peer)
        {
            connectedPeers.Add(peer);

            ClientConnectedEvent?.Invoke(this, new PeerConnectedEventArgs(peer));
        }

        protected override void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            connectedPeers.Remove(peer);

            ClientDisconnectedEvent?.Invoke(this, new PeerDisconnectedEventArgs(peer, disconnectInfo));
        }

        protected override void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            netPacketProcessor.ReadAllPackets(reader, peer);
            reader.Recycle();
        }

        protected override void OnConnectionRequest(ConnectionRequest request)
        {
            request.AcceptIfKey(Constants.ConnectionKey);
        }
    }
}
