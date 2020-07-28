using System;
using Exanite.Arpg.Logging;
using LiteNetLib;
using LiteNetLib.Utils;
using UnityEngine;
using Zenject;

namespace Exanite.Arpg.NewNetworking.Server
{
    public class UnityServer : MonoBehaviour
    {
        private ushort port = Constants.DefaultPort;

        private bool isCreated = false;

        private EventBasedNetListener netListener;
        private NetManager netManager;
        private NetPacketProcessor netPacketProcessor;

        private ILog log;

        [Inject]
        public void Inject(ILog log)
        {
            this.log = log;
        }

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

        public bool IsCreated
        {
            get
            {
                return isCreated;
            }

            private set
            {
                isCreated = value;
            }
        }

        private void Awake()
        {
            netListener = new EventBasedNetListener();
            netManager = new NetManager(netListener);
            netPacketProcessor = new NetPacketProcessor();

            netListener.ConnectionRequestEvent += UnityServer_ConnectionRequestEvent;
        }

        private void Start() // ! temp
        {
            Create();
        }

        private void FixedUpdate()
        {
            netManager.PollEvents();
        }

        private void OnDestroy()
        {
            netListener.ConnectionRequestEvent -= UnityServer_ConnectionRequestEvent;

            Close();
        }

        public void Create()
        {
            if (IsCreated)
            {
                throw new InvalidOperationException("Server has already been created.");
            }

            netManager.Start(Port);

            IsCreated = true;
        }

        public void Close()
        {
            if (!IsCreated)
            {
                return;
            }

            netManager.DisconnectAll();
            netManager.Stop();

            IsCreated = false;
        }

        private void UnityServer_ConnectionRequestEvent(ConnectionRequest request)
        {
            request.AcceptIfKey(Constants.ConnectionKey);
        }
    }
}
