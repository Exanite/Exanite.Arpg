using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Exanite.Arpg;
using Exanite.Arpg.Logging;
using Exanite.Arpg.Networking;
using Exanite.Arpg.Networking.Server;
using LiteNetLib;
using Prototype.Networking.Players;
using Prototype.Networking.Players.Data;
using Prototype.Networking.Zones;
using Prototype.Networking.Zones.Packets;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Random = UnityEngine.Random;

namespace Prototype.Networking.Server
{
    public class ServerZoneManager : ZoneManager, IPacketHandler
    {
        private const int publicZoneCount = 1; // ! for testing

        [SerializeField] private string zoneSceneName = "Zone";

        public List<Zone> publicZones;

        public Dictionary<Guid, Zone> zones = new Dictionary<Guid, Zone>();
        public Dictionary<Player, Zone> loadingPlayers = new Dictionary<Player, Zone>();

        private bool isCreatingPublicZones;

        private ILog log;
        private UnityServer server;
        private ServerGameManager gameManager;
        private ServerPlayerManager playerManager;
        private Scene scene;
        private SceneLoader sceneLoader;
        private SceneContextRegistry sceneContextRegistry;

        [Inject]
        public void Inject(ILog log, UnityServer server, ServerGameManager gameManager, ServerPlayerManager playerManager, Scene scene, SceneLoader sceneLoader, SceneContextRegistry sceneContextRegistry)
        {
            this.log = log;
            this.server = server;
            this.gameManager = gameManager;
            this.playerManager = playerManager;
            this.scene = scene;
            this.sceneLoader = sceneLoader;
            this.sceneContextRegistry = sceneContextRegistry;
        }

        public event EventHandler<ServerZoneManager, Zone> ZoneAddedEvent;
        public event EventHandler<ServerZoneManager, Zone> ZoneRemovedEvent;

        public async UniTask<Zone> CreateZone()
        {
            var zone = new Zone(true);
            await zone.Create(zoneSceneName, scene, sceneLoader);

            AddZone(zone);
            return zone;
        }

        public void AddZone(Zone zone)
        {
            if (!zones.ContainsKey(zone.Guid))
            {
                zones.Add(zone.Guid, zone);

                zone.PlayerEnteredEvent += OnZonePlayerEntered;
                zone.PlayerLeftEvent += OnZonePlayerLeft;

                ZoneAddedEvent?.Invoke(this, zone);
            }
        }

        public void RemoveZone(Zone zone)
        {
            if (zones.Remove(zone.Guid))
            {
                zone.PlayerEnteredEvent -= OnZonePlayerEntered;
                zone.PlayerLeftEvent -= OnZonePlayerLeft;

                ZoneRemovedEvent?.Invoke(this, zone);
            }
        }

        public override Zone GetPlayerCurrentZone(Player player)
        {
            // ! replace with Dictionary<Player, Zone> lookup: O(n) -> O(1)

            foreach (var zone in zones.Values)
            {
                if (zone.PlayersById.ContainsKey(player.Id))
                {
                    return zone;
                }
            }

            return null;
        }

        public async UniTask<Zone> GetOpenZone()
        {
            if (publicZones == null)
            {
                await CreatePublicZones();
            }

            await UniTask.WaitWhile(() => isCreatingPublicZones);

            int index = Random.Range(0, publicZones.Count);

            return publicZones[index];
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
            server.SendPacket(player.Connection.Peer, new ZoneLoadPacket() { guid = zone.Guid }, DeliveryMethod.ReliableOrdered);
        }

        public void RegisterPackets(UnityNetwork network)
        {
            network.RegisterPacketReceiver<ZoneLoadFinishedPacket>(OnZoneLoadFinished);
        }

        public void UnregisterPackets(UnityNetwork network)
        {
            network.ClearPacketReceiver<ZoneLoadFinishedPacket>();
        }

        private async UniTask CreatePublicZones()
        {
            isCreatingPublicZones = true;

            publicZones = new List<Zone>(publicZoneCount);

            for (int i = 0; i < publicZoneCount; i++)
            {
                var zone = await CreateZone();

                publicZones.Add(zone);
            }

            isCreatingPublicZones = false;
        }

        private void OnZonePlayerEntered(Zone sender, Player e)
        {
            var newPlayer = (ServerPlayer)e;

            newPlayer.CreatePlayerCharacter(gameManager.playerCharacterPrefab, sceneContextRegistry);

            var playerEnterPacket = new ZonePlayerEnteredPacket();
            var joinPacket = new ZoneJoinPacket();

            joinPacket.guid = sender.Guid;
            joinPacket.tick = sender.Tick;
            joinPacket.localPlayer = new PlayerCreateData(newPlayer.Id, newPlayer.Character.Interpolation.current);

            foreach (ServerPlayer player in sender.Players)
            {
                if (player == newPlayer)
                {
                    continue;
                }

                joinPacket.zonePlayers.Add(new PlayerCreateData(player.Id, player.Character.Interpolation.current));

                playerEnterPacket.data.PlayerId = newPlayer.Id;
                playerEnterPacket.data.UpdateData = newPlayer.Character.Interpolation.current;

                server.SendPacket(player.Connection.Peer, playerEnterPacket, DeliveryMethod.ReliableOrdered);
            }

            server.SendPacket(newPlayer.Connection.Peer, joinPacket, DeliveryMethod.ReliableOrdered);
        }

        private void OnZonePlayerLeft(Zone sender, Player e)
        {
            var packet = new ZonePlayerLeftPacket()
            {
                playerId = e.Id,
            };

            foreach (ServerPlayer player in sender.Players)
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

            if (!loadingPlayers.TryGetValue(loadingPlayer, out Zone zone) || zone.Guid != e.guid)
            {
                log.Warning("Player with Id '{Id}' attempted to enter invalid zone", sender.Id);
                return;
            }

            zone.AddPlayer(loadingPlayer);
            loadingPlayers.Remove(loadingPlayer);
        }
    }
}
