using Exanite.Arpg.Logging;
using Exanite.Arpg.Networking.Server;
using LiteNetLib;
using Prototype.Networking.Players;
using Prototype.Networking.Players.Packets;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Prototype.Networking.Server
{
    public class ServerGameManager : MonoBehaviour
    {
        public UnityServer server;

        private ILog log;
        private Scene scene;
        public ServerPlayerManager playerManager;

        [Inject]
        public void Inject(ILog log, Scene scene, ServerPlayerManager playerManager)
        {
            this.log = log;
            this.scene = scene;
            this.playerManager = playerManager;
        }

        private void Start()
        {
            StartServer();
        }

        //private void FixedUpdate()
        //{
        //    foreach (var player in playerManager.Players)
        //    {
        //        var playerTransform = player.character.transform;

        //        playerTransform.position += (Vector3)(player.movementInput * Time.deltaTime * 5);

        //        float verticalExtents = Camera.main.orthographicSize;
        //        float horizontalExtents = Camera.main.orthographicSize * Screen.width / Screen.height;

        //        if (playerTransform.position.x > horizontalExtents)
        //        {
        //            Vector2 newPosition = playerTransform.position;
        //            newPosition.x -= horizontalExtents * 2;
        //            playerTransform.position = newPosition;
        //        }
        //        else if (playerTransform.position.x < -horizontalExtents)
        //        {
        //            Vector2 newPosition = playerTransform.position;
        //            newPosition.x += horizontalExtents * 2;
        //            playerTransform.position = newPosition;
        //        }
        //        else if (playerTransform.position.y > verticalExtents)
        //        {
        //            Vector2 newPosition = playerTransform.position;
        //            newPosition.y -= verticalExtents * 2;
        //            playerTransform.position = newPosition;
        //        }
        //        else if (playerTransform.position.y < -verticalExtents)
        //        {
        //            Vector2 newPosition = playerTransform.position;
        //            newPosition.y += verticalExtents * 2;
        //            playerTransform.position = newPosition;
        //        }

        //        SendPositionUpdates();
        //    }
        //}

        public void StartServer()
        {
            log.Information("Starting server");

            //server.RegisterPacketReceiver<PlayerInputPacket>(OnPlayerInput);

            server.ClientConnectedEvent += OnPlayerConnected;
            server.ClientDisconnectedEvent += OnPlayerDisconnected;

            server.Create();
        }

        public void StopServer()
        {
            log.Information("Stopping server");

            server.Close();
        }

        //private void CreateNewPlayer(NetPeer peer)
        //{
        //    var connection = new PlayerConnection() { Id = peer.Id, Peer = peer };
        //    var player = new Player(connection);

        //    float angle = Random.Range(0, 360);
        //    player.character.transform.position = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 5;

        //    playerManager.AddPlayer(player);
        //}

        private void OnPlayerConnected(UnityServer sender, ClientConnectedEventArgs e)
        {
            log.Information("Player {Id} connected", e.Peer.Id);

            var newPlayer = new Player(new PlayerConnection(e.Peer));
            playerManager.AddPlayer(newPlayer);

            server.SendPacket(e.Peer, new PlayerIdAssignmentPacket() { id = e.Peer.Id }, DeliveryMethod.ReliableOrdered);
        }

        private void OnPlayerDisconnected(UnityServer sender, ClientDisconnectedEventArgs e)
        {
            log.Information("Player {Id} disconnected", e.Peer.Id);

            var disconnectedPlayer = playerManager.GetPlayer(e.Peer.Id);
            playerManager.RemovePlayer(disconnectedPlayer);
        }

        //private void OnPlayerInput(NetPeer sender, PlayerInputPacket e)
        //{
        //    if (playerManager.TryGetPlayer(sender.Id, out Player player))
        //    {
        //        player.movementInput = e.movementInput;
        //    }
        //}

        //private void SendPositionUpdates()
        //{
        //    var packet = new PlayerPositionUpdatePacket(playerManager.Players);

        //    foreach (var player in playerManager.Players)
        //    {
        //        server.SendPacket(player.Connection.Peer, packet, DeliveryMethod.Unreliable);
        //    }
        //}
    }
}
