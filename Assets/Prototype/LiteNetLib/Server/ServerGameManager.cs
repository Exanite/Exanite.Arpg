using System.Collections.Generic;
using Exanite.Arpg.Logging;
using Exanite.Arpg.NewNetworking.Server;
using LiteNetLib;
using Prototype.LiteNetLib.Shared;
using Prototype.LiteNetLib.Shared.Packets;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Random = UnityEngine.Random;

namespace Prototype.LiteNetLib.Server
{
    public class ServerGameManager : MonoBehaviour
    {
        public UnityServer server;

        public Dictionary<NetPeer, Player> players = new Dictionary<NetPeer, Player>();

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
                player.transform.position += (Vector3)(player.movementInput * Time.deltaTime * 5);

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
            var player = new Player(peer.Id, scene);

            float angle = Random.Range(0, 360);

            player.transform.position = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 5;

            players.Add(peer, player);
        }

        private void OnPlayerConnected(UnityServer sender, ClientConnectedEventArgs e)
        {
            log.Information("Player {ClientID} connected", e.Peer.Id);

            server.SendPacket(e.Peer, new PlayerIDAssignmentPacket() { id = e.Peer.Id }, DeliveryMethod.ReliableOrdered);

            CreateNewPlayer(e.Peer);

            foreach (var peer in players.Keys)
            {
                server.SendPacket(peer, new PlayerCreatePacket(players.Values), DeliveryMethod.ReliableOrdered);
            }
        }

        private void OnPlayerDisconnected(UnityServer sender, ClientDisconnectedEventArgs e)
        {
            log.Information("Player {ClientID} disconnected", e.Peer.Id);

            Destroy(players[e.Peer].transform.gameObject);

            players.Remove(e.Peer);

            foreach (var peer in players.Keys)
            {
                server.SendPacket(peer, new PlayerDestroyPacket() { id = e.Peer.Id }, DeliveryMethod.ReliableOrdered);
            }
        }

        private void OnPlayerInput(NetPeer sender, PlayerInputPacket e)
        {
            players[sender].movementInput = e.movementInput;
        }

        private void SendPositionUpdates()
        {
            foreach (var peer in players.Keys)
            {
                server.SendPacket(peer, new PlayerPositionUpdatePacket(players.Values), DeliveryMethod.Unreliable);
            }
        }
    }
}
