using LiteNetLib;

namespace Prototype.LiteNetLib.Shared
{
    /// <summary>
    /// Represents a player connected to the server<para/>
    /// Can be used in both the Client and the Server
    /// </summary>
    public class PlayerConnection
    {
        private int id;
        private NetPeer peer;

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
        /// The <see cref="NetPeer"/> of the connected player<para/>
        /// Note: This property will be <see langword="null"/> on the Client side
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
    }
}
