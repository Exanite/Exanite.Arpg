using System.Collections.Generic;
using Exanite.Arpg.Networking;
using LiteNetLib.Utils;
using UnityEngine;

namespace Prototype.LiteNetLib.Players.Packets
{
    public class PlayerPositionUpdatePacket : IPacket
    {
        public List<PlayerPosition> playerPositions = new List<PlayerPosition>();

        public PlayerPositionUpdatePacket() { }

        public PlayerPositionUpdatePacket(ICollection<Player> players)
        {
            playerPositions.Capacity = players.Count;

            foreach (var player in players)
            {
                playerPositions.Add(new PlayerPosition(player.Id, player.character.transform.position));
            }
        }

        public void Deserialize(NetDataReader reader)
        {
            int count = reader.GetInt();

            playerPositions.Clear();

            if (playerPositions.Capacity < count)
            {
                playerPositions.Capacity = count;
            }

            for (int i = 0; i < count; i++)
            {
                playerPositions.Add(new PlayerPosition(reader.GetInt(), reader.GetVector2()));
            }
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(playerPositions.Count);

            for (int i = 0; i < playerPositions.Count; i++)
            {
                writer.Put(playerPositions[i].id);
                writer.Put(playerPositions[i].position);
            }
        }

        public struct PlayerPosition
        {
            public int id;
            public Vector2 position;

            public PlayerPosition(int id, Vector2 position)
            {
                this.id = id;
                this.position = position;
            }
        }
    }
}
