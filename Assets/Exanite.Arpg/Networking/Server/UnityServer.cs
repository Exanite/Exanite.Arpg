using System;
using System.Collections.Specialized;
using System.Threading;
using System.Xml.Linq;
using DarkRift.Dispatching;
using DarkRift.Server;
using Exanite.Arpg.Logging;
using Exanite.Arpg.Networking.Shared;
using UniRx.Async;
using UnityEngine;
using Zenject;

namespace Exanite.Arpg.Networking.Server
{
    /// <summary>
    /// Used to start a server
    /// </summary>
    public class UnityServer : MonoBehaviour
    {
        [SerializeField] private TextAsset configuration;
        [SerializeField] private bool useMainThreadForEvents = true;

        private DarkRiftServer server;

        private ILog log;
        private PlayerManager playerManager;

        [Inject]
        public void Inject(ILog log, PlayerManager playerManager)
        {
            this.log = log;
            this.playerManager = playerManager;
        }

        /// <summary>
        /// Event fired when a player connects to this server
        /// </summary>
        public event EventHandler<ClientConnectedEventArgs> OnPlayerConnected;

        /// <summary>
        /// Event fired when a player disconnects from this server
        /// </summary>
        public event EventHandler<ClientDisconnectedEventArgs> OnPlayerDisconnected;

        /// <summary>
        /// Event fired when the server recieves a message from any player
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs> OnMessageRecieved;

        /// <summary>
        /// The XML configuration file to use
        /// </summary>
        public TextAsset Configuration
        {
            get
            {
                return configuration;
            }

            set
            {
                configuration = value;
            }
        }

        /// <summary>
        /// Should events be called from the Main Thread?
        /// </summary>
        public bool UseMainThreadForEvents
        {
            get
            {
                return useMainThreadForEvents;
            }

            set
            {
                useMainThreadForEvents = value;
            }
        }

        /// <summary>
        /// The client manager handling all clients on this server
        /// </summary>
        public IClientManager ClientManager
        {
            get
            {
                return server?.ClientManager;
            }
        }

        /// <summary>
        /// Dispatcher for running tasks on the main thread.
        /// </summary>
        public IDispatcher Dispatcher
        {
            get
            {
                return server?.Dispatcher;
            }
        }

        /// <summary>
        /// Information about this server
        /// </summary>
        public DarkRiftInfo ServerInfo
        {
            get
            {
                return server?.ServerInfo;
            }
        }

        private void Update()
        {
            server?.ExecuteDispatcherTasks();
        }

        private void OnDisable()
        {
            Close();
        }

        private void OnApplicationQuit()
        {
            Close();
        }

        /// <summary>
        /// Creates the server
        /// </summary>
        public void Create() // Cannot use Start due to Unity
        {
            Create(new NameValueCollection());
        }

        /// <summary>
        /// Creates the server with the specified variables
        /// </summary>
        public void Create(NameValueCollection variables)
        {
            if (server != null)
            {
                throw new InvalidOperationException("The server has already been created.");
            }

            if (Configuration == null)
            {
                log.Error("No configuration file specified");
                return;
            }

            ServerSpawnData spawnData = ServerSpawnData.CreateFromXml(XDocument.Parse(Configuration.text), variables);
            spawnData.DispatcherExecutorThreadID = Thread.CurrentThread.ManagedThreadId;
            spawnData.EventsFromDispatcher = UseMainThreadForEvents;

            server = new DarkRiftServer(spawnData);
            server.Start();

            server.ClientManager.ClientConnected += Server_OnClientConnected;
            server.ClientManager.ClientDisconnected += Server_OnClientDisconnected;
        }

        /// <summary>
        /// Closes the server
        /// </summary>
        public void Close()
        {
            if (server != null)
            {
                server.ClientManager.ClientConnected -= Server_OnClientConnected;
                server.ClientManager.ClientDisconnected -= Server_OnClientDisconnected;

                server.Dispose();
            }
        }

        private void Server_OnClientConnected(object sender, ClientConnectedEventArgs e)
        {
            Server_OnClientConnectedAsync(sender, e).Forget();

            // wait for client login request
            // if the client does not respond in 10 sec, send timeout packet and disconnect
            // if the client responds, process login request
        }

        private async UniTask Server_OnClientConnectedAsync(object sender, ClientConnectedEventArgs e)
        {
            var result = await WaitForMessageWithTag(e.Client, MessageTag.LoginRequest, 10 * 1000);

            if (result.success)
            {
                using (var message = result.e.GetMessage())
                using (var reader = message.GetReader())
                {
                    var request = reader.ReadSerializable<LoginRequestData>();

                    if (Application.version != request.GameVersion)
                    {
                        //! send login request denied packet first

                        e.Client.Disconnect();
                    }

                    if (playerManager.Contains(request.PlayerName))
                    {
                        //! send login request denied packet first

                        e.Client.Disconnect();
                    }

                    playerManager.AddPlayer(new PlayerConnection()
                    {
                        Id = e.Client.ID,
                        Client = e.Client,

                        Name = request.PlayerName,
                    });

                    //! fire OnPlayerConnected event
                }
            }
            else
            {
                //! send login request denied packet first

                e.Client.Disconnect();
            }
        }

        private async UniTask<(bool success, object sender, MessageReceivedEventArgs e)> WaitForMessageWithTag(IClient client, ushort tag, int millisecondsTimeout = -1)
        {
            var source = new UniTaskCompletionSource<(object sender, MessageReceivedEventArgs e)>();

            EventHandler<MessageReceivedEventArgs> handler = (sender, e) =>
            {
                if (e.Tag == tag)
                {
                    source.TrySetResult((sender, e));
                }
            };

            client.MessageReceived += handler;

            if (millisecondsTimeout > 0)
            {
                await UniTask.WhenAny(source.Task, UniTask.Delay(millisecondsTimeout));
            }
            else
            {
                await source.Task;
            }

            client.MessageReceived -= handler;

            if (source.Task.IsCompleted)
            {
                var result = source.Task.Result;

                return (true, result.sender, result.e);
            }
            else
            {
                return (false, null, null);
            }
        }

        private void Server_OnClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            // check if it is an authenticated player
            // if it is, remove player and fire event
        }
    }
}
