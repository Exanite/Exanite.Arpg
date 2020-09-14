using Prototype.Networking.Players.Data;
using UnityEngine;

namespace Prototype.Movement
{
    public class ClientPlayerCharacter : MonoBehaviour
    {
        public ServerPlayerCharacter server;
        public uint tick;
        public float mapSize = 10;

        private PlayerUpdateData currentUpdateData;

        private RingBuffer<PlayerUpdateData> updateBuffer;

        private PlayerInput input;
        private PlayerLogic logic;
        private PlayerInterpolation interpolation;
        private PlayerReconciliation reconciliation;

        private void Start()
        {
            updateBuffer = new RingBuffer<PlayerUpdateData>(64);

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

            // state
            if (updateBuffer.TryDequeue(out PlayerUpdateData updateData))
            {
                reconciliation.Reconciliate(ref currentUpdateData, updateData, tick - 1); // todo use tick from server
            }

            // simulation
            currentUpdateData = logic.Simulate(currentUpdateData, inputData);

            // view
            interpolation.UpdateData(currentUpdateData, tick);

            // messaging
            server.ReceivePlayerInput(inputData);
            reconciliation.AddFrame(tick, currentUpdateData, inputData);

            tick++;
        }

        public void ReceivePlayerUpdate(PlayerUpdateData data)
        {
            if (!updateBuffer.IsFull) // todo add functionality for overwriting existing, but outdated entries
            {
                updateBuffer.Enqueue(data);
            }
        }
    }
}
