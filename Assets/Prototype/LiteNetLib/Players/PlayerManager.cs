using System.Collections.Generic;
using Exanite.Arpg.Networking;
using UnityEngine;

namespace Prototype.LiteNetLib.Players
{
    public class PlayerManager : MonoBehaviour
    {
        private Dictionary<int, Player> playersById = new Dictionary<int, Player>();

        public event EventHandler<PlayerManager, Player> PlayerAddedEvent;
        public event EventHandler<PlayerManager, Player> PlayerRemovedEvent;

        public ICollection<Player> Players
        {
            get
            {
                return playersById.Values;
            }
        }

        public int PlayerCount
        {
            get
            {
                return playersById.Count;
            }
        }

        public void AddPlayer(Player player)
        {
            playersById.Add(player.Id, player);

            PlayerAddedEvent?.Invoke(this, player);
        }

        public void RemovePlayer(Player player)
        {
            RemovePlayer(player.Id);
        }

        public void RemovePlayer(int id)
        {
            if (playersById.TryGetValue(id, out Player player))
            {
                playersById.Remove(id);

                PlayerRemovedEvent?.Invoke(this, player);
            }
        }

        public bool Contains(int id)
        {
            return playersById.ContainsKey(id);
        }

        public Player GetPlayer(int id)
        {
            return playersById[id];
        }

        public bool TryGetPlayer(int id, out Player player)
        {
            player = null;

            if (Contains(id))
            {
                player = playersById[id];

                return true;
            }

            return false;
        }
    }
}
