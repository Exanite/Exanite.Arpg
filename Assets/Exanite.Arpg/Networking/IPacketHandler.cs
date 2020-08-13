namespace Exanite.Arpg.Networking
{
    /// <summary>
    /// Represents a packet handler that can receive packets from the network
    /// </summary>
    public interface IPacketHandler
    {
        /// <summary>
        /// Used to register packet receivers to a <see cref="UnityNetwork"/>
        /// </summary>
        /// <param name="network"><see cref="UnityNetwork"/> to register packet receivers to</param>
        void RegisterPackets(UnityNetwork network);

        /// <summary>
        /// Used to unregister packet receivers from a <see cref="UnityNetwork"/>
        /// </summary>
        /// <param name="network"><see cref="UnityNetwork"/> to unregister packet receivers from</param>
        void UnregisterPackets(UnityNetwork network);
    }
}
