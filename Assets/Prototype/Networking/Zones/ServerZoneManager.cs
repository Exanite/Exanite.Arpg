using System;
using System.Collections.Generic;
using Exanite.Arpg.Networking;
using Exanite.Arpg.Networking.Server;
using LiteNetLib;
using Prototype.Networking.Players;
using Prototype.Networking.Zones.Packets;
using UnityEngine;
using Zenject;

namespace Prototype.Networking.Zones
{
    public class ServerZoneManager : MonoBehaviour, IPacketHandler
    {
        public Dictionary<Guid, Zone> zones = new Dictionary<Guid, Zone>();

        private UnityServer server;
        private ServerPlayerManager playerManager;

        [Inject]
        public void Inject(UnityServer server, ServerPlayerManager playerManager)
        {
            this.server = server;
            this.playerManager = playerManager;
        }

        public void RegisterPackets(UnityNetwork network)
        {
            network.RegisterPacketReceiver<ZoneCreateFinishedPacket>(OnZoneCreateFinished);
        }

        public void UnregisterPackets(UnityNetwork network)
        {
            network.ClearPacketReceiver<ZoneCreateFinishedPacket>();
        }

        private void OnZoneCreateFinished(NetPeer sender, ZoneCreateFinishedPacket e)
        {
            if (playerManager.TryGetPlayer(sender.Id, out ServerPlayer newPlayer))
            {
                var zone = zones[e.guid];

                newPlayer.CreatePlayerCharacter(zone); // ! should check if player is loading zone on server or not first
                zone.AddPlayer(newPlayer);

                foreach (ServerPlayer playerInZone in zone.playersById.Values) // needs refactoring
                {
                    server.SendPacket(
                        playerInZone.Connection.Peer,
                        new ZonePlayerEnterPacket()
                        {
                            playerId = newPlayer.Id,
                            playerPosition = newPlayer.character.transform.position,
                        },
                        DeliveryMethod.ReliableOrdered);

                    server.SendPacket(
                        newPlayer.Connection.Peer,
                        new ZonePlayerEnterPacket()
                        {
                            playerId = playerInZone.Id,
                            playerPosition = playerInZone.character.transform.position,
                        },
                        DeliveryMethod.ReliableOrdered);
                }
            }
        }
    }
}
