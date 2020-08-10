using System;
using Exanite.Arpg.Logging;
using Exanite.Arpg.Networking.Server;
using LiteNetLib;
using Prototype.Networking.Players;
using Prototype.Networking.Players.Packets;
using Prototype.Networking.Zones;
using Prototype.Networking.Zones.Packets;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Prototype.Networking.Server
{
    public class ServerGameManager : MonoBehaviour
    {
        public UnityServer server;

        public GameObject tempZonePrefab;
        public Guid tempMainZoneGuid;

        private ILog log;
        private Scene scene;
        private ServerPlayerManager playerManager;
        private ServerZoneManager zoneManager;

        [Inject]
        public void Inject(ILog log, Scene scene, ServerPlayerManager playerManager, ServerZoneManager zoneManager)
        {
            this.log = log;
            this.scene = scene;
            this.playerManager = playerManager;
            this.zoneManager = zoneManager;
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

            server.ClientConnectedEvent += OnPlayerConnected;
            server.ClientDisconnectedEvent += OnPlayerDisconnected;

            zoneManager.RegisterPackets(server);

            server.Create();

            // ! Move zone creation somewhere else
            var zone = new Zone(tempZonePrefab);
            tempMainZoneGuid = zone.guid;
            zoneManager.zones.Add(zone.guid, zone);
        }

        public void StopServer()
        {
            log.Information("Stopping server");

            server.Close();

            zoneManager.UnregisterPackets(server);

            server.ClientDisconnectedEvent -= OnPlayerDisconnected;
            server.ClientConnectedEvent-= OnPlayerConnected;
        }

        private void OnPlayerConnected(UnityServer sender, ClientConnectedEventArgs e)
        {
            log.Information("Player {Id} connected", e.Peer.Id);

            playerManager.CreateFor(e.Peer);

            server.SendPacket(e.Peer, new PlayerIdAssignmentPacket() { id = e.Peer.Id }, DeliveryMethod.ReliableOrdered);
            server.SendPacket(e.Peer, new ZoneCreatePacket() { guid = tempMainZoneGuid }, DeliveryMethod.ReliableOrdered);
        }

        private void OnPlayerDisconnected(UnityServer sender, ClientDisconnectedEventArgs e)
        {
            log.Information("Player {Id} disconnected", e.Peer.Id);

            playerManager.RemoveFor(e.Peer);
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
