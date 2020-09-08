using System;
using Cysharp.Threading.Tasks;
using Exanite.Arpg;
using Exanite.Arpg.Logging;
using Exanite.Arpg.Networking.Client;
using LiteNetLib;
using Prototype.Networking.Players;
using Prototype.Networking.Players.Packets;
using Prototype.Networking.Startup;
using Prototype.Networking.Zones;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Prototype.Networking.Client
{
    public class ClientGameManager : MonoBehaviour
    {
        public UnityClient client;
        public Material glMaterial;
        public PlayerCharacter playerCharacterPrefab;

        public Player localPlayer;

        private ILog log;
        private GameStartSettings startSettings;
        private ClientZoneManager zoneManager;
        private Scene scene;
        private SceneLoader sceneLoader;

        [Inject]
        public void Inject(ILog log, GameStartSettings startSettings, ClientZoneManager zoneManager, Scene scene, SceneLoader sceneLoader)
        {
            this.log = log;
            this.startSettings = startSettings;
            this.zoneManager = zoneManager;
            this.scene = scene;
            this.sceneLoader = sceneLoader;
        }

        private void Start()
        {
            if (startSettings.gameType != GameType.Client)
            {
                throw new ArgumentException(nameof(startSettings.gameType));
            }

            client.IPAddress = startSettings.address;
            client.Port = startSettings.port;

            Connect();
        }

        private void OnRenderObject()
        {
            if (!startSettings.useAI && zoneManager.currentZone != null)
            {
                foreach (var player in zoneManager.currentZone.Players)
                {
                    if (player.Character)
                    {
                        Color color = player == localPlayer ? Color.blue : Color.red;

                        player.Character.DrawWithGL(glMaterial, color);
                    }
                }
            }
        }

        public void Connect()
        {
            RegisterEvents();

            client.ConnectAsync().ContinueWith(x =>
            {
                if (x.IsSuccess)
                {
                    log.Information("Connected to {IP} on port {Port}", client.IPAddress, client.Port);
                }
                else
                {
                    log.Information("Failed to connect to {IP} on port {Port}. Reason: {FailReason}", client.IPAddress, client.Port, x.FailReason);
                }
            })
            .Forget();
        }

        public void Disconnect()
        {
            client.Disconnect();
            UnregisterEvents();
        }

        private void RegisterEvents()
        {
            client.DisconnectedEvent += OnDisconnected;

            client.RegisterPacketReceiver<PlayerIdAssignmentPacket>(OnPlayerIdAssignment);
            client.RegisterPacketReceiver<PlayerPositionUpdatePacket>(OnPlayerPositionUpdate);

            zoneManager.RegisterPackets(client);
        }

        private void UnregisterEvents()
        {
            zoneManager.UnregisterPackets(client);

            client.ClearPacketReceiver<PlayerPositionUpdatePacket>();
            client.ClearPacketReceiver<PlayerIdAssignmentPacket>();

            client.DisconnectedEvent -= OnDisconnected;
        }

        private void OnDisconnected(UnityClient sender, DisconnectedEventArgs e)
        {
            sceneLoader.UnloadScene(scene).Forget();
            zoneManager.currentZone?.Destroy(sceneLoader).Forget();
        }

        private void OnPlayerIdAssignment(NetPeer sender, PlayerIdAssignmentPacket e)
        {
            localPlayer = new Player(e.id, zoneManager, false, true);
        }

        private void OnPlayerPositionUpdate(NetPeer sender, PlayerPositionUpdatePacket e)
        {
            if (zoneManager.currentZone.PlayersById.TryGetValue(e.playerId, out Player player))
            {
                player.Character?.UpdatePosition(e.playerPosition, e.tick);
            }
        }
    }
}
