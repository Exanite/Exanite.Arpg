using DarkRift;

namespace Exanite.Arpg.Networking.Shared
{
    /// <summary>
    /// Data sent during a <see cref="MessageTag.LoginRequest"/> message
    /// </summary>
    public class LoginRequest : IDarkRiftSerializable
    {
        private string playerName = string.Empty;
        private string gameVersion = string.Empty;

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
            PlayerName = e.Reader.ReadString();
            GameVersion = e.Reader.ReadString();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(PlayerName);
            e.Writer.Write(GameVersion);
        }
    }
}
