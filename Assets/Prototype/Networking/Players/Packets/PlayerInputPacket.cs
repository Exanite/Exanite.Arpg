using Exanite.Arpg.Networking;
using LiteNetLib.Utils;
using Prototype.Networking.Players.Data;

namespace Prototype.Networking.Players.Packets
{
    public class PlayerInputPacket : IPacket
    {
        public uint tick;

        public PlayerInputData data;

        public void Deserialize(NetDataReader reader)
        {
            tick = reader.GetUInt();

            data.Deserialize(reader);
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(tick);

            data.Serialize(writer);
        }
    }
}
