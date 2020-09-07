using Prototype.Networking.Zones;

namespace Prototype.Networking.Players
{
    /// <summary>
    /// Represents a player connected to the server
    /// </summary>
    public class ServerPlayer : Player
    {
        private PlayerMovementBehaviour movementBehaviour;

        private readonly PlayerConnection connection;

        /// <summary>
        /// Creates a new <see cref="ServerPlayer"/>
        /// </summary>
        public ServerPlayer(PlayerConnection connection, ServerZoneManager zoneManager) : base(connection.Id, zoneManager)
        {
            this.connection = connection;
        }

        /// <summary>
        /// The <see cref="Player"/>'s connection
        /// </summary>
        public PlayerConnection Connection
        {
            get
            {
                return connection;
            }
        }

        public PlayerMovementBehaviour MovementBehaviour
        {
            get
            {
                return movementBehaviour;
            }

            private set
            {
                movementBehaviour = value;
            }
        }

        public override void CreatePlayerCharacter()
        {
            base.CreatePlayerCharacter();

            MovementBehaviour = Character.gameObject.AddComponent<PlayerMovementBehaviour>();
            MovementBehaviour.zone = CurrentZone;
        }
    }
}
