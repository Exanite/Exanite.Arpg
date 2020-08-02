using System.Collections.Generic;
using Prototype.LiteNetLib.Server;

namespace Prototype.LiteNetLib.Shared
{
    public class PlayerManager
    {
        private Dictionary<int, PlayerConnection> playersById = new Dictionary<int, PlayerConnection>();

        public ICollection<PlayerConnection> ConnectedPlayers
        {
            get
            {
                return playersById.Values;
            }
        }

        public void AddPlayer(PlayerConnection connection)
        {
            playersById.Add(connection.Id, connection);
        }

        public void RemovePlayer(PlayerConnection connection)
        {
            playersById.Remove(connection.Id);
        }

        public bool Contains(ushort id)
        {
            return playersById.ContainsKey(id);
        }

        public PlayerConnection GetPlayerConnection(ushort id)
        {
            return playersById[id];
        }
    }
}
