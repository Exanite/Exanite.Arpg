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
        public ushort port = Constants.DefaultPort;

        public bool isCreated = false;

        private EventBasedNetListener netListener;
        private NetManager netManager;
        private NetPacketProcessor netPacketProcessor;

        private ILog log;

        [Inject]
        public void Inject(ILog log)
        {
            this.log = log;
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
            if (isCreated)
            {
                throw new InvalidOperationException("Server has already been created.");
            }

            netManager.Start(port);

            isCreated = true;
        }

        public void Close()
        {
            if (!isCreated)
            {
                return;
            }

            netManager.DisconnectAll();
            netManager.Stop();

            isCreated = false;
        }

        private void UnityServer_ConnectionRequestEvent(ConnectionRequest request)
        {
            request.AcceptIfKey(Constants.ConnectionKey);
        }
    }
}
