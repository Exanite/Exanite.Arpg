using System;
using Exanite.Arpg.Logging;
using Exanite.Arpg.Networking.Client;
using LiteNetLib;
using Prototype.Networking.Players;
using Prototype.Networking.Players.Packets;
using Prototype.Networking.Startup;
using Prototype.Networking.Zones;
using UniRx.Async;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Prototype.Networking.Client
{
    public class ClientGameManager : MonoBehaviour
    {
        public UnityClient client;

        public Player localPlayer;

        private ILog log;
        private GameStartSettings startSettings;
        private Scene scene;
        private ClientZoneManager zoneManager;

        [Inject]
        public void Inject(ILog log, GameStartSettings startSettings, Scene scene, ClientZoneManager zoneManager)
        {
            this.log = log;
            this.startSettings = startSettings;
            this.scene = scene;
            this.zoneManager = zoneManager;
        }

        private void Start()
        {
            if (startSettings.gameType != GameStartSettings.GameType.Client)
            {
                throw new ArgumentException(nameof(startSettings.gameType));
            }

            client.IPAddress = startSettings.address;
            client.Port = startSettings.port;

            Connect();
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
            SceneManager.UnloadSceneAsync(scene);
            SceneManager.UnloadSceneAsync(zoneManager.currentZone.scene);
        }

        private void OnPlayerIdAssignment(NetPeer sender, PlayerIdAssignmentPacket e)
        {
            localPlayer = new Player(e.id, zoneManager);
        }

        private void OnPlayerPositionUpdate(NetPeer sender, PlayerPositionUpdatePacket e)
        {
            if (zoneManager.currentZone.playersById.TryGetValue(e.playerId, out Player player))
            {
                if (player.Character)
                {
                    player.Character.transform.position = e.playerPosition;
                }
            }
        }
    }
}
