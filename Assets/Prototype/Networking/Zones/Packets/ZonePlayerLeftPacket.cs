using Exanite.Arpg.Networking;
using LiteNetLib.Utils;

namespace Prototype.Networking.Zones.Packets
{
    public class ZonePlayerLeftPacket : IPacket
    {
        public int playerId;

        public void Deserialize(NetDataReader reader)
        {
            playerId = reader.GetInt();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(playerId);
        }
    }
}
