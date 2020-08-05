using Exanite.Arpg.Networking;
using LiteNetLib.Utils;

namespace Prototype.Networking.Players.Packets
{
    /// <summary>
    /// Server to Client - Sent when a player disconnects from the server
    /// </summary>
    public class PlayerDisconnectedPacket : IPacket
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
