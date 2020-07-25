using System.Collections.Generic;

namespace Exanite.Arpg.Networking.Server
{
    public class PlayerManager
    {
        private Dictionary<ushort, PlayerConnection> playersByID = new Dictionary<ushort, PlayerConnection>();
        private Dictionary<string, PlayerConnection> playersByName = new Dictionary<string, PlayerConnection>();

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
            playersByName.Add(connection.Name, connection);
        }

        public void RemovePlayer(PlayerConnection connection)
        {
            playersByID.Remove(connection.ID);
            playersByName.Remove(connection.Name);
        }

        public bool Contains(ushort id)
        {
            return playersByID.ContainsKey(id);
        }

        public bool Contains(string name)
        {
            return playersByName.ContainsKey(name);
        }

        public PlayerConnection GetPlayerConnection(ushort id)
        {
            return playersByID[id];
        }

        public PlayerConnection GetPlayerConnection(string name)
        {
            return playersByName[name];
        }
    }
}
