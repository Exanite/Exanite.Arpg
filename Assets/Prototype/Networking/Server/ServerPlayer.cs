﻿using Prototype.Networking.Players;

namespace Prototype.Networking.Server
{
    /// <summary>
    /// Represents a player connected to the server
    /// </summary>
    public class ServerPlayer : Player
    {
        private readonly PlayerConnection connection;

        /// <summary>
        /// Creates a new <see cref="ServerPlayer"/>
        /// </summary>
        public ServerPlayer(PlayerConnection connection, ServerZoneManager zoneManager) : base(connection.Id, zoneManager, true, false)
        {
            this.connection = connection;
        }

        /// <summary>
        /// The <see cref="Player"/>'s connection
        /// </summary>
        public PlayerConnection Connection
        {
            get
            {
                return connection;
            }
        }
    }
}
