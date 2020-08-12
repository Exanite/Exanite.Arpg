using System;
using LiteNetLib;

namespace Exanite.Arpg.Networking.Client
{
    public class ConnectedEventArgs : EventArgs
    {
        private NetPeer server;

        public ConnectedEventArgs(NetPeer server)
        {
            Server = server;
        }

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
