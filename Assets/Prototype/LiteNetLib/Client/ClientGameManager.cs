using System.Collections;
using System.Collections.Generic;
using Exanite.Arpg.Logging;
using Exanite.Arpg.Networking.Client;
using LiteNetLib;
using Prototype.LiteNetLib.Shared;
using Prototype.LiteNetLib.Shared.Packets;
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

        private Dictionary<int, Player> players = new Dictionary<int, Player>();

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
            client.SubscribePacketReceiver<PlayerIDAssignmentPacket>(OnPlayerIDAssignment);
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

        private void OnPlayerIDAssignment(NetPeer sender, PlayerIDAssignmentPacket e)
        {
            id = e.id;
        }

        private void OnPlayerCreate(NetPeer sender, PlayerCreatePacket e)
        {
            foreach (var playerPosition in e.playerPositions)
            {
                if (!players.ContainsKey(playerPosition.id))
                {
                    var player = new Player(playerPosition.id, scene);
                    player.transform.position = playerPosition.position;

                    players.Add(playerPosition.id, player);

                    if (playerPosition.id == id)
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
            Destroy(players[e.id].transform.gameObject);

            players.Remove(e.id);
        }

        private void OnPlayerPositionUpdate(NetPeer sender, PlayerPositionUpdatePacket e)
        {
            foreach (var playerPosition in e.playerPositions)
            {
                if (players.ContainsKey(playerPosition.id))
                {
                    players[playerPosition.id].transform.position = playerPosition.position;
                }
            }
        }
    }
}
