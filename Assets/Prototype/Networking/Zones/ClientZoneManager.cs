using System;
using Exanite.Arpg;
using Exanite.Arpg.Networking;
using Exanite.Arpg.Networking.Client;
using LiteNetLib;
using Prototype.Networking.Client;
using Prototype.Networking.Players;
using Prototype.Networking.Startup;
using Prototype.Networking.Zones.Packets;
using UniRx.Async;
using UnityEngine.SceneManagement;
using Zenject;

namespace Prototype.Networking.Zones
{
    public class ClientZoneManager : ZoneManager, IPacketHandler
    {
        public string zoneSceneName = "Zone";

        public Zone currentZone;
        public bool isLoadingZone;

        private UnityClient client;
        private GameStartSettings startSettings;
        private ClientGameManager gameManager;
        private Scene scene;
        private SceneLoader sceneLoader;

        [Inject]
        public void Inject(UnityClient client, GameStartSettings startSettings, ClientGameManager gameManager, Scene scene, SceneLoader sceneLoader)
        {
            this.client = client;
            this.startSettings = startSettings;
            this.gameManager = gameManager;
            this.scene = scene;
            this.sceneLoader = sceneLoader;
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
            if (currentZone.playersById.ContainsValue(player))
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
            network.RegisterPacketReceiver<ZonePlayerEnteredPacket>(OnZonePlayerEntered);
            network.RegisterPacketReceiver<ZonePlayerLeftPacket>(OnZonePlayerLeft);
        }

        public void UnregisterPackets(UnityNetwork network)
        {
            network.ClearPacketReceiver<ZoneLoadPacket>();
            network.ClearPacketReceiver<ZonePlayerEnteredPacket>();
            network.ClearPacketReceiver<ZonePlayerLeftPacket>();
        }

        private void OnZoneLoad(NetPeer sender, ZoneLoadPacket e)
        {
            LoadZoneAsync(e.guid).Forget();
        }

        private void OnZonePlayerEntered(NetPeer sender, ZonePlayerEnteredPacket e)
        {
            if (!currentZone.playersById.ContainsKey(e.playerId))
            {
                Player player;
                if (LocalPlayer.Id == e.playerId)
                {
                    player = LocalPlayer;
                }
                else
                {
                    player = new Player(e.playerId, this);
                }

                currentZone.AddPlayer(player);

                player.CreatePlayerCharacter();
                player.Character.transform.position = e.playerPosition;

                if (e.playerId == LocalPlayer.Id) // works for now, but try avoiding checking the Id twice
                {
                    var controller = player.Character.gameObject.AddComponent<PlayerController>();
                    controller.useAI = startSettings.useAI;
                    controller.player = player;
                    controller.client = client;

                    player.Character.name += " (Local)";
                }
            }
        }

        private void OnZonePlayerLeft(NetPeer sender, ZonePlayerLeftPacket e)
        {
            if (currentZone.playersById.TryGetValue(e.playerId, out Player player))
            {
                if (player.Character)
                {
                    Destroy(player.Character.gameObject);
                }

                currentZone.RemovePlayer(player);
            }
        }

        private async UniTask LoadZoneAsync(Guid zoneGuid)
        {
            isLoadingZone = true;

            if (currentZone != null)
            {
                await SceneManager.UnloadSceneAsync(currentZone.scene);
            }

            var newZone = new Zone(zoneGuid, zoneSceneName);
            await newZone.CreateZone(zoneSceneName, scene, sceneLoader);

            client.SendPacketToServer(new ZoneLoadFinishedPacket() { guid = zoneGuid }, DeliveryMethod.ReliableOrdered);
            currentZone = newZone;
            isLoadingZone = false;
        }
    }
}
