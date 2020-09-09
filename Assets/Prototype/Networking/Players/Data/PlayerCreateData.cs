using LiteNetLib.Utils;

namespace Prototype.Networking.Players.Data
{
    public struct PlayerCreateData : INetSerializable
    {
        public int playerId;

        public PlayerUpdateData updateData;

        public void Deserialize(NetDataReader reader)
        {
            playerId = reader.GetInt();

            updateData.Deserialize(reader);
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(playerId);

            updateData.Serialize(writer);
        }
    }
}
