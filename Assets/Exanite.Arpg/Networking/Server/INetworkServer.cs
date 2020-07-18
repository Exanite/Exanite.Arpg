using System;
using System.Collections.Specialized;
using DarkRift.Dispatching;
using DarkRift.Server;
using UnityEngine;

namespace Exanite.Arpg.Networking.Server
{
    // This is mainly used to hide Unity-related members from users of this interface

    /// <summary>
    /// Interface for <see cref="UnityServer"/>
    /// </summary>
    public interface INetworkServer
    {
        /// <summary>
        /// Event fired when a client connects to this server
        /// </summary>
        event EventHandler<ClientConnectedEventArgs> OnClientConnected;

        /// <summary>
        /// Event fired when a client disconnects from this server
        /// </summary>
        event EventHandler<ClientDisconnectedEventArgs> OnClientDisconnected;

        /// <summary>
        /// Event fired when the server recieves a message from any client
        /// </summary>
        event EventHandler<MessageReceivedEventArgs> OnMessageRecieved;

        /// <summary>
        /// The XML configuration file to use
        /// </summary>
        TextAsset Configuration { get; set; }

        /// <summary>
        /// Should events be called from the Main Thread?
        /// </summary>
        bool UseMainThreadForEvents { get; set; }

        /// <summary>
        /// The client manager handling all clients on this server
        /// </summary>
        IClientManager ClientManager { get; }

        /// <summary>
        /// Dispatcher for running tasks on the main thread.
        /// </summary>
        IDispatcher Dispatcher { get; }

        /// <summary>
        /// Information about this server
        /// </summary>
        DarkRiftInfo ServerInfo { get; }

        /// <summary>
        /// Creates the server
        /// </summary>
        void Create();

        /// <summary>
        /// Creates the server with the specified variables
        /// </summary>
        void Create(NameValueCollection variables);

        /// <summary>
        /// Closes the server
        /// </summary>
        void Close();
    }
}