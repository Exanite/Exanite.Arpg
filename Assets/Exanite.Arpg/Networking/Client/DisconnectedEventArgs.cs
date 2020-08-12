using System;
using LiteNetLib;

namespace Exanite.Arpg.Networking.Client
{
    /// <summary>
    /// Arguments for client Disconnected events
    /// </summary>
    public class DisconnectedEventArgs : EventArgs
    {
        private readonly NetPeer server;
        private readonly DisconnectInfo disconnectInfo;

        /// <summary>
        /// Creates a new <see cref="DisconnectedEventArgs"/>
        /// </summary>
        public DisconnectedEventArgs(NetPeer server, DisconnectInfo disconnectInfo)
        {
            this.server = server;
            this.disconnectInfo = disconnectInfo;
        }

        /// <summary>
        /// The <see cref="NetPeer"/> of the server the client disconnected from
        /// </summary>
        public NetPeer Server
        {
            get
            {
                return server;
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
        }
    }
}
