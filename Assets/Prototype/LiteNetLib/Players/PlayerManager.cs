using System.Collections.Generic;

namespace Prototype.LiteNetLib.Players
{
    public class PlayerManager
    {
        private Dictionary<int, Player> playersById = new Dictionary<int, Player>();

        public ICollection<Player> Players
        {
            get
            {
                return playersById.Values;
            }
        }

        public void AddPlayer(Player player)
        {
            playersById.Add(player.Id, player);
        }

        public void RemovePlayer(Player player)
        {
            playersById.Remove(player.Id);
        }

        public void RemovePlayer(int id)
        {
            playersById.Remove(id);
        }

        public bool Contains(int id)
        {
            return playersById.ContainsKey(id);
        }

        public Player GetPlayer(int id)
        {
            return playersById[id];
        }
    }
}
