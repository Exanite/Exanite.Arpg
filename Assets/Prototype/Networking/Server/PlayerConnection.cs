using System;
using LiteNetLib;

namespace Prototype.Networking.Server
{
    /// <summary>
    /// Represents a <see cref="Player"/>'s connection to the server
    /// </summary>
    public class PlayerConnection
    {
        private NetPeer peer;

        /// <summary>
        /// Creates a new <see cref="PlayerConnection"/> from the provided <see cref="NetPeer"/>
        /// </summary>
        public PlayerConnection(NetPeer peer)
        {
            Peer = peer ?? throw new ArgumentNullException(nameof(peer));
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
        /// Network Id of the connected player
        /// </summary>
        public int Id
        {
            get
            {
                return peer.Id;
            }
        }
    }
}
