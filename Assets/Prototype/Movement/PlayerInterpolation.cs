using Prototype.Networking.Players.Data;
using UnityEngine;

namespace Prototype.Movement
{
    public class PlayerInterpolation
    {
        public Transform player;
        public float positionSnapThreshold;

        public PlayerUpdateData current;
        public PlayerUpdateData previous;
        public uint lastUpdateTick;

        public PlayerInterpolation(Transform player, float positionSnapThreshold = 2f)
        {
            this.player = player;
            this.positionSnapThreshold = positionSnapThreshold;
        }

        public void Update(uint currentTick)
        {
            uint ticksSinceLastUpdate = currentTick - lastUpdateTick;
            float timePerTick = Time.fixedDeltaTime;
            float timeSinceLastTick = Time.time - Time.fixedTime;

            float timeSinceLastUpdate = ticksSinceLastUpdate * timePerTick + timeSinceLastTick;
            float t = timeSinceLastUpdate / timePerTick;

            player.position = Vector3.LerpUnclamped(previous.playerPosition, current.playerPosition, t);
        }

        public void UpdateData(PlayerUpdateData newData, uint tick)
        {
            if ((newData.playerPosition - current.playerPosition).sqrMagnitude > positionSnapThreshold * positionSnapThreshold)
            {
                current.playerPosition = newData.playerPosition;
            }

            previous = current;
            current = newData;

            lastUpdateTick = tick;
        }
    }
}
