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

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
            {
                GUILayout.FlexibleSpace();

                GUILayout.Label($"--Server--");
                GUILayout.Label($"Tick: {tick}");
                GUILayout.Label($"InputBuffer.Count: {inputBuffer.Count}");
            }
            GUILayout.EndArea();
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
