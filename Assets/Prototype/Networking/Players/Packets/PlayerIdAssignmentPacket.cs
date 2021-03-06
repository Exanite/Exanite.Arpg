﻿using Exanite.Arpg.Networking;
using LiteNetLib.Utils;

namespace Prototype.Networking.Players.Packets
{
    public class PlayerIdAssignmentPacket : IPacket
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
