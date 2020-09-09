using Exanite.Arpg.Networking;
using LiteNetLib.Utils;
using UnityEngine;

namespace Prototype.Networking.Players.Data
{
    public struct PlayerUpdateData : INetSerializable
    {
        public int tick;

        public Vector3 playerPosition;

        public void Deserialize(NetDataReader reader)
        {
            tick = reader.GetInt();

            playerPosition = reader.GetVector3();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(tick);

            writer.Put(playerPosition);
        }
    }
}
