using System.Collections.Generic;
using Exanite.Arpg.NewNetworking;
using LiteNetLib.Utils;
using UnityEngine;

namespace Prototype.LiteNetLib.Shared.Packets
{
    public class PlayerPositionUpdatePacket : IPacket
    {
        public List<(int id, Vector2 position)> playerPositions = new List<(int, Vector2)>();

        public PlayerPositionUpdatePacket() { }

        public PlayerPositionUpdatePacket(IEnumerable<Player> players)
        {
            foreach (var player in players)
            {
                playerPositions.Add((player.id, player.transform.position));
            }
        }

        public void Deserialize(NetDataReader reader)
        {
            playerPositions.Clear();

            int count = reader.GetInt();

            for (int i = 0; i < count; i++)
            {
                playerPositions.Add((reader.GetInt(), reader.GetVector2()));
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
    }
}
