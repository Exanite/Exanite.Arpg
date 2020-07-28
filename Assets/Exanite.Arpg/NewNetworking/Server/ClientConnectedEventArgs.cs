using System;
using LiteNetLib;

namespace Exanite.Arpg.NewNetworking.Server
{
    public class ClientConnectedEventArgs : EventArgs
    {
        private NetPeer peer;

        public ClientConnectedEventArgs(NetPeer peer)
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
