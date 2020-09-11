using Prototype.Networking.Players.Data;

namespace Prototype.Networking.Players
{
    public struct ReconciliationData
    {
        public uint tick;

        public PlayerInputData inputData;
        public PlayerUpdateData updateData;
    }
}
