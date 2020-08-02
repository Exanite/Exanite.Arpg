using System.Collections;
using Exanite.Arpg.Logging;
using Exanite.Arpg.Networking.Client;
using LiteNetLib;
using Prototype.LiteNetLib.Players;
using Prototype.LiteNetLib.Players.Packets;
using UniRx.Async;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Prototype.LiteNetLib.Client
{
    public class ClientGameManager : MonoBehaviour
    {
        public UnityClient client;

        public int id = -1;

        public bool isDisconnecting = false;

        private PlayerManager playerManager = new PlayerManager();

        private Player localPlayer;
        private PlayerController playerController;

        private ILog log;
        private Scene scene;

        [Inject]
        public void Inject(ILog log, Scene scene)
        {
            this.log = log;
            this.scene = scene;
        }

        private void Start()
        {
            Connect();
        }

        private void Update()
        {
            if (isDisconnecting)
            {
                Disconnect();

                isDisconnecting = false;
            }
        }

        private void OnDrawGizmos()
        {
            foreach (var player in playerManager.Players)
            {
                Gizmos.color = Color.red * 0.1f;

                Gizmos.DrawSphere(player.transform.position, 0.5f);
            }
        }

        public void Connect()
        {
            client.SubscribePacketReceiver<PlayerIdAssignmentPacket>(OnPlayerIdAssignment);
            client.SubscribePacketReceiver<PlayerCreatePacket>(OnPlayerCreate);
            client.SubscribePacketReceiver<PlayerDestroyPacket>(OnPlayerDestroy);
            client.SubscribePacketReceiver<PlayerPositionUpdatePacket>(OnPlayerPositionUpdate);

            client.DisconnectedEvent += OnDisconnected;

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
        }

        private void OnDisconnected(UnityClient sender, DisconnectedEventArgs e)
        {
            SceneManager.UnloadSceneAsync(scene);
        }

        private void OnPlayerIdAssignment(NetPeer sender, PlayerIdAssignmentPacket e)
        {
            id = e.id;
        }

        private void OnPlayerCreate(NetPeer sender, PlayerCreatePacket e)
        {
            foreach (var newPlayer in e.newPlayers)
            {
                if (!playerManager.Contains(newPlayer.id))
                {
                    var connection = new PlayerConnection() { Id = newPlayer.id };
                    var player = new Player(connection, scene);

                    player.transform.position = newPlayer.position;

                    playerManager.AddPlayer(player);

                    if (newPlayer.id == id)
                    {
                        localPlayer = player;
                        localPlayer.transform.gameObject.name += " (Local)";

                        playerController = localPlayer.transform.gameObject.AddComponent<PlayerController>();
                        playerController.client = client;
                        playerController.player = localPlayer;
                    }
                }
            }

        }

        private void OnPlayerDestroy(NetPeer sender, PlayerDestroyPacket e)
        {
            var player = playerManager.GetPlayer(e.id);

            Destroy(player.transform.gameObject);
            playerManager.RemovePlayer(player);
        }

        private void OnPlayerPositionUpdate(NetPeer sender, PlayerPositionUpdatePacket e)
        {
            foreach (var playerPosition in e.playerPositions)
            {
                if (playerManager.Contains(playerPosition.id))
                {
                    var player = playerManager.GetPlayer(playerPosition.id);
                    player.transform.position = playerPosition.position;
                }
            }
        }
    }
}
