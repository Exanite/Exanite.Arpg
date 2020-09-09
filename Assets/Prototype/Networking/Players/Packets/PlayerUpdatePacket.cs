using Exanite.Arpg.Networking;
using LiteNetLib.Utils;
using Prototype.Networking.Players.Data;

namespace Prototype.Networking.Players.Packets
{
    public class PlayerUpdatePacket : IPacket
    {
        public int playerId;

        public PlayerUpdateData data;

        public void Deserialize(NetDataReader reader)
        {
            playerId = reader.GetInt();
            data.Deserialize(reader);
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(playerId);
            data.Serialize(writer);
        }
    }
}
