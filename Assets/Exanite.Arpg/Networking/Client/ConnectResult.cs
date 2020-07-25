namespace Exanite.Arpg.Networking.Client
{
    public class ConnectResult
    {
        private readonly bool isSuccess;
        private readonly string failReason;

        public ConnectResult(bool isSuccess, string failReason = Constants.DefaultReason)
        {
            this.isSuccess = isSuccess;
            this.failReason = failReason;
        }

        public bool IsSuccess
        {
            get
            {
                return isSuccess;
            }
        }

        public string FailReason
        {
            get
            {
                return failReason;
            }
        }
    }
}
