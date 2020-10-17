using Prototype.Networking.Players.Data;

namespace Prototype.Movement
{
    public struct PlayerReconciliationData
    {
        public uint tick;

        public PlayerUpdateData updateData;
        public PlayerInputData inputData;

        public PlayerReconciliationData(uint tick, PlayerUpdateData updateData, PlayerInputData inputData)
        {
            this.tick = tick;
            this.updateData = updateData;
            this.inputData = inputData;
        }
    }
}
