using DarkRift;

namespace Exanite.Arpg.Networking.Shared
{
    /// <summary>
    /// Data sent during a <see cref="MessageTag.LoginRequest"/>
    /// </summary>
    public class LoginRequestData : IDarkRiftSerializable
    {
        private string playerName;
        private string gameVersion;

        /// <summary>
        /// Name of the player that is requesting the connection
        /// </summary>
        public string PlayerName
        {
            get
            {
                return playerName;
            }

            set
            {
                playerName = value;
            }
        }

        /// <summary>
        /// Game version the player's client is using
        /// </summary>
        public string GameVersion
        {
            get
            {
                return gameVersion;
            }

            set
            {
                gameVersion = value;
            }
        }

        public void Deserialize(DeserializeEvent e)
        {
            playerName = e.Reader.ReadString();
            gameVersion = e.Reader.ReadString();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(PlayerName);
            e.Writer.Write(GameVersion);
        }
    }
}
