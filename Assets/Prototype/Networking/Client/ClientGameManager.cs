using Exanite.Arpg.Logging;
using Exanite.Arpg.Networking.Client;
using LiteNetLib;
using Prototype.Networking.Players;
using Prototype.Networking.Players.Packets;
using UniRx.Async;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Prototype.Networking.Client
{
    public class ClientGameManager : MonoBehaviour
    {
        public UnityClient client;

        public int id = -1;

        private Player localPlayer;

        private ILog log;
        private Scene scene;

        [Inject]
        public void Inject(ILog log, Scene scene)
        {
            this.log = log;
            this.scene = scene;
        }

        //private void Start()
        //{
        //    Connect();
        //}

        public void Connect()
        {
            client.RegisterPacketReceiver<PlayerIdAssignmentPacket>(OnPlayerIdAssignment);
            //client.RegisterPacketReceiver<PlayerConnectedPacket>(OnPlayerConnected);
            //client.RegisterPacketReceiver<PlayerDisconnectedPacket>(OnPlayerDisconnected);
            //client.RegisterPacketReceiver<PlayerPositionUpdatePacket>(OnPlayerPositionUpdate);

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

        //private void OnPlayerConnected(NetPeer sender, PlayerConnectedPacket e)
        //{
        //    foreach (var newPlayer in e.newPlayers)
        //    {
        //        if (!playerManager.Contains(newPlayer.id))
        //        {
        //            var connection = new PlayerConnection() { Id = newPlayer.id };
        //            var player = new Player(connection);

        //            player.character.transform.position = newPlayer.position;

        //            playerManager.AddPlayer(player);

        //            if (newPlayer.id == id)
        //            {
        //                localPlayer = player;
        //                localPlayer.character.transform.gameObject.name += " (Local)";

        //                playerController = localPlayer.character.transform.gameObject.AddComponent<PlayerController>();
        //                playerController.client = client;
        //                playerController.player = localPlayer;
        //            }
        //        }
        //    }
        //}

        //private void OnPlayerDisconnected(NetPeer sender, PlayerDisconnectedPacket e)
        //{
        //    var player = playerManager.GetPlayer(e.id);

        //    Destroy(player.character.transform.gameObject);
        //    playerManager.RemovePlayer(player);
        //}

        //private void OnPlayerPositionUpdate(NetPeer sender, PlayerPositionUpdatePacket e)
        //{
        //    foreach (var playerPosition in e.playerPositions)
        //    {
        //        if (playerManager.TryGetPlayer(playerPosition.id, out Player player))
        //        {
        //            player.character.transform.position = playerPosition.position;
        //        }
        //    }
        //}
    }
}
