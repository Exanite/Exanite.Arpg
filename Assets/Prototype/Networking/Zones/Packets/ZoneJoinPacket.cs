using System;
using System.Collections.Generic;
using Exanite.Arpg.Networking;
using LiteNetLib.Utils;
using Prototype.Networking.Players.Packets;

namespace Prototype.Networking.Zones.Packets
{
    public class ZoneJoinPacket : IPacket
    {
        public Guid guid;
        public int tick;

        public PlayerCreateData localPlayer = new PlayerCreateData();
        public List<PlayerCreateData> zonePlayers = new List<PlayerCreateData>();

        public void Deserialize(NetDataReader reader)
        {
            guid = reader.GetGuid();
            tick = reader.GetInt();

            localPlayer.Deserialize(reader);
            reader.GetListWithCount(zonePlayers);
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(guid);
            writer.Put(tick);

            localPlayer.Serialize(writer);
            writer.PutListWithCount(zonePlayers);
        }
    }
}
