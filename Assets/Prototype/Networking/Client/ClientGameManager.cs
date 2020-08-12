using Exanite.Arpg.Logging;
using Exanite.Arpg.Networking.Client;
using LiteNetLib;
using Prototype.Networking.Players;
using Prototype.Networking.Players.Packets;
using Prototype.Networking.Zones;
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

        private ILog log;
        private Scene scene;
        private ClientZoneManager zoneManager;

        [Inject]
        public void Inject(ILog log, Scene scene, ClientZoneManager zoneManager)
        {
            this.log = log;
            this.scene = scene;
            this.zoneManager = zoneManager;
        }

        private void Start()
        {
            Connect();
        }

        public void Connect()
        {
            client.RegisterPacketReceiver<PlayerIdAssignmentPacket>(OnPlayerIdAssignment);
            client.RegisterPacketReceiver<PlayerPositionUpdatePacket>(OnPlayerPositionUpdate);

            zoneManager.RegisterPackets(client);

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

            zoneManager.UnregisterPackets(client);

            client.ClearPacketReceiver<PlayerPositionUpdatePacket>();
            client.ClearPacketReceiver<PlayerIdAssignmentPacket>();

            client.DisconnectedEvent -= OnDisconnected;
        }

        private void OnDisconnected(UnityClient sender, DisconnectedEventArgs e)
        {
            SceneManager.UnloadSceneAsync(scene);
            SceneManager.UnloadSceneAsync(zoneManager.currentZone.scene);
        }

        private void OnPlayerIdAssignment(NetPeer sender, PlayerIdAssignmentPacket e)
        {
            id = e.id;
        }

        private void OnPlayerPositionUpdate(NetPeer sender, PlayerPositionUpdatePacket e)
        {
            if (zoneManager.currentZone.playersById.TryGetValue(e.playerId, out Player player))
            {
                if (player.character)
                {
                    player.character.transform.position = e.playerPosition;
                }
            }
        }
    }
}
