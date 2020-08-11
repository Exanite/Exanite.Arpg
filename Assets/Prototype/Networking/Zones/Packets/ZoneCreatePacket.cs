using System;
using Exanite.Arpg.Networking;
using LiteNetLib.Utils;

namespace Prototype.Networking.Zones.Packets
{
    public class ZoneCreatePacket : IPacket
    {
        public Guid guid;

        // zone data here

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
