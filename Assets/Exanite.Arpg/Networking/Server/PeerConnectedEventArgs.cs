using System;
using LiteNetLib;

namespace Exanite.Arpg.Networking.Server
{
    public class PeerConnectedEventArgs : EventArgs
    {
        private NetPeer peer;

        public PeerConnectedEventArgs(NetPeer peer)
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
