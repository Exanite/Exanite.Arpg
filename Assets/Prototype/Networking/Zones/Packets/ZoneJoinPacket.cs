using System;
using System.Collections.Generic;
using Exanite.Arpg.Networking;
using LiteNetLib.Utils;
using Prototype.Networking.Players;

namespace Prototype.Networking.Zones.Packets
{
    public class ZoneJoinPacket : IPacket
    {
        public Guid guid;

        public PlayerCreateData localPlayer = new PlayerCreateData();
        public List<PlayerCreateData> zonePlayers = new List<PlayerCreateData>();

        public void Deserialize(NetDataReader reader)
        {
            guid = reader.GetGuid();
            localPlayer.Deserialize(reader);

            zonePlayers.Clear();
            int count = reader.GetInt();
            for (int i = 0; i < count; i++)
            {
                zonePlayers.Add(reader.Get<PlayerCreateData>());
            }
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(guid);
            localPlayer.Serialize(writer);

            writer.Put(zonePlayers.Count);
            for (int i = 0; i < zonePlayers.Count; i++)
            {
                writer.Put(zonePlayers[i]);
            }
        }
    }
}
