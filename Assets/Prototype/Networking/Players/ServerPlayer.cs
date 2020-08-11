namespace Prototype.Networking.Players
{
    public class ServerPlayer : Player
    {
        private readonly PlayerConnection connection;

        public ServerPlayer(PlayerConnection connection) : base(connection.Id)
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
