using Exanite.Arpg.Collections;
using Prototype.Networking.Players.Data;
using UnityEngine;

namespace Prototype.Movement
{
    public class ClientPlayerCharacter : PlayerCharacter
    {
        public ServerPlayerCharacter server;

        private RingBuffer<Frame<PlayerUpdateData>> updateFrameBuffer;

        private PlayerInput input;
        private PlayerLogic logic;
        private PlayerInterpolation interpolation;
        private PlayerReconciliation reconciliation;

        private void Start()
        {
            updateFrameBuffer = new RingBuffer<Frame<PlayerUpdateData>>(64);

            input = new PlayerInput();
            logic = new PlayerLogic(mapSize);
            interpolation = new PlayerInterpolation(transform);
            reconciliation = new PlayerReconciliation(logic);
        }

        private void Update()
        {
            interpolation.Update(tick);
        }

        private void FixedUpdate()
        {
            // input
            var inputData = input.Get();

            // simulation
            currentUpdateData = logic.Simulate(currentUpdateData, inputData);

            // state
            if (updateFrameBuffer.TryDequeue(out var updateFrame))
            {
                reconciliation.Reconciliate(ref currentUpdateData, updateFrame.data, updateFrame.tick);
            }

            // view
            interpolation.UpdateData(currentUpdateData, tick);

            // messaging
            server.OnReceivePlayerInput(tick, inputData);
            reconciliation.AddFrame(tick, currentUpdateData, inputData);

            tick++;
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
            {
                GUILayout.Label("--Client--");
                GUILayout.Label($"Tick: {tick}");
                GUILayout.Label($"UpdateBuffer.Count: {updateFrameBuffer.Count}");
            }
            GUILayout.EndArea();
        }

        public void OnReceivePlayerUpdate(uint tick, PlayerUpdateData data)
        {
            if (updateFrameBuffer.IsFull)
            {
                updateFrameBuffer.Dequeue();
            }

            updateFrameBuffer.Enqueue(new Frame<PlayerUpdateData>(tick, data));
        }
    }
}
