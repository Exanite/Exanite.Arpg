﻿using System;
using System.Collections.Generic;
using System.Linq;
using Exanite.Arpg.Logging;
using Exanite.Arpg.Networking;
using Exanite.Arpg.Networking.Server;
using LiteNetLib;
using Prototype.Networking.Players;
using Prototype.Networking.Zones.Packets;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Prototype.Networking.Zones
{
    public class ServerZoneManager : ZoneManager, IPacketHandler
    {
        public Dictionary<Guid, Zone> zones = new Dictionary<Guid, Zone>();

        [SerializeField] private int publicZoneCount = 3; // for testing
        public List<Zone> publicZones;

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

        public override Zone GetZoneWithPlayer(Player player)
        {
            // ! replace with Dictionary<Player, Zone> lookup: O(n) -> O(1)

            foreach (var zone in zones.Values)
            {
                if (zone.playersById.ContainsValue(player))
                {
                    return zone;
                }
            }

            return null;
        }

        public Zone GetOpenZone()
        {
            if (publicZones == null)
            {
                publicZones = new List<Zone>(publicZoneCount);

                for (int i = 0; i < publicZoneCount; i++)
                {
                    var zone = new Zone();

                    publicZones.Add(zone);
                    zones.Add(zone.guid, zone);
                }
            }

            return publicZones.OrderBy(x => Random.value).First();
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
                var zone = zones[e.guid]; // also check if zone exists first

                if (!newPlayer.isLoadingZone || newPlayer.CurrentZone != zone)
                {
                    log.Warning("Player with Id '{Id}' attempted to enter invalid zone", sender.Id);
                    return;
                }

                newPlayer.CreatePlayerCharacter();
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
