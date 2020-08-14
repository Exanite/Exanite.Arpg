using System;
using System.Collections.Generic;
using Exanite.Arpg.Logging;
using Exanite.Arpg.Networking;
using Exanite.Arpg.Networking.Server;
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

        public Zone mainZone;

        private ILog log;
        private UnityServer server;
        private ServerPlayerManager playerManager;

        [Inject]
        public void Inject(ILog log, UnityServer server, ServerPlayerManager playerManager)
        {
            this.log = log;
            this.server = server;
            this.playerManager = playerManager;
        }

        public Zone GetMainZone()
        {
            if (mainZone == null)
            {
                mainZone = new Zone();
                zones.Add(mainZone.guid, mainZone);
            }

            return mainZone;
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
            if (playerManager.TryGetPlayer(sender.Id, out ServerPlayer newPlayer))
            {
                var zone = zones[e.guid];

                if (!newPlayer.isLoadingZone || newPlayer.currentZone != zone)
                {
                    log.Warning("Player with Id '{Id}' attempted to enter invalid zone", sender.Id);
                    return;
                }

                newPlayer.CreatePlayerCharacter(zone);
                zone.AddPlayer(newPlayer);

                var packet = new ZonePlayerEnterPacket();
                foreach (ServerPlayer playerInZone in zone.playersById.Values)
                {
                    packet.playerId = newPlayer.Id;
                    packet.playerPosition = newPlayer.character.transform.position;
                    server.SendPacket(playerInZone.Connection.Peer, packet, DeliveryMethod.ReliableOrdered);

                    packet.playerId = playerInZone.Id;
                    packet.playerPosition = playerInZone.character.transform.position;
                    server.SendPacket(newPlayer.Connection.Peer, packet, DeliveryMethod.ReliableOrdered);
                }

                newPlayer.isLoadingZone = false;
            }
        }
    }
}
