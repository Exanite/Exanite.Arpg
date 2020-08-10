using System;
using System.Collections.Generic;
using Exanite.Arpg.Networking;
using LiteNetLib;
using Prototype.Networking.Players;
using Prototype.Networking.Zones.Packets;
using UnityEngine;
using Zenject;

namespace Prototype.Networking.Zones
{
    public class ServerZoneManager : MonoBehaviour, IPacketHandler
    {
        public Dictionary<Guid, Zone> zones = new Dictionary<Guid, Zone>();

        private ServerPlayerManager playerManager;

        [Inject]
        public void Inject(ServerPlayerManager playerManager)
        {
            this.playerManager = playerManager;
        }

        public void RegisterPackets(UnityNetwork network)
        {
            network.RegisterPacketReceiver<ZoneCreateFinishedPacket>(OnZoneCreateFinished);
        }

        public void UnregisterPackets(UnityNetwork network)
        {
            network.ClearPacketReceiver<ZoneCreateFinishedPacket>();
        }

        private void OnZoneCreateFinished(NetPeer sender, ZoneCreateFinishedPacket e)
        {
            if (playerManager.TryGetPlayer(sender.Id, out Player player))
            {
                player.CreatePlayerCharacter(zones[e.guid]); // ! should check if player is loading zone on server or not first

                // send ZonePlayerEnter packets to all in zone
                // this should one packet to player already in the zone and
                // many packets to the player that just entered the zone
            }
        }
    }
}
