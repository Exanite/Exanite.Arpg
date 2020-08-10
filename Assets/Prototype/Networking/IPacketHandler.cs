using Exanite.Arpg.Networking;

namespace Prototype.Networking
{
    public interface IPacketHandler
    {
        void RegisterPackets(UnityNetwork network);

        void UnregisterPackets(UnityNetwork network);
    }
}
