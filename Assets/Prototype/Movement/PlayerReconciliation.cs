using Prototype.Networking.Players.Data;
using UnityEngine;

namespace Prototype.Movement
{
    public class PlayerReconciliation
    {
        public PlayerLogic logic;
        public float reconciliationPositionThreshold;

        private RingBuffer<PlayerReconciliationData> reconciliationBuffer = new RingBuffer<PlayerReconciliationData>(64);
        private uint lastTickFromServer;

        public PlayerReconciliation(PlayerLogic logic, float reconciliationPositionThreshold = 0.1f)
        {
            this.logic = logic;
            this.reconciliationPositionThreshold = reconciliationPositionThreshold;
        }

        public void Reconciliate(ref PlayerUpdateData currentData, PlayerUpdateData newData, uint tick)
        {
            if (tick < lastTickFromServer)
            {
                return;
            }

            lastTickFromServer = tick;

            while (reconciliationBuffer.Count > 0 && reconciliationBuffer.Peek().tick < lastTickFromServer)
            {
                reconciliationBuffer.Dequeue();
            }

            if (reconciliationBuffer.Count > 0 && reconciliationBuffer.Peek().tick == lastTickFromServer)
            {
                PlayerReconciliationData reconciliationData = reconciliationBuffer.Dequeue();

                if (Vector3.Distance(reconciliationData.updateData.playerPosition, newData.playerPosition) > reconciliationPositionThreshold)
                {
                    for (int i = 0; i < reconciliationBuffer.Count; i++)
                    {
                        currentData = logic.Simulate(currentData, reconciliationBuffer[i].inputData);
                    }
                }
            }
        }

        public void AddFrame(uint tick, PlayerUpdateData updateData, PlayerInputData inputData)
        {
            var reconciliationData = new PlayerReconciliationData(tick, updateData, inputData);

            if (!reconciliationBuffer.IsFull) // todo add functionality for overwriting existing, but outdated entries
            {
                reconciliationBuffer.Enqueue(reconciliationData);
            }
        }
    }
}
