using Prototype.Networking.Players.Data;
using UnityEngine;

namespace Prototype.Movement
{
    public class ServerPlayerCharacter : MonoBehaviour
    {
        public ClientPlayerCharacter client;
        public uint tick;

        private RingBuffer<PlayerInputData> inputBuffer;

        private void Start()
        {
            inputBuffer = new RingBuffer<PlayerInputData>(64);
        }

        private void FixedUpdate()
        {
            // input

            // state

            // simulation

            // messaging

            tick++;
        }

        public void ReceivePlayerInput(PlayerInputData data)
        {
            if (!inputBuffer.IsFull)
            {
                inputBuffer.Enqueue(data);
            }
        }
    }
}
