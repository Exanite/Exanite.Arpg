using System.Collections.Generic;

namespace Exanite.Arpg.Networking.Server
{
    public class PlayerManager
    {
        private Dictionary<ushort, PlayerConnection> playersByID = new Dictionary<ushort, PlayerConnection>();
        private Dictionary<string, PlayerConnection> playersByName = new Dictionary<string, PlayerConnection>();

        public void AddPlayer(PlayerConnection connection)
        {
            playersByID.Add(connection.id, connection);
            playersByName.Add(connection.name, connection);
        }

        public void RemovePlayer(PlayerConnection connection)
        {
            playersByID.Remove(connection.id);
            playersByName.Remove(connection.name);
        }

        public bool Contains(ushort id)
        {
            return playersByID.ContainsKey(id);
        }

        public bool Contains(string name)
        {
            return playersByName.ContainsKey(name);
        }
    }
}
