using System;
using LiteNetLib;

namespace Exanite.Arpg.Networking.Client
{
    /// <summary>
    /// Arguments for client Connected events
    /// </summary>
    public class ConnectedEventArgs : EventArgs
    {
        private NetPeer server;

        /// <summary>
        /// Creates a new <see cref="ConnectedEventArgs"/>
        /// </summary>
        public ConnectedEventArgs(NetPeer server)
        {
            Server = server;
        }

        /// <summary>
        /// The <see cref="NetPeer"/> of the server the client connected to
        /// </summary>
        public NetPeer Server
        {
            get
            {
                return server;
            }

            set
            {
                server = value;
            }
        }
    }
}
