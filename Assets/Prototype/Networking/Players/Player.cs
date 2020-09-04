using System;
using Exanite.Arpg;
using Prototype.Networking.Zones;

namespace Prototype.Networking.Players
{
    /// <summary>
    /// Represents a player in the game
    /// </summary>
    public class Player
    {
        private PlayerCharacter character;

        private readonly int id;
        private readonly ZoneManager zoneManager;

        /// <summary>
        /// Creates a new <see cref="Player"/>
        /// </summary>
        public Player(int id, ZoneManager zoneManager)
        {
            this.id = id;
            this.zoneManager = zoneManager;
        }

        /// <summary>
        /// The Id of the player
        /// </summary>
        public int Id
        {
            get
            {
                return id;
            }
        }

        /// <summary>
        /// The <see cref="Zone"/> the <see cref="Player"/> is currently in
        /// </summary>
        public Zone CurrentZone
        {
            get
            {
                return zoneManager.GetPlayerCurrentZone(this);
            }
        }

        /// <summary>
        /// Is the <see cref="Player"/> currently loading a <see cref="Zone"/>?
        /// </summary>
        public bool IsLoadingZone
        {
            get
            {
                return zoneManager.IsPlayerLoading(this);
            }
        }

        /// <summary>
        /// The <see cref="PlayerCharacter"/> of the <see cref="Player"/><para/>
        /// Note: <see langword="null"/> if the <see cref="Player"/> is not in a <see cref="Zone"/>
        /// </summary>
        public PlayerCharacter Character
        {
            get
            {
                return character;
            }

            protected set
            {
                character = value;
            }
        }

        /// <summary>
        /// Creates a new <see cref="PlayerCharacter"/> in the <see cref="Player"/>'s current zone
        /// </summary>
        public virtual void CreatePlayerCharacter()
        {
            if (CurrentZone == null)
            {
                throw new InvalidOperationException("Player is currently not in a Zone");
            }

            Character = CurrentZone.scene.InstantiateNew($"Player {Id}").AddComponent<PlayerCharacter>();
            Character.player = this;
        }
    }
}
