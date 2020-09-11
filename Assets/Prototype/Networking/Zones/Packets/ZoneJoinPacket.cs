using System;
using System.Collections.Generic;
using Exanite.Arpg.Networking;
using LiteNetLib.Utils;
using Prototype.Networking.Players.Data;

namespace Prototype.Networking.Zones.Packets
{
    public class ZoneJoinPacket : IPacket
    {
        public Guid guid;
        public uint tick;

        public PlayerCreateData localPlayer;
        public List<PlayerCreateData> zonePlayers = new List<PlayerCreateData>();

        public void Deserialize(NetDataReader reader)
        {
            guid = reader.GetGuid();
            tick = reader.GetUInt();

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
