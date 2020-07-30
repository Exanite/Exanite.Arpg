using System;
using LiteNetLib;

namespace Exanite.Arpg.NewNetworking.Client
{
    public class DisconnectedEventArgs : EventArgs
    {
        private NetPeer peer;
        private DisconnectInfo disconnectInfo;

        public DisconnectedEventArgs(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            Peer = peer;
            DisconnectInfo = disconnectInfo;
        }

        public NetPeer Peer
        {
            get
            {
                return peer;
            }

            set
            {
                peer = value;
            }
        }

        public DisconnectInfo DisconnectInfo
        {
            get
            {
                return disconnectInfo;
            }

            set
            {
                disconnectInfo = value;
            }
        }
    }
}
