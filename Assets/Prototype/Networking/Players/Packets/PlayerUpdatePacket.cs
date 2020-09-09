using Exanite.Arpg.Networking;
using LiteNetLib.Utils;
using UnityEngine;

namespace Prototype.Networking.Players.Packets
{
    public class PlayerUpdatePacket : IPacket
    {
        public int tick;

        public int playerId;
        public Vector3 playerPosition;

        public void Deserialize(NetDataReader reader)
        {
            tick = reader.GetInt();

            playerId = reader.GetInt();
            playerPosition = reader.GetVector3();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(tick);

            writer.Put(playerId);
            writer.Put(playerPosition);
        }
    }
}
