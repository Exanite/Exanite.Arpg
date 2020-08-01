using LiteNetLib;

namespace Prototype.LiteNetLib.Server
{
    /// <summary>
    /// Represents a player connected to the server
    /// </summary>
    public class PlayerConnection
    {
        private int id;
        private NetPeer client;

        private string name;

        /// <summary>
        /// Network ID of the connected player
        /// </summary>
        public int ID
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        /// <summary>
        /// The <see cref="NetPeer"/> of the connected player
        /// </summary>
        public NetPeer Client
        {
            get
            {
                return client;
            }

            set
            {
                client = value;
            }
        }

        /// <summary>
        /// The name of the connected player
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }
    }
}
