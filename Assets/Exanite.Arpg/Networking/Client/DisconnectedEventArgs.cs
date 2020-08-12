using System;
using LiteNetLib;

namespace Exanite.Arpg.Networking.Client
{
    public class DisconnectedEventArgs : EventArgs
    {
        private NetPeer server;
        private DisconnectInfo disconnectInfo;

        public DisconnectedEventArgs(NetPeer server, DisconnectInfo disconnectInfo)
        {
            Server = server;
            DisconnectInfo = disconnectInfo;
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
