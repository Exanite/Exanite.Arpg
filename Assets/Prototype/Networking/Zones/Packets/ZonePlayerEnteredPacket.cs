using Exanite.Arpg.Networking;
using LiteNetLib.Utils;
using Prototype.Networking.Players.Packets;

namespace Prototype.Networking.Zones.Packets
{
    public class ZonePlayerEnteredPacket : IPacket
    {
        public PlayerCreateData data = new PlayerCreateData();

        public void Deserialize(NetDataReader reader)
        {
            data.Deserialize(reader);
        }

        public void Serialize(NetDataWriter writer)
        {
            data.Serialize(writer);
        }
    }
}
