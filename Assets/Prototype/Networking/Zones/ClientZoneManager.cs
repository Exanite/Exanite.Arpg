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
        private UnityClient client;
        private ClientGameManager gameManager;

        [Inject]
        public void Inject(UnityClient client, ClientGameManager gameManager)
        {
            this.client = client;
            this.gameManager = gameManager; // ! replace with reference to local player later on
        }

        private Player LocalPlayer
        {
            get
            {
                return gameManager.localPlayer;
            }
        }

        private Zone CurrentZone
        {
            get
            {
                return gameManager.localPlayer.currentZone;
            }

            set
            {
                gameManager.localPlayer.currentZone = value;
            }
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
            LocalPlayer.isLoadingZone = true;

            var newZone = new Zone(e.guid);
            CurrentZone = newZone;

            client.SendPacketToServer(new ZoneCreateFinishedPacket() { guid = e.guid }, DeliveryMethod.ReliableOrdered);
            LocalPlayer.isLoadingZone = false;
        }

        private void OnZonePlayerEnter(NetPeer sender, ZonePlayerEnterPacket e)
        {
            if (!CurrentZone.playersById.ContainsKey(e.playerId))
            {
                Player player;
                if (LocalPlayer.Id == e.playerId)
                {
                    player = LocalPlayer;
                }
                else
                {
                    player = new Player(e.playerId)
                    {
                        currentZone = CurrentZone
                    };
                }

                CurrentZone.AddPlayer(player);

                player.CreatePlayerCharacter(CurrentZone);
                player.character.transform.position = e.playerPosition;

                if (e.playerId == LocalPlayer.Id) // works for now, but try avoiding checking the Id twice
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
            if (CurrentZone.playersById.TryGetValue(e.playerId, out Player player))
            {
                Destroy(player.character.gameObject);
                CurrentZone.RemovePlayer(player);
            }
        }
    }
}
