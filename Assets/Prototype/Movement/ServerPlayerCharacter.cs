using Exanite.Arpg.Collections;
using Prototype.Networking.Players.Data;
using UnityEngine;

namespace Prototype.Movement
{
    public class ServerPlayerCharacter : PlayerCharacter
    {
        public ClientPlayerCharacter client;

        private RingBuffer<Frame<PlayerInputData>> inputFrameBuffer;

        private PlayerLogic logic;

        private void Start()
        {
            inputFrameBuffer = new RingBuffer<Frame<PlayerInputData>>(64);

            logic = new PlayerLogic(mapSize);
        }

        private void FixedUpdate()
        {
            // input
            inputFrameBuffer.TryDequeue(out var inputFrame);

            // simulation
            currentUpdateData = logic.Simulate(currentUpdateData, inputFrame.data);

            transform.position = currentUpdateData.position; // ! temp

            // state

            // messaging
            client.OnReceivePlayerUpdate(tick, currentUpdateData);

            tick++;
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
            {
                GUILayout.FlexibleSpace();

                GUILayout.Label("--Server--");
                GUILayout.Label($"Tick: {tick}");
                GUILayout.Label($"InputBuffer.Count: {inputFrameBuffer.Count}");
            }
            GUILayout.EndArea();
        }

        public void OnReceivePlayerInput(uint tick, PlayerInputData data)
        {
            if (inputFrameBuffer.IsFull)
            {
                inputFrameBuffer.Dequeue();
            }

            inputFrameBuffer.Enqueue(new Frame<PlayerInputData>(tick, data));
        }
    }
}
