using System;
using LiteNetLib;

namespace Exanite.Arpg.NewNetworking.Server
{
    public class ClientDisconnectedEventArgs : EventArgs
    {
        private NetPeer peer;

        public ClientDisconnectedEventArgs(NetPeer peer)
        {
            Peer = peer;
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
    }
}
