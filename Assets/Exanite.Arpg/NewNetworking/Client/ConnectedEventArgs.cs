using System;
using LiteNetLib;

namespace Exanite.Arpg.NewNetworking.Client
{
    public class ConnectedEventArgs : EventArgs
    {
        private NetPeer peer;

        public ConnectedEventArgs(NetPeer peer)
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
