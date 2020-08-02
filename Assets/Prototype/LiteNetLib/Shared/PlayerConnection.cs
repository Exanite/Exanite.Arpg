using LiteNetLib;

namespace Prototype.LiteNetLib.Shared
{
    /// <summary>
    /// Represents a player connected to the server
    /// </summary>
    public class PlayerConnection
    {
        private int id;
        private NetPeer peer;

        private string name;

        /// <summary>
        /// Network Id of the connected player
        /// </summary>
        public int Id
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
