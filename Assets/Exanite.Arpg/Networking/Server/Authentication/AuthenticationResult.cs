namespace Exanite.Arpg.Networking.Server.Authentication
{
    public class AuthenticationResult
    {
        public const string DefaultReason = "No reason was provided";

        private bool isSuccess;
        private string failReason = DefaultReason;

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

        public string FailReason
        {
            get
            {
                return failReason;
            }

            set
            {
                failReason = value;
            }
        }
    }
}
