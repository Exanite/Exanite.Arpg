using Exanite.Arpg.Networking;
using Exanite.Arpg.Networking.Client;
using LiteNetLib;
using Prototype.Networking.Client;
using Prototype.Networking.Players;
using Prototype.Networking.Zones.Packets;
using UnityEngine;
using Zenject;

namespace Prototype.Networking.Zones
{
    public class ClientZoneManager : MonoBehaviour, IPacketHandler
    {
        public Zone currentZone;

        private UnityClient client;
        private ClientGameManager gameManager;

        [Inject]
        public void Inject(UnityClient client, ClientGameManager gameManager)
        {
            this.client = client;
            this.gameManager = gameManager; // ! replace with reference to local player later on
        }

        public void RegisterPackets(UnityNetwork network)
        {
            network.RegisterPacketReceiver<ZoneCreatePacket>(OnZoneCreate);
            network.RegisterPacketReceiver<ZonePlayerEnterPacket>(OnZonePlayerEnter);
            network.RegisterPacketReceiver<ZonePlayerLeavePacket>(OnZonePlayerLeave);
        }

        public void UnregisterPackets(UnityNetwork network)
        {
            network.ClearPacketReceiver<ZoneCreatePacket>();
            network.ClearPacketReceiver<ZonePlayerEnterPacket>();
            network.ClearPacketReceiver<ZonePlayerLeavePacket>();
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

                if (e.playerId == gameManager.id)
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
    }
}
