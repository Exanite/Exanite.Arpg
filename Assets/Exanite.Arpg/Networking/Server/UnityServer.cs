using System;
using System.Collections.Specialized;
using System.Threading;
using System.Xml.Linq;
using DarkRift;
using DarkRift.Dispatching;
using DarkRift.Server;
using Exanite.Arpg.Logging;
using Exanite.Arpg.Networking.Server.Authentication;
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
        private Authenticator authenticator;

        [Inject]
        public void Inject(ILog log, PlayerManager playerManager, Authenticator authenticator)
        {
            this.log = log;
            this.playerManager = playerManager;
            this.authenticator = authenticator;
        }

        /// <summary>
        /// Event fired when a player connects to this server
        /// </summary>
        public event EventHandler<PlayerConnectedArgs> OnPlayerConnected;

        /// <summary>
        /// Event fired when a player disconnects from this server
        /// </summary>
        public event EventHandler<PlayerDisconnectedArgs> OnPlayerDisconnected;

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

        public async UniTask<(bool isSuccess, object sender, MessageReceivedEventArgs e)>
            WaitForMessageWithTag(IClient client, ushort tag, int timeoutMilliseconds = Constants.DefaultTimeoutMilliseconds)
        {
            if (timeoutMilliseconds > Constants.DefaultTimeoutMilliseconds)
            {
                timeoutMilliseconds = Constants.DefaultTimeoutMilliseconds;
            }

            var source = new UniTaskCompletionSource<(object sender, MessageReceivedEventArgs e)>();

            EventHandler<MessageReceivedEventArgs> handler = (sender, e) =>
            {
                if (e.Tag == tag)
                {
                    source.TrySetResult((sender, e));
                }
            };

            client.MessageReceived += handler;

            await UniTask.WhenAny(source.Task, UniTask.Delay(timeoutMilliseconds, true));

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

        private void Server_OnClientConnected(object sender, ClientConnectedEventArgs e)
        {
            Server_OnClientConnectedAsync(sender, e).Forget();
        }

        private async UniTask Server_OnClientConnectedAsync(object sender, ClientConnectedEventArgs e)
        {
            var waitResult = await WaitForMessageWithTag(e.Client, MessageTag.LoginRequest, 10 * 1000);

            if (waitResult.isSuccess)
            {
                using (var message = waitResult.e.GetMessage())
                using (var reader = message.GetReader())
                {
                    var request = reader.ReadSerializable<LoginRequest>();

                    var authenticationResult = authenticator.Authenticate(request);

                    if (authenticationResult.IsSuccess)
                    {
                        var connection = new PlayerConnection()
                        {
                            ID = e.Client.ID,
                            Client = e.Client,

                            Name = request.PlayerName,
                        };

                        playerManager.AddPlayer(connection);

                        SendLoginRequestAccepted(e.Client);

                        e.Client.MessageReceived += OnMessageRecieved;
                        OnPlayerConnected?.Invoke(connection.Client, new PlayerConnectedArgs(connection));
                    }
                    else
                    {
                        SendLoginRequestDenied(e.Client, authenticationResult.FailReason);

                        await UniTask.Delay(250); // ! temporary fix to make sure the client receives the Response message

                        e.Client.Disconnect();
                    }
                }
            }
            else
            {
                SendLoginRequestDenied(e.Client, $"Login request timed out");

                e.Client.Disconnect();
            }
        }

        private void SendLoginRequestAccepted(IClient client)
        {
            var response = new LoginRequestReponse()
            {
                IsSuccess = true,
            };

            SendLoginRequestResponse(client, response);
        }

        private void SendLoginRequestDenied(IClient client, string reason = LoginRequestReponse.DefaultReason)
        {
            var response = new LoginRequestReponse()
            {
                IsSuccess = false,
                DisconnectReason = reason,
            };

            SendLoginRequestResponse(client, response);
        }

        private void SendLoginRequestResponse(IClient client, LoginRequestReponse response)
        {
            using (var writer = DarkRiftWriter.Create())
            {
                writer.Write(response);

                using (var message = Message.Create(MessageTag.LoginRequestResponse, writer))
                {
                    client.SendMessage(message, SendMode.Reliable);
                }
            }
        }

        private void Server_OnClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            if (playerManager.Contains(e.Client.ID))
            {
                var connection = playerManager.GetPlayerConnection(e.Client.ID);

                OnPlayerDisconnected?.Invoke(e.Client, new PlayerDisconnectedArgs(connection));
                e.Client.MessageReceived -= OnMessageRecieved;

                playerManager.RemovePlayer(connection);
            }
        }
    }
}
