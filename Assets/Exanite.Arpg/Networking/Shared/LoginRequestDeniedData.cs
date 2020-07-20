using DarkRift;

namespace Exanite.Arpg.Networking.Shared
{
    /// <summary>
    /// Data sent during a <see cref="MessageTag.LoginRequestDenied"/> message
    /// </summary>
    public class LoginRequestDeniedData : IDarkRiftSerializable
    {
        private string reason;

        /// <summary>
        /// The reason for the disconnection
        /// </summary>
        public string Reason
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
            Reason = e.Reader.ReadString();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(Reason);
        }
    }
}
