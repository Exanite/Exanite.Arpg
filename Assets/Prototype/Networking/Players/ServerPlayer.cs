using Prototype.Networking.Zones;

namespace Prototype.Networking.Players
{
    public class ServerPlayer : Player
    {
        private readonly PlayerConnection connection;
        private readonly ServerZoneManager zoneManager;

        public ServerPlayer(PlayerConnection connection, ServerZoneManager zoneManager) : base(connection.Id, zoneManager)
        {
            this.connection = connection;
        }

        public PlayerConnection Connection
        {
            get
            {
                return connection;
            }
        }
    }
}
