using System;
using LiteNetLib;

namespace Exanite.Arpg.Networking.Server
{
    /// <summary>
    /// Arguments for server PeerConnected events
    /// </summary>
    public class PeerConnectedEventArgs : EventArgs
    {
        private readonly NetPeer peer;

        /// <summary>
        /// Creates a new <see cref="PeerConnectedEventArgs"/>
        /// </summary>
        public PeerConnectedEventArgs(NetPeer peer)
        {
            this.peer = peer;
        }

        /// <summary>
        /// The <see cref="NetPeer"/> that connected to the server
        /// </summary>
        public NetPeer Peer
        {
            get
            {
                return peer;
            }
        }
    }
}
