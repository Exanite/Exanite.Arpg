using System;
using LiteNetLib;

namespace Exanite.Arpg.Networking.Server
{
    /// <summary>
    /// Arguments for server PeerDisconnected events
    /// </summary>
    public class PeerDisconnectedEventArgs : EventArgs
    {
        private NetPeer peer;
        private DisconnectInfo disconnectInfo;

        /// <summary>
        /// Creates a new <see cref="PeerDisconnectedEventArgs"/>
        /// </summary>
        public PeerDisconnectedEventArgs(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            Peer = peer;
            DisconnectInfo = disconnectInfo;
        }

        /// <summary>
        /// The <see cref="NetPeer"/> that disconnected from the server
        /// </summary>
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

        /// <summary>
        /// Additional information about the disconnection
        /// </summary>
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
