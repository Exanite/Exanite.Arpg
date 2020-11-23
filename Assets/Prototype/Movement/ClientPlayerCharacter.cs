using Exanite.Arpg.Collections;
using UnityEngine;

namespace Prototype.Movement
{
    public class ClientPlayerCharacter : PlayerCharacter
    {
        public ServerPlayerCharacter server;

        private RingBuffer<Frame<PlayerStateData>> stateFrameBuffer;

        private PlayerInput input;
        private PlayerLogic logic;
        private PlayerReconciliation reconciliation;

        private void Start()
        {
            stateFrameBuffer = new RingBuffer<Frame<PlayerStateData>>(64);

            input = new PlayerInput();
            logic = new PlayerLogic(mapSize);
            reconciliation = new PlayerReconciliation(logic);
        }

        protected override void OnTick()
        {
            // input
            var inputData = input.Get();

            // simulation
            currentStateData = logic.Simulate(currentStateData, inputData);

            // state
            Frame<PlayerStateData> stateFrame = default;
            bool hasValue = false;

            while (stateFrameBuffer.Count > 0 && stateFrameBuffer.Peek().tick < Time.CurrentTick)
            {
                stateFrame = stateFrameBuffer.Dequeue();
                hasValue = true;
            }

            if (hasValue)
            {
                reconciliation.Reconciliate(ref currentStateData, stateFrame.data, stateFrame.tick + 1); // ! shouldn't need to '+ 1' after tick sync
            }

            ApplyState(currentStateData);
            OnStateUpdated();

            // messaging
            server.OnReceivePlayerInput(Time.CurrentTick + 1, inputData); // ! shouldn't need to '+ 1' after tick sync
            reconciliation.AddFrame(Time.CurrentTick, currentStateData, inputData);
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
            {
                GUILayout.Label("--Client--");
                GUILayout.Label($"Tick: {Time.CurrentTick}");
                GUILayout.Label($"UpdateBuffer.Count: {stateFrameBuffer.Count}");
            }
            GUILayout.EndArea();
        }

        public void OnReceivePlayerState(uint tick, PlayerStateData data)
        {
            if (stateFrameBuffer.IsFull)
            {
                stateFrameBuffer.Dequeue();
            }

            stateFrameBuffer.Enqueue(new Frame<PlayerStateData>(tick, data));
        }
    }
}
