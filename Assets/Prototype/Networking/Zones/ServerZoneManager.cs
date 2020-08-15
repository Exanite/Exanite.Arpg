using System;
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
        public Dictionary<Player, Zone> loadingPlayers = new Dictionary<Player, Zone>();

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

        public override bool IsPlayerLoading(Player player)
        {
            return loadingPlayers.ContainsKey(player);
        }

        public void MovePlayerToZone(ServerPlayer player, Zone zone)
        {
            var previousZone = GetZoneWithPlayer(player);
            if (previousZone != null)
            {
                previousZone.RemovePlayer(player);
                Destroy(player.Character);
            }

            loadingPlayers.Add(player, zone);
            server.SendPacket(player.Connection.Peer, new ZoneCreatePacket() { guid = zone.guid }, DeliveryMethod.ReliableOrdered);
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
            if (!playerManager.TryGetPlayer(sender.Id, out ServerPlayer newPlayer))
            {
                return;
            }

            if (!loadingPlayers.TryGetValue(newPlayer, out Zone zone) || zone.guid != e.guid)
            {
                log.Warning("Player with Id '{Id}' attempted to enter invalid zone", sender.Id);
                return;
            }

            zone.AddPlayer(newPlayer);
            newPlayer.CreatePlayerCharacter();

            var packet = new ZonePlayerEnterPacket();
            foreach (ServerPlayer playerInZone in zone.playersById.Values)
            {
                packet.playerId = newPlayer.Id;
                packet.playerPosition = newPlayer.Character.transform.position;
                server.SendPacket(playerInZone.Connection.Peer, packet, DeliveryMethod.ReliableOrdered);

                packet.playerId = playerInZone.Id;
                packet.playerPosition = playerInZone.Character.transform.position;
                server.SendPacket(newPlayer.Connection.Peer, packet, DeliveryMethod.ReliableOrdered);
            }

            loadingPlayers.Remove(newPlayer);
        }
    }
}
