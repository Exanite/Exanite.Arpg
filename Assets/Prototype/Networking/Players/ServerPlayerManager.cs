using System.Collections.Generic;
using Exanite.Arpg;
using LiteNetLib;
using UnityEngine;

namespace Prototype.Networking.Players
{
    public class ServerPlayerManager : MonoBehaviour
    {
        private Dictionary<int, ServerPlayer> playersById = new Dictionary<int, ServerPlayer>();

        public event EventHandler<ServerPlayerManager, ServerPlayer> PlayerAddedEvent;
        public event EventHandler<ServerPlayerManager, ServerPlayer> PlayerRemovedEvent;

        public ICollection<ServerPlayer> Players
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

        public void CreateFor(NetPeer peer) // add PlayerCreateSettings parameter
        {
            var player = new ServerPlayer(new PlayerConnection(peer));

            playersById.Add(player.Id, player);
            PlayerAddedEvent?.Invoke(this, player);
        }

        public void RemoveFor(NetPeer peer)
        {
            int id = peer.Id;

            if (playersById.TryGetValue(id, out ServerPlayer player))
            {
                playersById.Remove(id);
                PlayerRemovedEvent?.Invoke(this, player);
            }
        }

        public bool Contains(int id)
        {
            return playersById.ContainsKey(id);
        }

        public ServerPlayer GetPlayer(int id)
        {
            return playersById[id];
        }

        public bool TryGetPlayer(int id, out ServerPlayer player)
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
