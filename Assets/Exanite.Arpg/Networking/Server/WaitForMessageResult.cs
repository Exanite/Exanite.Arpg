using DarkRift.Server;

namespace Exanite.Arpg.Networking.Server
{
    public class WaitForMessageResult
    {
        private readonly bool isSuccess;
        private readonly object sender;
        private readonly MessageReceivedEventArgs e;

        public WaitForMessageResult(bool isSuccess, object sender, MessageReceivedEventArgs e)
        {
            this.isSuccess = isSuccess;
            this.sender = sender;
            this.e = e;
        }

        public bool IsSuccess
        {
            get
            {
                return isSuccess;
            }
        }

        public object Sender
        {
            get
            {
                return sender;
            }
        }

        public MessageReceivedEventArgs E
        {
            get
            {
                return e;
            }
        }
    }
}
