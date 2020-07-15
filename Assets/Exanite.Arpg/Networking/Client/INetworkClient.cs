using System;
using System.Net;
using DarkRift;
using DarkRift.Client;
using DarkRift.Dispatching;

namespace Exanite.Arpg.Networking.Client
{
    // This is mainly used to hide Unity-related members from users of this interface

    /// <summary>
    /// Interface for <see cref="UnityClient"/>
    /// </summary>
    public interface INetworkClient
    {
        /// <summary>
        /// Event fired when the client is disconnected from the server
        /// </summary>
        event EventHandler<DisconnectedEventArgs> OnDisconnected;

        /// <summary>
        /// Event fired when the client recieves a message from the server
        /// </summary>
        event EventHandler<MessageReceivedEventArgs> OnMessageReceived;

        /// <summary>
        /// The IP Address of the server the client will connect to
        /// </summary>
        IPAddress IPAddress { get; set; }

        /// <summary>
        /// The port on the server the client will connect to
        /// </summary>
        ushort Port { get; set; }

        /// <summary>
        /// Should Nagel's Algorithm be used?<para/>
        /// Enabling this will reduce the number of packets sent, but increase delay
        /// </summary>
        bool EnableNagelsAlgorithm { get; set; }

        /// <summary>
        /// Should events be called from the main thread?
        /// </summary>
        bool UseMainThreadForEvents { get; set; }

        /// <summary>
        /// The object cache settings in use
        /// </summary>
        ClientObjectCacheSettings ClientObjectCacheSettings { get; }

        /// <summary>
        /// The dispatcher for moving work to the main thread
        /// </summary>
        IDispatcher Dispatcher { get; }

        /// <summary>
        /// The ID the client has been assigned
        /// </summary>
        ushort ID { get; }

        /// <summary>
        /// Returns the state of the connection with the server
        /// </summary>
        ConnectionState ConnectionState { get; }

        /// <summary>
        /// The round trip time helper for this client
        /// </summary>
        RoundTripTimeHelper RoundTripTime { get; }

        /// <summary>
        /// Connects to a server asynchronously
        /// </summary>
        void ConnectInBackground(DarkRiftClient.ConnectCompleteHandler callback = null);

        /// <summary>
        /// Disconnects this client from the server
        /// </summary>
        /// <returns>Whether the disconnect was successful</returns>
        bool Disconnect();

        /// <summary>
        /// Sends a message to the server
        /// </summary>
        /// <returns>If the send was successful</returns>
        bool SendMessage(Message message, SendMode sendMode);
    }
}