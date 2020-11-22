using Exanite.Arpg.Collections;
using Prototype.Networking.Players.Data;
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

        private void FixedUpdate()
        {
            // input
            var inputData = input.Get();

            // simulation
            currentStateData = logic.Simulate(currentStateData, inputData);

            // state
            if (stateFrameBuffer.TryDequeue(out var stateFrame))
            {
                reconciliation.Reconciliate(ref currentStateData, stateFrame.data, stateFrame.tick);
            }

            OnStateUpdated();

            // messaging
            server.OnReceivePlayerInput(tick, inputData);
            reconciliation.AddFrame(tick, currentStateData, inputData);

            tick++;
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
            {
                GUILayout.Label("--Client--");
                GUILayout.Label($"Tick: {tick}");
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
