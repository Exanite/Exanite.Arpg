﻿using System;
using Cysharp.Threading.Tasks;
using Exanite.Arpg;
using Exanite.Arpg.Logging;
using Exanite.Arpg.Networking;
using Exanite.Arpg.Networking.Client;
using LiteNetLib;
using Prototype.Networking.Players;
using Prototype.Networking.Players.Data;
using Prototype.Networking.Startup;
using Prototype.Networking.Zones;
using Prototype.Networking.Zones.Packets;
using UnityEngine.SceneManagement;
using Zenject;

namespace Prototype.Networking.Client
{
    public class ClientZoneManager : ZoneManager, IPacketHandler
    {
        public string zoneSceneName = "Zone";

        public Zone currentZone;
        public bool isLoadingZone;

        private ILog log;
        private UnityClient client;
        private GameStartSettings startSettings;
        private ClientGameManager gameManager;
        private Scene scene;
        private SceneLoader sceneLoader;
        private SceneContextRegistry sceneContextRegistry; // ! temporary

        [Inject]
        public void Inject(ILog log, UnityClient client, GameStartSettings startSettings, ClientGameManager gameManager, Scene scene, SceneLoader sceneLoader, SceneContextRegistry sceneContextRegistry)
        {
            this.log = log;
            this.client = client;
            this.startSettings = startSettings;
            this.gameManager = gameManager;
            this.scene = scene;
            this.sceneLoader = sceneLoader;
            this.sceneContextRegistry = sceneContextRegistry;
        }

        private Player LocalPlayer
        {
            get
            {
                return gameManager.localPlayer;
            }
        }

        public override Zone GetPlayerCurrentZone(Player player)
        {
            if (currentZone.PlayersById.ContainsKey(player.Id))
            {
                return currentZone;
            }

            return null;
        }

        public override bool IsPlayerLoading(Player player)
        {
            return isLoadingZone;
        }

        public void RegisterPackets(UnityNetwork network)
        {
            network.RegisterPacketReceiver<ZoneLoadPacket>(OnZoneLoad);
            network.RegisterPacketReceiver<ZoneJoinPacket>(OnZoneJoin);
            network.RegisterPacketReceiver<ZonePlayerEnteredPacket>(OnZonePlayerEntered);
            network.RegisterPacketReceiver<ZonePlayerLeftPacket>(OnZonePlayerLeft);
        }

        public void UnregisterPackets(UnityNetwork network)
        {
            network.ClearPacketReceiver<ZoneLoadPacket>();
            network.ClearPacketReceiver<ZoneJoinPacket>();
            network.ClearPacketReceiver<ZonePlayerEnteredPacket>();
            network.ClearPacketReceiver<ZonePlayerLeftPacket>();
        }

        private void OnZoneLoad(NetPeer sender, ZoneLoadPacket e)
        {
            LoadZoneAsync(e.guid).Forget();
        }

        private void OnZoneJoin(NetPeer sender, ZoneJoinPacket e)
        {
            if (currentZone.Guid != e.guid)
            {
                log.Warning("Expected to recieve ZoneJoinPacket with Guid '{Expected}', but recieved '{Actual}' instead", currentZone.Guid, e.guid);
                return;
            }

            currentZone.Tick = e.tick;

            CreateLocalPlayer(e.localPlayer);

            foreach (var playerCreateData in e.zonePlayers)
            {
                CreatePlayer(playerCreateData);
            }
        }

        private void OnZonePlayerEntered(NetPeer sender, ZonePlayerEnteredPacket e)
        {
            CreatePlayer(e.data);
        }

        private void OnZonePlayerLeft(NetPeer sender, ZonePlayerLeftPacket e)
        {
            if (currentZone.PlayersById.TryGetValue(e.playerId, out Player player))
            {
                if (player.Character)
                {
                    Destroy(player.Character.gameObject);
                }

                currentZone.RemovePlayer(player);
            }
        }

        private void CreatePlayer(PlayerCreateData data, Player player = null)
        {
            if (currentZone.PlayersById.ContainsKey(data.PlayerId))
            {
                log.Warning("Cannot create player that already exists");
            }

            if (player == null)
            {
                player = new Player(data.PlayerId, this, false, false);
            }

            currentZone.AddPlayer(player);

            player.CreatePlayerCharacter(gameManager.playerCharacterPrefab, sceneContextRegistry);
            player.Character.Interpolation.UpdateData(data.UpdateData, true);
        }

        private void CreateLocalPlayer(PlayerCreateData data)
        {
            CreatePlayer(data, LocalPlayer);

            LocalPlayer.Character.name += " (Local)";
        }

        private async UniTask LoadZoneAsync(Guid zoneGuid)
        {
            isLoadingZone = true;

            currentZone?.Destroy(sceneLoader);

            var newZone = new Zone(zoneGuid, false);
            await newZone.Create(zoneSceneName, scene, sceneLoader);

            client.SendPacketToServer(new ZoneLoadFinishedPacket() { guid = zoneGuid }, DeliveryMethod.ReliableOrdered);
            currentZone = newZone;
            isLoadingZone = false;
        }
    }
}
