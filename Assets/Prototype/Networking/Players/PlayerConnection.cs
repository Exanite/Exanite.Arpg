using System;
using LiteNetLib;

namespace Prototype.Networking.Players
{
    /// <summary>
    /// Represents a player connected to the server
    /// </summary>
    public class PlayerConnection
    {
        private NetPeer peer;

        public PlayerConnection(NetPeer peer)
        {
            Peer = peer ?? throw new ArgumentNullException(nameof(peer));
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
