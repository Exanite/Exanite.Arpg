using System.Net;

namespace Prototype.Networking.Startup
{
    public class GameStartSettings // likely will split into Server and Client implementations later on
    {
        public GameType gameType;

        public ushort port;
        public string username;
        public IPAddress address;

        public GameStartSettings(GameType gameType, ushort port, string username, IPAddress address)
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
