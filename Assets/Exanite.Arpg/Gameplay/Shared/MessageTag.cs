namespace Exanite.Arpg.Gameplay.Shared
{
    public static class MessageTag
    {
        /// <summary>
        /// Client to server - Request for authentication from the client
        /// </summary>
        public const ushort LoginRequest = 0;
        /// <summary>
        /// Server to client - Reply to LoginRequest that the login succeeded
        /// </summary>
        public const ushort LoginRequestAccepted = 1;
        /// <summary>
        /// Server to client - Reply to LoginRequest that the login failed and the reason for disconnection
        /// </summary>
        public const ushort LoginRequestDenied = 2;

        /// <summary>
        /// Server to client - Tells the client to create a level
        /// </summary>
        public const ushort LevelLoad = 100;
        /// <summary>
        /// Client to server - Tells the server that the client finished loading a level
        /// </summary>
        public const ushort LevelLoadFinished = 101;

        /// <summary>
        /// Server to client - Tells the client to create a player
        /// </summary>
        public const ushort PlayerCreate = 200;
        /// <summary>
        /// Server to client - Tells the client to destroy a player
        /// </summary>
        public const ushort PlayerDestroy = 201;
        /// <summary>
        /// Client to server - Sends player input to the server
        /// </summary>
        public const ushort PlayerInput = 202;
        /// <summary>
        /// Server to client - Tells the client the new position of a player
        /// </summary>
        public const ushort PlayerPositionUpdate = 203;

        /// <summary>
        /// Server to client - Tells the client to register a new channel
        /// </summary>
        public const ushort RegisterChannel = 5000;
        /// <summary>
        /// Server to client - Tells the client to unregister an existing channel
        /// </summary>
        public const ushort UnregisterChannel = 5001;
        /// <summary>
        /// Two-way - Used for any message sent using a channel
        /// </summary>
        public const ushort ChannelMessage = 5002;
    }
}
