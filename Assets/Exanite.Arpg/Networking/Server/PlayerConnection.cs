using DarkRift.Server;

namespace Exanite.Arpg.Networking.Server
{
    /// <summary>
    /// Represents a player connected to the server
    /// </summary>
    public class PlayerConnection
    {
        private ushort id;
        private IClient client;

        private string name;

        /// <summary>
        /// Network ID of the connected player
        /// </summary>
        public ushort ID
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        /// <summary>
        /// The <see cref="IClient"/> of the connected player
        /// </summary>
        public IClient Client
        {
            get
            {
                return client;
            }

            set
            {
                client = value;
            }
        }

        /// <summary>
        /// The name of the connected player
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }
    } 
}
