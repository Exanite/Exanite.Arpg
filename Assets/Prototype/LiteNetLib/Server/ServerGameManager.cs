using Exanite.Arpg.Logging;
using Exanite.Arpg.Networking.Server;
using LiteNetLib;
using Prototype.LiteNetLib.Players;
using Prototype.LiteNetLib.Players.Packets;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Random = UnityEngine.Random;

namespace Prototype.LiteNetLib.Server
{
    public class ServerGameManager : MonoBehaviour
    {
        public UnityServer server;

        private ILog log;
        private Scene scene;
        public PlayerManager playerManager;

        [Inject]
        public void Inject(ILog log, Scene scene, PlayerManager playerManager)
        {
            this.log = log;
            this.scene = scene;
            this.playerManager = playerManager;
        }

        private void Start()
        {
            StartServer();
        }

        private void FixedUpdate()
        {
            foreach (var player in playerManager.Players)
            {
                var playerTransform = player.character.transform;

                playerTransform.position += (Vector3)(player.movementInput * Time.deltaTime * 5);

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
            foreach (var player in playerManager.Players)
            {
                Gizmos.color = Color.red;

                Gizmos.DrawSphere(player.character.transform.position, 0.5f);
            }
        }

        public void StartServer()
        {
            log.Information("Starting server");

            server.SubscribePacketReceiver<PlayerInputPacket>(OnPlayerInput);

            server.ClientConnectedEvent += OnPlayerConnected;
            server.ClientDisconnectedEvent += OnPlayerDisconnected;

            server.Create();
        }

        public void StopServer()
        {
            log.Information("Stopping server");

            server.Close();
        }

        private void CreateNewPlayer(NetPeer peer)
        {
            var connection = new PlayerConnection() { Id = peer.Id, Peer = peer };
            var player = new Player(connection, scene);

            float angle = Random.Range(0, 360);
            player.character.transform.position = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 5;

            playerManager.AddPlayer(player);
        }

        private void OnPlayerConnected(UnityServer sender, ClientConnectedEventArgs e)
        {
            log.Information("Player {Id} connected", e.Peer.Id);

            server.SendPacket(e.Peer, new PlayerIdAssignmentPacket() { id = e.Peer.Id }, DeliveryMethod.ReliableOrdered);

            CreateNewPlayer(e.Peer);

            var packet = new PlayerCreatePacket(playerManager.Players);

            foreach (var player in playerManager.Players)
            {
                server.SendPacket(player.Connection.Peer, packet, DeliveryMethod.ReliableOrdered);
            }
        }

        private void OnPlayerDisconnected(UnityServer sender, ClientDisconnectedEventArgs e)
        {
            log.Information("Player {Id} disconnected", e.Peer.Id);

            var disconnectedPlayer = playerManager.GetPlayer(e.Peer.Id);

            Destroy(disconnectedPlayer.character.transform.gameObject);
            playerManager.RemovePlayer(disconnectedPlayer);

            var packet = new PlayerDestroyPacket() { id = e.Peer.Id };

            foreach (var player in playerManager.Players)
            {
                server.SendPacket(player.Connection.Peer, packet, DeliveryMethod.ReliableOrdered);
            }
        }

        private void OnPlayerInput(NetPeer sender, PlayerInputPacket e)
        {
            var player = playerManager.GetPlayer(sender.Id);
            player.movementInput = e.movementInput;
        }

        private void SendPositionUpdates()
        {
            var packet = new PlayerPositionUpdatePacket(playerManager.Players);

            foreach (var player in playerManager.Players)
            {
                server.SendPacket(player.Connection.Peer, packet, DeliveryMethod.Unreliable);
            }
        }
    }
}
