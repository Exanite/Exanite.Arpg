using Exanite.Arpg.Networking;
using LiteNetLib.Utils;
using UnityEngine;

namespace Prototype.Networking.Players.Data
{
    public struct PlayerUpdateData : INetSerializable
    {
        public Vector3 position;

        public void Deserialize(NetDataReader reader)
        {
            position = reader.GetVector3();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(position);
        }
    }
}
