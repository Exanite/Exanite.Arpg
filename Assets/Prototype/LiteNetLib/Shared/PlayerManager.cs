using System.Collections.Generic;
using Prototype.LiteNetLib.Server;

namespace Prototype.LiteNetLib.Shared
{
    public class PlayerManager
    {
        private Dictionary<int, PlayerConnection> playersByID = new Dictionary<int, PlayerConnection>();

        public ICollection<PlayerConnection> ConnectedPlayers
        {
            get
            {
                return playersByID.Values;
            }
        }

        public void AddPlayer(PlayerConnection connection)
        {
            playersByID.Add(connection.ID, connection);
        }

        public void RemovePlayer(PlayerConnection connection)
        {
            playersByID.Remove(connection.ID);
        }

        public bool Contains(ushort id)
        {
            return playersByID.ContainsKey(id);
        }

        public PlayerConnection GetPlayerConnection(ushort id)
        {
            return playersByID[id];
        }
    }
}
