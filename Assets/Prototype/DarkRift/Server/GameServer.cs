using System.Collections.Generic;
using DarkRift;
using DarkRift.Server;
using Exanite.Arpg.DarkRift.Server;
using Exanite.Arpg.Logging;
using Prototype.DarkRift.Shared;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Random = UnityEngine.Random;

namespace Prototype.DarkRift.Server
{
    public class GameServer : MonoBehaviour
    {
        public XmlUnityServer server;

        public Dictionary<IClient, Player> players = new Dictionary<IClient, Player>();

        private ILog log;
        private Scene scene;
        private PhysicsScene physics;

        [Inject]
        public void Inject(ILog log, Scene scene, PhysicsScene physics)
        {
            this.log = log;
            this.scene = scene;
        }

        private void Start()
        {
            StartServer();
        }

        private void Update() // replace with server tick loop
        {
            foreach (var player in players.Values)
            {
                player.transform.position += (Vector3)(player.movementInput * Time.deltaTime * 10);

                float verticalExtents = Camera.main.orthographicSize;
                float horizontalExtents = Camera.main.orthographicSize * Screen.width / Screen.height;

                if (player.transform.position.x > horizontalExtents)
                {
                    Vector2 newPosition = player.transform.position;
                    newPosition.x -= horizontalExtents * 2;
                    player.transform.position = newPosition;
                }
                else if (player.transform.position.x < -horizontalExtents)
                {
                    Vector2 newPosition = player.transform.position;
                    newPosition.x += horizontalExtents * 2;
                    player.transform.position = newPosition;
                }
                else if (player.transform.position.y > verticalExtents)
                {
                    Vector2 newPosition = player.transform.position;
                    newPosition.y -= verticalExtents * 2;
                    player.transform.position = newPosition;
                }
                else if (player.transform.position.y < -verticalExtents)
                {
                    Vector2 newPosition = player.transform.position;
                    newPosition.y += verticalExtents * 2;
                    player.transform.position = newPosition;
                }

                SendPositionUpdates();
            }
        }

        private void OnDrawGizmos()
        {
            foreach (var player in players.Values)
            {
                Gizmos.color = Color.red;

                Gizmos.DrawSphere(player.transform.position, 0.5f);
            }
        }

        public void StartServer()
        {
            log.Information("Starting server");

            server.Create();

            server.Server.ClientManager.ClientConnected += OnClientConnected;
            server.Server.ClientManager.ClientDisconnected += OnClientDisconnected;
        }

        public void StopServer()
        {
            log.Information("Stopping server");

            server.Close();
        }

        private void CreateNewPlayer(IClient client)
        {
            var player = new Player(client.ID, scene);

            float angle = Random.Range(0, 360);

            player.transform.position = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 5;

            players.Add(client, player);
        }

        private void OnClientConnected(object sender, ClientConnectedEventArgs e)
        {
            log.Information("Player {ClientID} connected", e.Client.ID);

            e.Client.MessageReceived += OnMessageReceived;

            CreateNewPlayer(e.Client);

            using (var writer = DarkRiftWriter.Create())
            {
                foreach (var player in players.Values)
                {
                    writer.Write(player.id);

                    writer.Write(player.transform.position.x);
                    writer.Write(player.transform.position.y);
                }

                using (var playerCreateMessage = Message.Create(MessageTag.PlayerCreate, writer))
                {
                    foreach (var client in players.Keys)
                    {
                        client.SendMessage(playerCreateMessage, SendMode.Reliable);
                    }
                }
            }
        }

        private void OnClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            log.Information("Player {ClientID} disconnected", e.Client.ID);

            e.Client.MessageReceived -= OnMessageReceived;

            Destroy(players[e.Client].transform.gameObject);

            players.Remove(e.Client);

            using (var writer = DarkRiftWriter.Create())
            {
                writer.Write(e.Client.ID);

                using (var playerDestroyMessage = Message.Create(MessageTag.PlayerDestroy, writer))
                {
                    foreach (var client in players.Keys)
                    {
                        client.SendMessage(playerDestroyMessage, SendMode.Reliable);
                    }
                }
            }
        }

        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            switch (e.Tag)
            {
                case MessageTag.PlayerInput: OnPlayerInput(sender, e); return;
            }
        }

        private void OnPlayerInput(object sender, MessageReceivedEventArgs e)
        {
            using (var message = e.GetMessage())
            using (var reader = message.GetReader())
            {
                Vector2 movementInput = new Vector2(reader.ReadSingle(), reader.ReadSingle());

                players[e.Client].movementInput = movementInput.normalized;
            }
        }

        private void SendPositionUpdates()
        {
            using (var writer = DarkRiftWriter.Create())
            {
                foreach (var player in players.Values)
                {
                    writer.Write(player.id);

                    writer.Write(player.transform.position.x);
                    writer.Write(player.transform.position.y);
                }

                using (var message = Message.Create(MessageTag.PlayerPositionUpdate, writer))
                {
                    foreach (var client in players.Keys)
                    {
                        client.SendMessage(message, SendMode.Unreliable);
                    }
                }
            }
        }
    }
}
