using System;
using Exanite.Arpg.Networking;
using LiteNetLib.Utils;

namespace Prototype.Networking.Zones.Packets
{
    public class ZoneLoadPacket : IPacket
    {
        public Guid guid;

        // zone data here

        public void Deserialize(NetDataReader reader)
        {
            guid = reader.GetGuid();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(guid);
        }
    }
}
