using Exanite.Arpg.Networking;
using LiteNetLib.Utils;
using UnityEngine;

namespace Prototype.Networking.Players.Data
{
    public struct PlayerInputData : INetSerializable
    {
        public int tick;

        public Vector2 movement;

        public void Deserialize(NetDataReader reader)
        {
            tick = reader.GetInt();

            movement = reader.GetVector2();
            movement = Vector2.ClampMagnitude(movement, 1);
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(tick);

            movement = Vector2.ClampMagnitude(movement, 1);
            writer.Put(movement);
        }
    }
}
