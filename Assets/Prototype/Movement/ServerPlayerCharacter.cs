using Exanite.Arpg.Collections;
using Prototype.Networking.Players.Data;
using UnityEngine;

namespace Prototype.Movement
{
    public class ServerPlayerCharacter : MonoBehaviour
    {
        public ClientPlayerCharacter client;
        public uint tick;
        public float mapSize = 10;

        private PlayerUpdateData currentUpdateData;

        private RingBuffer<PlayerInputData> inputBuffer;

        private PlayerLogic logic;

        private void Start()
        {
            inputBuffer = new RingBuffer<PlayerInputData>(64);

            logic = new PlayerLogic(mapSize);
        }

        private void FixedUpdate()
        {
            // input
            inputBuffer.TryDequeue(out PlayerInputData inputData);

            // simulation
            currentUpdateData = logic.Simulate(currentUpdateData, inputData);

            transform.position = currentUpdateData.position; // ! temp

            // state

            // messaging
            client.ReceivePlayerUpdate(currentUpdateData);

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
            if (!inputBuffer.IsFull) // todo add functionality for overwriting existing, but outdated entries
            {
                inputBuffer.Enqueue(data);
            }
        }
    }
}
