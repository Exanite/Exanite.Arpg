using Exanite.Arpg.Networking;
using LiteNetLib.Utils;
using UnityEngine;

namespace Prototype.Networking.Zones.Packets
{
    public class ZonePlayerEnteredPacket : IPacket
    {
        public int playerId;
        public Vector3 playerPosition;

        public void Deserialize(NetDataReader reader)
        {
            playerId = reader.GetInt();
            playerPosition = reader.GetVector3();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(playerId);
            writer.Put(playerPosition);
        }
    }
}
