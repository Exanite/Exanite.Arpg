using System;

namespace Exanite.Arpg.Networking.Server
{
    public class PlayerConnectedArgs : EventArgs
    {
        private PlayerConnection connection;

        public PlayerConnectedArgs(PlayerConnection connection)
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
