using Exanite.Arpg.Collections;
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

        public void Reconciliate(ref PlayerStateData currentData, PlayerStateData newData, uint tick)
        {
            if (tick < lastTickFromServer)
            {
                return;
            }

            lastTickFromServer = tick;

            while (reconciliationBuffer.Count > 0 && reconciliationBuffer.Peek().tick < tick)
            {
                reconciliationBuffer.Dequeue();
            }

            if (reconciliationBuffer.TryPeek(out PlayerReconciliationData reconciliationData) && reconciliationData.tick == tick)
            {
                reconciliationBuffer.Dequeue();

                if (Vector3.Distance(reconciliationData.stateData.position, newData.position) > reconciliationPositionThreshold)
                {
                    currentData = newData;

                    for (int i = 0; i < reconciliationBuffer.Count; i++)
                    {
                        currentData = logic.Simulate(currentData, reconciliationBuffer[i].inputData);
                    }
                }
            }
        }

        public void AddFrame(uint tick, PlayerStateData stateData, PlayerInputData inputData)
        {
            var reconciliationData = new PlayerReconciliationData(tick, stateData, inputData);

            if (!reconciliationBuffer.IsFull) // todo add functionality for overwriting existing, but outdated entries
            {
                reconciliationBuffer.Enqueue(reconciliationData);
            }
        }
    }
}
