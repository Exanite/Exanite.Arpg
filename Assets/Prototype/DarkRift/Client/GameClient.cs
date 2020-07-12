using System;
using System.Collections;
using System.Collections.Generic;
using DarkRift.Client;
using Exanite.Arpg.DarkRift.Client;
using Exanite.Arpg.Logging;
using Prototype.DarkRift.Shared;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Prototype.DarkRift.Client
{
    public class GameClient : MonoBehaviour
    {
        public UnityClient client;

        public Dictionary<ushort, Player> players = new Dictionary<ushort, Player>();

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
            log.Information("Recieved message with ID {MessageID}", e.Tag);

            switch (e.Tag)
            {
                case MessageTag.PlayerCreate: OnPlayerCreate(sender, e); return;
                case MessageTag.PlayerDestroy: OnPlayerDestroy(sender, e); return;
                case MessageTag.PlayerPositionUpdate: OnPlayerPositionUpdate(sender, e); return;
            }
        }

        private void OnPlayerCreate(object sender, MessageReceivedEventArgs e)
        {
            if (e.Tag == MessageTag.PlayerCreate)
            {
                using (var message = e.GetMessage())
                {
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
                                    player.transform.gameObject.name += " (Local)";
                                    player.transform.gameObject.AddComponent<PlayerController>().client = client.Client;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void OnPlayerDestroy(object sender, MessageReceivedEventArgs e)
        {

        }

        private void OnPlayerPositionUpdate(object sender, MessageReceivedEventArgs e)
        {

        }
    }
}
