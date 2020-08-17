﻿using System;
using System.Linq;
using Exanite.Arpg.Logging;
using Exanite.Arpg.Networking.Server;
using LiteNetLib;
using Prototype.Networking.Players;
using Prototype.Networking.Players.Packets;
using Prototype.Networking.Zones;
using UniRx.Async;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Random = UnityEngine.Random;

namespace Prototype.Networking.Server
{
    public class ServerGameManager : MonoBehaviour
    {
        public UnityServer server;

        private Zone selectedZone;

        private ILog log;
        private Scene scene;
        private ServerPlayerManager playerManager;
        private ServerZoneManager zoneManager;

        [Inject]
        public void Inject(ILog log, Scene scene, ServerPlayerManager playerManager, ServerZoneManager zoneManager)
        {
            this.log = log;
            this.scene = scene;
            this.playerManager = playerManager;
            this.zoneManager = zoneManager;
        }

        private async UniTaskVoid Start()
        {
            StartServer();

            while (true) // for testing moving players between zones
            {
                MoveRandomPlayerToRandomZone();

                await UniTask.Delay(TimeSpan.FromSeconds(1));
            }
        }

        private void MoveRandomPlayerToRandomZone() // for testing moving players between zones
        {
            if (playerManager.PlayerCount > 0 && zoneManager.zones.Count > 0)
            {
                var player = playerManager.Players.OrderBy(x => Random.value).First();
                var zone = zoneManager.zones.OrderBy(x => Random.value).First().Value;

                zoneManager.MovePlayerToZone(player, zone);
            }
        }

        private void Update() // for debug
        {
            if (selectedZone == null
                && zoneManager.publicZones != null
                && zoneManager.publicZones.Count > 0)
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
            foreach (var player in playerManager.Players)
            {
                if (!player.Character)
                {
                    continue;
                }

                var playerTransform = player.Character.transform;

                playerTransform.position += (Vector3)(player.Character.movementInput * Time.deltaTime * 5);

                float verticalExtents = Camera.main.orthographicSize;
                float horizontalExtents = Camera.main.orthographicSize * Screen.width / Screen.height;

                if (playerTransform.position.x > horizontalExtents)
                {
                    Vector2 newPosition = playerTransform.position;
                    newPosition.x -= horizontalExtents * 2;
                    playerTransform.position = newPosition;
                }
                else if (playerTransform.position.x < -horizontalExtents)
                {
                    Vector2 newPosition = playerTransform.position;
                    newPosition.x += horizontalExtents * 2;
                    playerTransform.position = newPosition;
                }
                else if (playerTransform.position.y > verticalExtents)
                {
                    Vector2 newPosition = playerTransform.position;
                    newPosition.y -= verticalExtents * 2;
                    playerTransform.position = newPosition;
                }
                else if (playerTransform.position.y < -verticalExtents)
                {
                    Vector2 newPosition = playerTransform.position;
                    newPosition.y += verticalExtents * 2;
                    playerTransform.position = newPosition;
                }

                SendPositionUpdates();
            }
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying || selectedZone == null)
            {
                return;
            }

            Gizmos.color = Color.red;
            foreach (var player in selectedZone.playersById.Values)
            {
                Gizmos.DrawSphere(player.Character.transform.position, 0.5f);
            }
        }

        private void OnGUI()
        {
            GUILayout.Label($"Selected zone: {selectedZone?.guid}");
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

            var zone = zoneManager.GetOpenZone();
            zoneManager.MovePlayerToZone(player, zone);
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
                player.Character.movementInput = e.movementInput;
            }
        }

        private void SendPositionUpdates()
        {
            foreach (Zone zone in zoneManager.zones.Values)
            {
                foreach (ServerPlayer target in zone.playersById.Values)
                {
                    foreach (ServerPlayer current in zone.playersById.Values)
                    {
                        server.SendPacket(
                            target.Connection.Peer,
                            new PlayerPositionUpdatePacket()
                            {
                                playerId = current.Id,
                                playerPosition = current.Character.transform.position,
                            },
                            DeliveryMethod.Unreliable);
                    }
                }
            }
        }
    }
}
