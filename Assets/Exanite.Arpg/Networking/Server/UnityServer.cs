﻿using System;
using System.Collections.Generic;
using Exanite.Arpg.Logging;
using LiteNetLib;
using UnityEngine;
using Zenject;

namespace Exanite.Arpg.Networking.Server
{
    public class UnityServer : UnityNetBase
    {
        [Header("Settings:")]
        [SerializeField] private ushort port = Constants.DefaultPort;

        private bool isCreated = false;
        private List<NetPeer> connectedClients = new List<NetPeer>();

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

        protected override bool IsReady
        {
            get
            {
                return IsCreated;
            }
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

        protected override void OnPeerConnected(NetPeer peer)
        {
            connectedClients.Add(peer);

            ClientConnectedEvent?.Invoke(this, new ClientConnectedEventArgs(peer));
        }

        protected override void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            connectedClients.Remove(peer);

            ClientDisconnectedEvent?.Invoke(this, new ClientDisconnectedEventArgs(peer, disconnectInfo));
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
