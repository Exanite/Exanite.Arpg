using System;
using Exanite.Arpg;
using Prototype.Networking.Zones;

namespace Prototype.Networking.Players
{
    public class Player
    {
        public bool isLoadingZone;

        public PlayerCharacter character;

        private readonly int id;
        private readonly ZoneManager zoneManager;

        public Player(int id, ZoneManager zoneManager)
        {
            this.id = id;
            this.zoneManager = zoneManager;
        }

        public int Id
        {
            get
            {
                return id;
            }
        }

        public Zone CurrentZone
        {
            get
            {
                return zoneManager.GetZoneWithPlayer(this);
            }
        }

        public void CreatePlayerCharacter()
        {
            if (CurrentZone == null)
            {
                throw new InvalidOperationException("Player is currently not in a Zone");
            }

            character = CurrentZone.scene.InstantiateNew($"Player {Id}").AddComponent<PlayerCharacter>();
            character.player = this;
        }
    }
}
