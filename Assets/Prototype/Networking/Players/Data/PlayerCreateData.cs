using LiteNetLib.Utils;

namespace Prototype.Networking.Players.Data
{
    public struct PlayerCreateData : INetSerializable
    {
        private int playerId;

        private PlayerUpdateData updateData;

        public PlayerCreateData(int playerId, PlayerUpdateData updateData)
        {
            this.playerId = playerId;
            this.updateData = updateData;
        }

        public int PlayerId
        {
            get
            {
                return playerId;
            }

            set
            {
                playerId = value;
            }
        }

        public PlayerUpdateData UpdateData
        {
            get
            {
                return updateData;
            }

            set
            {
                updateData = value;
            }
        }

        public void Deserialize(NetDataReader reader)
        {
            PlayerId = reader.GetInt();

            UpdateData.Deserialize(reader);
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(PlayerId);

            UpdateData.Serialize(writer);
        }
    }
}
