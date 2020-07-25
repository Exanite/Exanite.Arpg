using System;

namespace Exanite.Arpg.Networking.Server
{
    public class PlayerDisconnectedArgs : EventArgs
    {
        private PlayerConnection connection;

        public PlayerDisconnectedArgs(PlayerConnection connection)
        {
            Connection = connection;
        }

        public PlayerConnection Connection
        {
            get
            {
                return connection;
            }

            set
            {
                connection = value;
            }
        }
    }
}
