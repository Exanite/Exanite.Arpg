using Exanite.Arpg.Networking;
using LiteNetLib.Utils;
using Prototype.Networking.Players.Data;

namespace Prototype.Networking.Players.Packets
{
    public class PlayerInputPacket : IPacket
    {
        public int tick; // ! unused

        public PlayerInputData data;

        public void Deserialize(NetDataReader reader)
        {
            tick = reader.GetInt();

            data.Deserialize(reader);
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(tick);

            data.Serialize(writer);
        }
    }
}
