using DarkRift;

namespace Exanite.Arpg.Networking.Shared
{
    /// <summary>
    /// Data sent during a <see cref="MessageTag.LoginRequestDenied"/> message
    /// </summary>
    public class LoginRequestReponse : IDarkRiftSerializable
    {
        /// <summary>
        /// Default reason for disconnection
        /// </summary>
        public const string DefaultReason = "No reason was provided";

        private bool isSuccess;
        private string disconnectReason = DefaultReason;

        /// <summary>
        /// Was the authentication successful?
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                return isSuccess;
            }

            set
            {
                isSuccess = value;
            }
        }

        /// <summary>
        /// The reason for the disconnection, invalid if authentication succeeded
        /// </summary>
        public string DisconnectReason
        {
            get
            {
                return disconnectReason;
            }

            set
            {
                disconnectReason = value;
            }
        }

        public void Deserialize(DeserializeEvent e)
        {
            IsSuccess = e.Reader.ReadBoolean();
            DisconnectReason = e.Reader.ReadString();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(IsSuccess);
            e.Writer.Write(DisconnectReason);
        }
    }
}
