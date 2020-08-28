using System.Net;

namespace Prototype.Networking.Startup
{
    public class GameStartSettings // likely will split into Server and Client implementations later on
    {
        public GameType gameType;

        public string username;

        public IPAddress address;
        public ushort port;

        public GameStartSettings(GameType gameType, string username, IPAddress address, ushort port)
        {
            this.gameType = gameType;
            this.port = port;
            this.username = username;
            this.address = address;
        }

        public enum GameType
        {
            Server,
            Client,
        }
    } 
}
