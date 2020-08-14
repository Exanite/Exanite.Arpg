using Exanite.Arpg;
using Prototype.Networking.Zones;

namespace Prototype.Networking.Players
{
    public class Player
    {
        private readonly int id;

        // ! functionality not implemented yet
        public bool isLoadingZone;
        public Zone currentZone;

        public PlayerCharacter character;
        
        public Player(int id)
        {
            this.id = id;
        }

        public int Id
        {
            get
            {
                return id;
            }
        }

        public void CreatePlayerCharacter(Zone zone)
        {
            character = zone.scene.InstantiateNew($"Player {Id}").AddComponent<PlayerCharacter>();
            character.player = this;
        }
    }
}
