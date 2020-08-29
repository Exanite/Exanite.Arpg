using System.Net;

namespace Prototype.Networking.Startup
{
    public class GameStartSettings // likely will split into Server and Client implementations later on
    {
        public GameType gameType = GameType.Client;

        public string username = "Player";
        public bool useAI = false;

        public IPAddress address = IPAddress.Loopback;
        public ushort port = 17175;
    }
}
