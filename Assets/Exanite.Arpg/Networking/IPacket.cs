using LiteNetLib.Utils;

namespace Exanite.Arpg.Networking
{
    /// <summary>
    /// Represents a packet of data that can be serialized and sent over the network
    /// </summary>
    public interface IPacket : INetSerializable { }
}
