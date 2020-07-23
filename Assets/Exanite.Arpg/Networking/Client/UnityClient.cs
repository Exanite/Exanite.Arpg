using System;
using System.Net;
using DarkRift;
using DarkRift.Client;
using DarkRift.Dispatching;
using Exanite.Arpg.Logging;
using Exanite.Arpg.Networking.Shared;
using UniRx.Async;
using UnityEngine;
using Zenject;

namespace Exanite.Arpg.Networking.Client
{
    /// <summary>
    /// Client used to connect to a server
    /// </summary>
    public class UnityClient : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField] private string address = IPAddress.Loopback.ToString();
        [SerializeField] private ushort port = 4296;
        [SerializeField] private volatile bool useMainThreadForEvents = true;
        [SerializeField] private SerializableClientObjectCacheSettings serializableClientObjectCacheSettings = new SerializableClientObjectCacheSettings();

        private ClientObjectCacheSettings clientObjectCacheSettings;
        private IPAddress ipAddress;
        private Dispatcher dispatcher;

        private DarkRiftClient client;

        private ILog log;

        [Inject]
        public void Inject(ILog log)
        {
            this.log = log;
        }

        /// <summary>
        /// Event fired when the client is disconnected from the server
        /// </summary>
        public event EventHandler<DisconnectedEventArgs> OnDisconnected;

        /// <summary>
        /// Event fired when the client recieves a message from the server
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs> OnMessageReceived;

        /// <summary>
        /// The IP Address of the server the client will connect to
        /// </summary>
        public IPAddress IPAddress
        {
            get
            {
                return ipAddress;
            }

            set
            {
                ipAddress = value;
            }
        }

        /// <summary>
        /// The port on the server the client will connect to
        /// </summary>
        public ushort Port
        {
            get
            {
                return port;
            }

            set
            {
                port = value;
            }
        }

        /// <summary>
        /// Should events be called from the main thread?
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
        /// The object cache settings in use
        /// </summary>
        public ClientObjectCacheSettings ClientObjectCacheSettings
        {
            get
            {
                return clientObjectCacheSettings;
            }

            private set
            {
                clientObjectCacheSettings = value;
            }
        }

        /// <summary>
        /// The dispatcher for moving work to the main thread
        /// </summary>
        public IDispatcher Dispatcher
        {
            get
            {
                return dispatcher;
            }
        }

        /// <summary>
        /// The ID the client has been assigned
        /// </summary>
        public ushort ID
        {
            get
            {
                return client.ID;
            }
        }

        /// <summary>
        /// Returns the state of the connection with the server
        /// </summary>
        public ConnectionState ConnectionState
        {
            get
            {
                return client.ConnectionState;
            }
        }

        /// <summary>
        /// The round trip time helper for this client
        /// </summary>
        public RoundTripTimeHelper RoundTripTime
        {
            get
            {
                return client.RoundTripTime;
            }
        }

        private void Awake()
        {
            ClientObjectCacheSettings = serializableClientObjectCacheSettings.ToClientObjectCacheSettings();

            client = new DarkRiftClient(ClientObjectCacheSettings);

            dispatcher = new Dispatcher(true);

            client.MessageReceived += Client_OnMessageReceived;
            client.Disconnected += Client_OnDisconnected;
        }

        private void Update()
        {
            dispatcher.ExecuteDispatcherTasks();
        }

        private void OnDestroy()
        {
            Close();
        }

        private void OnApplicationQuit()
        {
            Close();
        }

        public async UniTask<bool> ConnectAsync(LoginRequestData loginRequest)
        {
            return await ConnectAsync(loginRequest, IPAddress, Port);
        }

        public async UniTask<bool> ConnectAsync(LoginRequestData loginRequest, IPAddress ip, int port) // todo clean up
        {
            var source = new UniTaskCompletionSource();

            client.ConnectInBackground(ip, port, true, (e) =>
            {
                source.TrySetResult();
            });

            await source.Task;

            if (ConnectionState == ConnectionState.Connected)
            {
                await UniTask.SwitchToMainThread();

                SendLoginRequest(loginRequest);

                var result = await WaitForMessageWithTag(MessageTag.LoginRequestResponse);

                if (result.success)
                {
                    using (var message = result.e.GetMessage())
                    using (var reader = message.GetReader())
                    {
                        var response = reader.ReadSerializable<LoginRequestReponseData>();

                        if (response.IsSuccess)
                        {
                            log.Information($"Connected to {ip} on port {port}.");

                            return true;
                        }
                        else
                        {
                            log.Information($"Connection failed to {ip} on port {port}. Reason: {response.DisconnectReason}");

                            return false;
                        }
                    }
                }
            }

            log.Information($"Connection failed to {ip} on port {port}. Reason: The server failed to respond");

            return false;
        }

        private async UniTask<(bool success, object sender, MessageReceivedEventArgs e)> WaitForMessageWithTag(ushort tag, int millisecondsTimeout = -1)
        {
            var source = new UniTaskCompletionSource<(object sender, MessageReceivedEventArgs e)>();

            EventHandler<MessageReceivedEventArgs> handler = (sender, e) =>
            {
                if (e.Tag == tag)
                {
                    source.TrySetResult((sender, e));
                }
            };

            OnMessageReceived += handler;

            if (millisecondsTimeout > 0)
            {
                await UniTask.WhenAny(source.Task, UniTask.Delay(millisecondsTimeout));
            }
            else
            {
                await source.Task;
            }

            OnMessageReceived -= handler;

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

        private void SendLoginRequest(LoginRequestData request)
        {
            using (var message = Message.Create(MessageTag.LoginRequest, request))
            {
                SendMessage(message, SendMode.Reliable);
            }
        }

        /// <summary>
        /// Disconnects this client from the server
        /// </summary>
        /// <returns>If the disconnect was successful</returns>
        public bool Disconnect()
        {
            return client.Disconnect();
        }

        /// <summary>
        /// Sends a message to the server
        /// </summary>
        /// <returns>If the send was successful</returns>
        public bool SendMessage(Message message, SendMode sendMode)
        {
            return client.SendMessage(message, sendMode);
        }

        /// <summary>
        /// Closes this client
        /// </summary>
        private void Close()
        {
            client.MessageReceived -= Client_OnMessageReceived;
            client.Disconnected -= Client_OnDisconnected;

            client.Dispose();
            dispatcher.Dispose();
        }

        private void Client_OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            if (UseMainThreadForEvents)
            {
                Message message = e.GetMessage();
                MessageReceivedEventArgs args = MessageReceivedEventArgs.Create(message, e.SendMode);

                Dispatcher.InvokeAsync(() =>
                {
                    EventHandler<MessageReceivedEventArgs> handler = OnMessageReceived;
                    if (handler != null)
                    {
                        handler.Invoke(sender, args);
                    }

                    message.Dispose();
                    args.Dispose();
                });
            }
            else
            {
                EventHandler<MessageReceivedEventArgs> handler = OnMessageReceived;

                if (handler != null)
                {
                    handler.Invoke(sender, e);
                }
            }
        }

        private void Client_OnDisconnected(object sender, DisconnectedEventArgs e)
        {
            if (UseMainThreadForEvents)
            {
                if (!e.LocalDisconnect)
                {
                    log.Information("Disconnected from server, disconnect code: " + e.Error);
                }

                Dispatcher.InvokeAsync(() =>
                {
                    EventHandler<DisconnectedEventArgs> handler = OnDisconnected;
                    if (handler != null)
                    {
                        handler.Invoke(sender, e);
                    }
                });
            }
            else
            {
                if (!e.LocalDisconnect)
                {
                    log.Information("Disconnected from server, disconnect code: " + e.Error);
                }

                EventHandler<DisconnectedEventArgs> handler = OnDisconnected;

                if (handler != null)
                {
                    handler.Invoke(sender, e);
                }
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (IPAddress != null)
            {
                address = IPAddress.ToString();
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            IPAddress = IPAddress.Parse(address);
        }
    }
}
