using System;
using System.Collections;
using System.Collections.Generic;
using DarkRift.Client;
using Exanite.Arpg.Logging;
using Exanite.Arpg.Networking;
using Exanite.Arpg.Networking.Client;
using Exanite.Arpg.Networking.Shared;
using Prototype.DarkRift.Shared;
using UniRx.Async;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Prototype.DarkRift.Client
{
    public class ClientGameManager : MonoBehaviour
    {
        public UnityClient client;

        public bool isDisconnecting = false;

        private Dictionary<ushort, Player> players = new Dictionary<ushort, Player>();

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

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(1);

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
            foreach (var player in players.Values)
            {
                Gizmos.color = Color.red * 0.1f;

                Gizmos.DrawSphere(player.transform.position, 0.5f);
            }
        }

        public void Connect()
        {
            var request = new LoginRequest()
            {
                PlayerName = $"Player {Guid.NewGuid()}",
                GameVersion = Application.version,
            };

            client.ConnectAsync(request).Forget();
            client.OnMessageReceived += OnMessageRecieved;
        }

        public void Disconnect()
        {
            client.Disconnect();
        }

        private void OnMessageRecieved(object sender, MessageReceivedEventArgs e)
        {
            switch (e.Tag)
            {
                case MessageTag.PlayerCreate: OnPlayerCreate(sender, e); return;
                case MessageTag.PlayerDestroy: OnPlayerDestroy(sender, e); return;
                case MessageTag.PlayerPositionUpdate: OnPlayerPositionUpdate(sender, e); return;
            }
        }

        private void OnPlayerCreate(object sender, MessageReceivedEventArgs e)
        {
            using (var message = e.GetMessage())
            using (var reader = message.GetReader())
            {
                while (reader.Position < reader.Length)
                {
                    ushort id = reader.ReadUInt16();
                    Vector2 position = new Vector2(reader.ReadSingle(), reader.ReadSingle());

                    if (!players.ContainsKey(id))
                    {
                        var player = new Player(id, scene);
                        player.transform.position = position;

                        players.Add(id, player);

                        if (id == client.ID)
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
        }

        private void OnPlayerDestroy(object sender, MessageReceivedEventArgs e)
        {
            using (var message = e.GetMessage())
            using (var reader = message.GetReader())
            {
                ushort id = reader.ReadUInt16();

                Destroy(players[id].transform.gameObject);

                players.Remove(id);
            }
        }

        private void OnPlayerPositionUpdate(object sender, MessageReceivedEventArgs e)
        {
            using (var message = e.GetMessage())
            using (var reader = message.GetReader())
            {
                while (reader.Position < reader.Length)
                {
                    ushort id = reader.ReadUInt16();
                    Vector2 position = reader.ReadVector2();

                    players[id].transform.position = position;
                }
            }
        }
    }
}
