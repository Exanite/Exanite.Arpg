using System;
using Cysharp.Threading.Tasks;
using Exanite.Arpg.Logging;
using Exanite.Arpg.Networking.Server;
using LiteNetLib;
using Prototype.Networking.Players;
using Prototype.Networking.Players.Data;
using Prototype.Networking.Players.Packets;
using Prototype.Networking.Startup;
using Prototype.Networking.Zones;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Prototype.Networking.Server
{
    public class ServerGameManager : MonoBehaviour
    {
        public UnityServer server;
        public Material glMaterial;
        public PlayerCharacter playerCharacterPrefab;

        private Zone selectedZone;

        private ILog log;
        private GameStartSettings startSettings;
        private Scene scene;
        private ServerPlayerManager playerManager;
        private ServerZoneManager zoneManager;

        [Inject]
        public void Inject(ILog log, GameStartSettings startSettings, Scene scene, ServerPlayerManager playerManager, ServerZoneManager zoneManager)
        {
            this.log = log;
            this.startSettings = startSettings;
            this.scene = scene;
            this.playerManager = playerManager;
            this.zoneManager = zoneManager;
        }

        private void Start()
        {
            if (startSettings.gameType != GameType.Server)
            {
                throw new ArgumentException(nameof(startSettings.gameType));
            }

            server.Port = startSettings.port;

            StartServer();

            //while (true) // for testing moving players between zones
            //{
            //    MoveRandomPlayerToRandomZone();

            //    await UniTask.Delay(TimeSpan.FromSeconds(1));
            //}
        }

        //private void MoveRandomPlayerToRandomZone() // for testing moving players between zones
        //{
        //    if (playerManager.PlayerCount > 0 && zoneManager.zones.Count > 0)
        //    {
        //        var player = playerManager.Players.OrderBy(x => Random.value).First();
        //        var zone = zoneManager.zones.OrderBy(x => Random.value).First().Value;

        //        zoneManager.MovePlayerToZone(player, zone);
        //    }
        //}

        private void Update() // for debug
        {
            if (zoneManager.publicZones == null)
            {
                return;
            }

            if (selectedZone == null && zoneManager.publicZones.Count > 0)
            {
                selectedZone = zoneManager.publicZones[0];
            }

            for (int i = 0; i < 9; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i) && zoneManager.publicZones.Count > i)
                {
                    selectedZone = zoneManager.publicZones[i];
                }
            }
        }

        private void FixedUpdate()
        {
            SendPositionUpdates();
        }

        private void OnRenderObject()
        {
            if (!Application.isPlaying || selectedZone == null)
            {
                return;
            }

            foreach (var player in selectedZone.Players)
            {
                if (player.Character)
                {
                    player.Character.DrawWithGL(glMaterial, Color.red);
                }
            }
        }

        private void OnGUI()
        {
            GUILayout.Label($"Selected zone: {selectedZone?.Guid}");
            GUILayout.Label($"Selected zone tick: {selectedZone?.Tick}");
            GUILayout.Label($"Active zone count: {zoneManager.zones.Count}");
            GUILayout.Label($"(Use the 1-9 keys to change selected zones)");
        }

        public void StartServer()
        {
            log.Information("Starting server");

            RegisterEvents();

            server.Create();
        }

        public void StopServer()
        {
            log.Information("Stopping server");

            server.Close();

            UnregisterEvents();
        }

        private void RegisterEvents()
        {
            server.ClientConnectedEvent += OnPlayerConnected;
            server.ClientDisconnectedEvent += OnPlayerDisconnected;

            server.RegisterPacketReceiver<PlayerInputPacket>(OnPlayerInput);

            zoneManager.RegisterPackets(server);
        }

        private void UnregisterEvents()
        {
            zoneManager.UnregisterPackets(server);

            server.ClearPacketReceiver<PlayerInputPacket>();

            server.ClientDisconnectedEvent -= OnPlayerDisconnected;
            server.ClientConnectedEvent -= OnPlayerConnected;
        }

        private void OnPlayerConnected(UnityServer sender, PeerConnectedEventArgs e)
        {
            log.Information("Player {Id} connected", e.Peer.Id);

            var player = playerManager.CreateFor(e.Peer);
            server.SendPacket(e.Peer, new PlayerIdAssignmentPacket() { id = e.Peer.Id }, DeliveryMethod.ReliableOrdered);

            zoneManager.GetOpenZone().ContinueWith((zone) => zoneManager.MovePlayerToZone(player, zone)).Forget();
        }

        private void OnPlayerDisconnected(UnityServer sender, PeerDisconnectedEventArgs e)
        {
            log.Information("Player {Id} disconnected", e.Peer.Id);

            if (playerManager.TryGetPlayer(e.Peer.Id, out ServerPlayer player))
            {
                player.CurrentZone?.RemovePlayer(player);

                if (player.Character)
                {
                    Destroy(player.Character.gameObject);
                }

                playerManager.RemoveFor(e.Peer);
            }
        }

        private void OnPlayerInput(NetPeer sender, PlayerInputPacket e)
        {
            if (playerManager.TryGetPlayer(sender.Id, out ServerPlayer player))
            {
                player.MovementBehaviour.input = e.data;
            }
        }

        private void SendPositionUpdates()
        {
            foreach (Zone zone in zoneManager.zones.Values)
            {
                foreach (ServerPlayer target in zone.Players)
                {
                    foreach (ServerPlayer current in zone.Players)
                    {
                        server.SendPacket(
                            target.Connection.Peer,
                            new PlayerUpdatePacket()
                            {
                                playerId = current.Id,
                                data = new PlayerUpdateData()
                                {
                                    playerPosition = current.Character.interpolation.current.playerPosition,
                                },
                            },
                            DeliveryMethod.Unreliable);
                    }
                }
            }
        }
    }
}
