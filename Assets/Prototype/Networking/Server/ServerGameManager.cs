using Exanite.Arpg.Logging;
using Exanite.Arpg.Networking.Server;
using LiteNetLib;
using Prototype.Networking.Players;
using Prototype.Networking.Players.Packets;
using Prototype.Networking.Zones;
using Prototype.Networking.Zones.Packets;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Prototype.Networking.Server
{
    public class ServerGameManager : MonoBehaviour
    {
        public UnityServer server;

        public Zone tempMainZone;

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

        private void Start()
        {
            StartServer();
        }

        private void FixedUpdate()
        {
            foreach (var player in playerManager.Players)
            {
                if (!player.character)
                {
                    continue;
                }

                var playerTransform = player.character.transform;

                playerTransform.position += (Vector3)(player.character.movementInput * Time.deltaTime * 5);

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
            Gizmos.color = Color.red;

            foreach (var player in tempMainZone.playersById.Values)
            {
                Gizmos.DrawSphere(player.character.transform.position, 0.5f);
            }
        }

        public void StartServer()
        {
            log.Information("Starting server");

            server.ClientConnectedEvent += OnPlayerConnected;
            server.ClientDisconnectedEvent += OnPlayerDisconnected;

            zoneManager.RegisterPackets(server);

            server.RegisterPacketReceiver<PlayerInputPacket>(OnPlayerInput);

            server.Create();

            // ! Move zone creation somewhere else
            tempMainZone = new Zone();
            zoneManager.zones.Add(tempMainZone.guid, tempMainZone);
        }

        public void StopServer()
        {
            log.Information("Stopping server");

            server.Close();

            server.ClearPacketReceiver<PlayerInputPacket>();

            zoneManager.UnregisterPackets(server);

            server.ClientDisconnectedEvent -= OnPlayerDisconnected;
            server.ClientConnectedEvent -= OnPlayerConnected;
        }

        private void OnPlayerConnected(UnityServer sender, PeerConnectedEventArgs e)
        {
            log.Information("Player {Id} connected", e.Peer.Id);

            playerManager.CreateFor(e.Peer);

            server.SendPacket(e.Peer, new PlayerIdAssignmentPacket() { id = e.Peer.Id }, DeliveryMethod.ReliableOrdered);
            server.SendPacket(e.Peer, new ZoneCreatePacket() { guid = tempMainZone.guid }, DeliveryMethod.ReliableOrdered);
        }

        private void OnPlayerDisconnected(UnityServer sender, PeerDisconnectedEventArgs e)
        {
            log.Information("Player {Id} disconnected", e.Peer.Id);

            if (tempMainZone.playersById.TryGetValue(e.Peer.Id, out Player player))
            {
                foreach (ServerPlayer playerInZone in tempMainZone.playersById.Values)
                {
                    if (playerInZone != player)
                    {
                        server.SendPacket(playerInZone.Connection.Peer, new ZonePlayerLeavePacket() { playerId = player.Id }, DeliveryMethod.ReliableOrdered);
                    }
                }

                if (player.character)
                {
                    Destroy(player.character.gameObject);
                }

                tempMainZone.RemovePlayer(player);
                playerManager.RemoveFor(e.Peer);
            }

            playerManager.RemoveFor(e.Peer);
        }

        private void OnPlayerInput(NetPeer sender, PlayerInputPacket e)
        {
            if (playerManager.TryGetPlayer(sender.Id, out ServerPlayer player))
            {
                player.character.movementInput = e.movementInput;
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
                                playerPosition = current.character.transform.position,
                            },
                            DeliveryMethod.Unreliable);
                    }
                }
            }
        }
    }
}
