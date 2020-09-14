using Prototype.Networking.Players.Data;
using UnityEngine;

namespace Prototype.Movement
{
    public class ClientPlayerCharacter : MonoBehaviour
    {
        public ServerPlayerCharacter server;
        public uint tick;

        private RingBuffer<PlayerUpdateData> updateBuffer;

        private void Start()
        {
            updateBuffer = new RingBuffer<PlayerUpdateData>(64);
        }

        private void FixedUpdate()
        {
            // input

            // state

            // simulation

            // view

            // messaging

            tick++;
        }

        public void ReceivePlayerUpdate(PlayerUpdateData data)
        {
            if (!updateBuffer.IsFull)
            {
                updateBuffer.Enqueue(data);
            }
        }
    }
}
