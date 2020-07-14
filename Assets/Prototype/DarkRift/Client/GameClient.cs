using System;
using System.Collections;
using System.Collections.Generic;
using DarkRift.Client;
using Exanite.Arpg.DarkRift.Client;
using Exanite.Arpg.Logging;
using Exanite.Arpg.Networking;
using Prototype.DarkRift.Shared;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Prototype.DarkRift.Client
{
    public class GameClient : MonoBehaviour
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
            client.ConnectInBackground(client.Address, client.Port, false, OnConnected);
            client.Disconnected += OnDisconnected;
            client.MessageReceived += OnMessageRecieved;
        }

        public void Disconnect()
        {
            client.Disconnect();
        }

        private void OnConnected(Exception e)
        {
            log.Information("Connected");
        }

        private void OnDisconnected(object sender, DisconnectedEventArgs e)
        {
            log.Information("Disconnected");
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
                            playerController.client = client.Client;
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
