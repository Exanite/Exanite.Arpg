namespace Exanite.Arpg.Networking
{
    public interface IPacketHandler
    {
        void RegisterPackets(UnityNetwork network);

        void UnregisterPackets(UnityNetwork network);
    }
}
