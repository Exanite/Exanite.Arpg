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

        protected override void OnTick()
        {
            // input
            Frame<PlayerInputData> inputFrame;
            while (inputFrameBuffer.TryDequeue(out inputFrame) && inputFrame.tick < Time.CurrentTick) { }

            // simulation
            currentStateData = logic.Simulate(currentStateData, inputFrame.data);

            // state
            ApplyState(currentStateData);
            OnStateUpdated();

            // messaging
            client.OnReceivePlayerState(Time.CurrentTick, currentStateData);
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
            {
                GUILayout.FlexibleSpace();

                GUILayout.Label("--Server--");
                GUILayout.Label($"Tick: {Time.CurrentTick}");
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
