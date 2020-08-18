﻿using System;
using System.Collections.Generic;
using System.Linq;
using Exanite.Arpg;
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
        [SerializeField] private int publicZoneCount = 3; // for testing
        [SerializeField] private string zoneSceneName = "Zone";

        public List<Zone> publicZones;

        public Dictionary<Guid, Zone> zones = new Dictionary<Guid, Zone>();
        public Dictionary<Player, Zone> loadingPlayers = new Dictionary<Player, Zone>();

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

        public event EventHandler<ServerZoneManager, Zone> ZoneAddedEvent;
        public event EventHandler<ServerZoneManager, Zone> ZoneRemovedEvent;

        public void AddZone(Zone zone)
        {
            if (!zones.ContainsKey(zone.guid))
            {
                zones.Add(zone.guid, zone);

                zone.PlayerEnteredEvent += OnZonePlayerEntered;
                zone.PlayerLeftEvent += OnZonePlayerLeft;

                ZoneAddedEvent?.Invoke(this, zone);
            }
        }

        public void RemoveZone(Zone zone)
        {
            if (zones.Remove(zone.guid))
            {
                zone.PlayerEnteredEvent -= OnZonePlayerEntered;
                zone.PlayerLeftEvent -= OnZonePlayerLeft;

                ZoneRemovedEvent?.Invoke(this, zone);
            }
        }

        // ! add method to base class as well
        public Zone CreateZone()
        {
            var zone = new Zone(zoneSceneName);
            AddZone(zone);

            return zone;
        }

        public override Zone GetPlayerCurrentZone(Player player)
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
                CreatePublicZones();
            }

            return publicZones.OrderBy(x => Random.value).First();
        }

        public override bool IsPlayerLoading(Player player)
        {
            return loadingPlayers.ContainsKey(player);
        }

        public void MovePlayerToZone(ServerPlayer player, Zone zone)
        {
            var previousZone = GetPlayerCurrentZone(player);

            if (previousZone == zone || player.IsLoadingZone)
            {
                // already in zone or loading
                return;
            }

            if (previousZone != null)
            {
                previousZone.RemovePlayer(player);
                Destroy(player.Character.gameObject);
            }

            loadingPlayers.Add(player, zone);
            server.SendPacket(player.Connection.Peer, new ZoneLoadPacket() { guid = zone.guid }, DeliveryMethod.ReliableOrdered);
        }

        public void RegisterPackets(UnityNetwork network)
        {
            network.RegisterPacketReceiver<ZoneLoadFinishedPacket>(OnZoneLoadFinished);
        }

        public void UnregisterPackets(UnityNetwork network)
        {
            network.ClearPacketReceiver<ZoneLoadFinishedPacket>();
        }

        private void CreatePublicZones()
        {
            publicZones = new List<Zone>(publicZoneCount);

            for (int i = 0; i < publicZoneCount; i++)
            {
                var zone = CreateZone();

                publicZones.Add(zone);
            }
        }

        private void OnZonePlayerEntered(Zone sender, Player e)
        {
            e.CreatePlayerCharacter();

            var packet = new ZonePlayerEnteredPacket();
            foreach (ServerPlayer player in sender.playersById.Values)
            {
                packet.playerId = e.Id;
                packet.playerPosition = e.Character.transform.position;
                server.SendPacket(player.Connection.Peer, packet, DeliveryMethod.ReliableOrdered);

                packet.playerId = player.Id;
                packet.playerPosition = player.Character.transform.position;
                server.SendPacket(((ServerPlayer)e).Connection.Peer, packet, DeliveryMethod.ReliableOrdered);
            }
        }

        private void OnZonePlayerLeft(Zone sender, Player e)
        {
            var packet = new ZonePlayerLeftPacket()
            {
                playerId = e.Id,
            };

            foreach (ServerPlayer player in sender.playersById.Values)
            {
                server.SendPacket(player.Connection.Peer, packet, DeliveryMethod.ReliableOrdered);
            }
        }

        private void OnZoneLoadFinished(NetPeer sender, ZoneLoadFinishedPacket e)
        {
            if (!playerManager.TryGetPlayer(sender.Id, out ServerPlayer loadingPlayer))
            {
                return;
            }

            if (!loadingPlayers.TryGetValue(loadingPlayer, out Zone zone) || zone.guid != e.guid)
            {
                log.Warning("Player with Id '{Id}' attempted to enter invalid zone", sender.Id);
                return;
            }

            zone.AddPlayer(loadingPlayer);
            loadingPlayers.Remove(loadingPlayer);
        }
    }
}
