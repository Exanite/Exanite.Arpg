using System;
using Exanite.Arpg.Networking;
using LiteNetLib.Utils;

namespace Prototype.Networking.Zones.Packets
{
    public class ZoneCreateFinishedPacket : IPacket
    {
        public Guid guid;

        // ? maybe add a hash or something for validating the generated zone

        public void Deserialize(NetDataReader reader)
        {
            guid = new Guid(reader.GetBytesWithLength());
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.PutBytesWithLength(guid.ToByteArray());
        }
    }
}
