using Prototype.Networking.Players.Data;

namespace Prototype.Networking.Players
{
    public struct ReconciliationData
    {
        public uint tick;

        public PlayerUpdateData updateData;
        public PlayerInputData inputData;

        public ReconciliationData(uint tick, PlayerUpdateData updateData, PlayerInputData inputData)
        {
            this.tick = tick;
            this.updateData = updateData;
            this.inputData = inputData;
        }
    }
}
