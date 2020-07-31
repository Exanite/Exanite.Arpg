using Exanite.Arpg.Networking;
using LiteNetLib.Utils;

namespace Prototype.LiteNetLib.Shared.Packets
{
    public class PlayerIDAssignmentPacket : IPacket
    {
        public int id;

        public void Deserialize(NetDataReader reader)
        {
            id = reader.GetInt();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(id);
        }
    }
}
