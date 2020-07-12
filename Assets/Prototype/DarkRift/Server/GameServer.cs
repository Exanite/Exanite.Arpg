using System;
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

        [Inject]
        public void Inject(ILog log, Scene scene)
        {
            this.log = log;
            this.scene = scene;
        }

        private void Start()
        {
            StartServer();
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

                using (var createPlayerMessage = Message.Create(MessageTag.PlayerCreate, writer))
                {
                    foreach (var client in players.Keys)
                    {
                        client.SendMessage(createPlayerMessage, SendMode.Reliable);
                    }
                }
            }
        }

        private void CreateNewPlayer(IClient client)
        {
            var player = new Player(client.ID, scene);
            player.transform.position = new Vector2(Random.value, Random.value);

            players.Add(client, player);
        }

        private void OnClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            log.Information("Player {ClientID} disconnected", e.Client.ID);

            e.Client.MessageReceived -= OnMessageReceived;

            Destroy(players[e.Client].transform.gameObject);

            players.Remove(e.Client);
        }

        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            log.Information("Message recieved from Player {ClientID} with ID {MessageID}", e.Client.ID, e.Tag);

            switch (e.Tag)
            {
                case MessageTag.PlayerInput: OnPlayerInput(sender, e); return;
            }
        }

        private void OnPlayerInput(object sender, MessageReceivedEventArgs e)
        {

        }
    }
}
