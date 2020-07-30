using Exanite.Arpg.NewNetworking;
using LiteNetLib.Utils;
using UnityEngine;

namespace Prototype.DarkRift.Shared.Packets
{
    public class PlayerInputPacket : IPacket
    {
        public Vector2 movementInput;

        public void Deserialize(NetDataReader reader)
        {
            movementInput = reader.GetVector2();

            movementInput = Vector2.ClampMagnitude(movementInput, 1);
        }

        public void Serialize(NetDataWriter writer)
        {
            movementInput = Vector2.ClampMagnitude(movementInput, 1);

            writer.Put(movementInput);
        }
    }
}
