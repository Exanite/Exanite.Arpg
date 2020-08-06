using Exanite.Arpg.Networking;

namespace Prototype.Networking
{
    public interface IPacketHandler
    {
        void RegisterPackets(UnityNetBase netBase);

        void UnregisterPackets(UnityNetBase netBase);
    }
}
