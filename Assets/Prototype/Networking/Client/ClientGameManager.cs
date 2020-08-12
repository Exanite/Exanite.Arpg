using Exanite.Arpg.Logging;
using Exanite.Arpg.Networking.Client;
using LiteNetLib;
using Prototype.Networking.Players;
using Prototype.Networking.Players.Packets;
using Prototype.Networking.Zones;
using Prototype.Networking.Zones.Packets;
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
        private Zone currentZone;

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
            Connect();
        }

        public void Connect()
        {
            client.RegisterPacketReceiver<PlayerIdAssignmentPacket>(OnPlayerIdAssignment);

            client.RegisterPacketReceiver<ZoneCreatePacket>(OnZoneCreate);
            client.RegisterPacketReceiver<ZonePlayerEnterPacket>(OnZonePlayerEnter);
            client.RegisterPacketReceiver<ZonePlayerLeavePacket>(OnZonePlayerLeave);

            client.RegisterPacketReceiver<PlayerPositionUpdatePacket>(OnPlayerPositionUpdate);

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

        private void OnZoneCreate(NetPeer sender, ZoneCreatePacket e)
        {
            var newZone = new Zone(e.guid);

            currentZone = newZone;

            client.SendPacketToServer(new ZoneCreateFinishedPacket() { guid = e.guid }, DeliveryMethod.ReliableOrdered);
        }

        private void OnZonePlayerEnter(NetPeer sender, ZonePlayerEnterPacket e)
        {
            if (!currentZone.playersById.ContainsKey(e.playerId))
            {
                var player = new Player(e.playerId);
                currentZone.AddPlayer(player);

                player.CreatePlayerCharacter(currentZone);
                player.character.transform.position = e.playerPosition;

                if (e.playerId == id)
                {
                    var controller = player.character.gameObject.AddComponent<PlayerController>();
                    controller.player = player;
                    controller.client = client;

                    player.character.name += " (Local)";
                }
            }
        }

        private void OnZonePlayerLeave(NetPeer sender, ZonePlayerLeavePacket e)
        {
            if (currentZone.playersById.TryGetValue(e.playerId, out Player player))
            {
                Destroy(player.character.gameObject);
                currentZone.RemovePlayer(player);
            }
        }

        private void OnPlayerPositionUpdate(NetPeer sender, PlayerPositionUpdatePacket e)
        {
            if (currentZone.playersById.TryGetValue(e.playerId, out Player player))
            {
                if (player.character)
                {
                    player.character.transform.position = e.playerPosition;
                }
            }
        }

        public void Disconnect()
        {
            client.Disconnect();
        }

        private void OnDisconnected(UnityClient sender, DisconnectedEventArgs e)
        {
            SceneManager.UnloadSceneAsync(scene);
            SceneManager.UnloadSceneAsync(currentZone.scene);
        }

        private void OnPlayerIdAssignment(NetPeer sender, PlayerIdAssignmentPacket e)
        {
            id = e.id;
        }
    }
}
