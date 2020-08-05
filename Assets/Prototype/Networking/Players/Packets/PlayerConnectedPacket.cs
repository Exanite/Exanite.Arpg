using System.Collections.Generic;
using Exanite.Arpg.Networking;
using LiteNetLib.Utils;
using UnityEngine;

namespace Prototype.Networking.Players.Packets
{
    /// <summary>
    /// Server to Client - Sent when a player connects to the server
    /// </summary>
    public class PlayerConnectedPacket : IPacket
    {
        public List<NewPlayer> newPlayers = new List<NewPlayer>(); // todo Change to only include one player

        public PlayerConnectedPacket() { }

        public PlayerConnectedPacket(ICollection<Player> players)
        {
            newPlayers.Capacity = players.Count;

            foreach (var player in players)
            {
                newPlayers.Add(new NewPlayer(player.Id, player.character.transform.position));
            }
        }

        public void Deserialize(NetDataReader reader)
        {
            int count = reader.GetInt();

            newPlayers.Clear();
            
            if(newPlayers.Capacity < count)
            {
                newPlayers.Capacity = count;
            }

            for (int i = 0; i < count; i++)
            {
                newPlayers.Add(new NewPlayer(reader.GetInt(), reader.GetVector2()));
            }
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(newPlayers.Count);

            for (int i = 0; i < newPlayers.Count; i++)
            {
                writer.Put(newPlayers[i].id);
                writer.Put(newPlayers[i].position);
            }
        }

        public struct NewPlayer
        {
            public int id;
            public Vector2 position;

            public NewPlayer(int id, Vector2 position)
            {
                this.id = id;
                this.position = position;
            }
        }
    }
}
