namespace Exanite.Arpg.Networking.Client
{
    /// <summary>
    /// The result returned by <see cref="UnityClient.ConnectAsync"/>
    /// </summary>
    public class ConnectResult
    {
        private readonly bool isSuccess;
        private readonly string failReason;

        /// <summary>
        /// Creates a new <see cref="ConnectResult"/>
        /// </summary>
        public ConnectResult(bool isSuccess, string failReason = Constants.DefaultReason)
        {
            this.isSuccess = isSuccess;
            this.failReason = failReason;
        }

        /// <summary>
        /// Was the connect a success?
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                return isSuccess;
            }
        }

        /// <summary>
        /// The reason the connected failed<para/>
        /// Note: Invalid if <see cref="IsSuccess"/> is <see langword="true"/>
        /// </summary>
        public string FailReason
        {
            get
            {
                return failReason;
            }
        }
    }
}
