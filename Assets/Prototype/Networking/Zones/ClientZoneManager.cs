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
        public string zoneSceneName = "Zone";

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
            network.RegisterPacketReceiver<ZoneLoadPacket>(OnZoneLoad);
            network.RegisterPacketReceiver<ZonePlayerEnteredPacket>(OnZonePlayerEntered);
            network.RegisterPacketReceiver<ZonePlayerLeftPacket>(OnZonePlayerLeft);
        }

        public void UnregisterPackets(UnityNetwork network)
        {
            network.ClearPacketReceiver<ZoneLoadPacket>();
            network.ClearPacketReceiver<ZonePlayerEnteredPacket>();
            network.ClearPacketReceiver<ZonePlayerLeftPacket>();
        }

        private void OnZoneLoad(NetPeer sender, ZoneLoadPacket e)
        {
            isLoadingZone = true;

            var newZone = new Zone(e.guid, zoneSceneName);
            currentZone = newZone;

            client.SendPacketToServer(new ZoneLoadFinishedPacket() { guid = e.guid }, DeliveryMethod.ReliableOrdered);
            isLoadingZone = false;
        }

        private void OnZonePlayerEntered(NetPeer sender, ZonePlayerEnteredPacket e)
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

        private void OnZonePlayerLeft(NetPeer sender, ZonePlayerLeftPacket e)
        {
            if (currentZone.playersById.TryGetValue(e.playerId, out Player player))
            {
                Destroy(player.Character.gameObject);
                currentZone.RemovePlayer(player);
            }
        }
    }
}
