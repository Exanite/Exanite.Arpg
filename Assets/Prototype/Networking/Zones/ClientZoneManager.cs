using Exanite.Arpg.Networking;
using Exanite.Arpg.Networking.Client;
using LiteNetLib;
using Prototype.Networking.Client;
using Prototype.Networking.Players;
using Prototype.Networking.Zones.Packets;
using Zenject;

namespace Prototype.Networking.Zones
{
    public class ClientZoneManager : ZoneManager, IPacketHandler
    {
        public Zone currentZone;
        public bool isLoadingZone;

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

        public override Zone GetPlayerCurrentZone(Player player)
        {
            if (currentZone.playersById.ContainsValue(player))
            {
                return currentZone;
            }

            return null;
        }

        public override bool IsPlayerLoading(Player player)
        {
            return isLoadingZone;
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
            isLoadingZone = true;

            var newZone = new Zone(e.guid);
            currentZone = newZone;

            client.SendPacketToServer(new ZoneCreateFinishedPacket() { guid = e.guid }, DeliveryMethod.ReliableOrdered);
            isLoadingZone = false;
        }

        private void OnZonePlayerEnter(NetPeer sender, ZonePlayerEnterPacket e)
        {
            if (!currentZone.playersById.ContainsKey(e.playerId))
            {
                Player player;
                if (LocalPlayer.Id == e.playerId)
                {
                    player = LocalPlayer;
                }
                else
                {
                    player = new Player(e.playerId, this);
                }

                currentZone.AddPlayer(player);

                player.CreatePlayerCharacter();
                player.Character.transform.position = e.playerPosition;

                if (e.playerId == LocalPlayer.Id) // works for now, but try avoiding checking the Id twice
                {
                    var controller = player.Character.gameObject.AddComponent<PlayerController>();
                    controller.player = player;
                    controller.client = client;

                    player.Character.name += " (Local)";
                }
            }
        }

        private void OnZonePlayerLeave(NetPeer sender, ZonePlayerLeavePacket e)
        {
            if (currentZone.playersById.TryGetValue(e.playerId, out Player player))
            {
                Destroy(player.Character.gameObject);
                currentZone.RemovePlayer(player);
            }
        }
    }
}
