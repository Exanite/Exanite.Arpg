using System;
using Exanite.Arpg.Networking;
using LiteNetLib.Utils;

namespace Prototype.Networking.Zones.Packets
{
    public class ZoneLoadFinishedPacket : IPacket
    {
        public Guid guid;

        // ? maybe add a hash or something for validating the generated zone

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
