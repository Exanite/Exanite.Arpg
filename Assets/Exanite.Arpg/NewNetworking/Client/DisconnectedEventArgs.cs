using System;
using LiteNetLib;

namespace Exanite.Arpg.NewNetworking.Client
{
    public class DisconnectedEventArgs : EventArgs
    {
        private DisconnectInfo disconnectInfo;

        public DisconnectedEventArgs(DisconnectInfo disconnectInfo)
        {
            DisconnectInfo = disconnectInfo;
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
