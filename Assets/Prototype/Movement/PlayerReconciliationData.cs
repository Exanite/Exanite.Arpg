using Prototype.Networking.Players.Data;

namespace Prototype.Movement
{
    public struct PlayerReconciliationData
    {
        public uint tick;

        public PlayerStateData stateData;
        public PlayerInputData inputData;

        public PlayerReconciliationData(uint tick, PlayerStateData stateData, PlayerInputData inputData)
        {
            this.tick = tick;
            this.stateData = stateData;
            this.inputData = inputData;
        }
    }
}
