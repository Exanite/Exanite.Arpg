using DarkRift;

namespace Exanite.Arpg.Networking.Shared
{
    /// <summary>
    /// Data sent during a <see cref="MessageTag.LoginRequestDenied"/> message
    /// </summary>
    public class LoginRequestReponseData : IDarkRiftSerializable
    {
        private bool isSuccess;
        private string reason = string.Empty;

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
        /// The reason for the disconnection, <see langword="null"/> if authentication succeeded
        /// </summary>
        public string DisconnectReason
        {
            get
            {
                return reason;
            }

            set
            {
                reason = value;
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
