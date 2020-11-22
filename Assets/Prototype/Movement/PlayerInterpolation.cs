using Prototype.Networking.Players.Data;
using UnityEngine;

namespace Prototype.Movement
{
    public class PlayerInterpolation
    {
        public Transform player;
        public float positionSnapThreshold;

        public PlayerStateData current;
        public PlayerStateData previous;
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

            float timeSinceLastUpdate = (ticksSinceLastUpdate * timePerTick) + timeSinceLastTick;
            float t = timeSinceLastUpdate / timePerTick;

            player.position = Vector3.LerpUnclamped(previous.position, current.position, t);
        }

        public void UpdateData(PlayerStateData newData, uint tick)
        {
            if ((newData.position - current.position).sqrMagnitude > positionSnapThreshold * positionSnapThreshold)
            {
                current.position = newData.position;
            }

            previous = current;
            current = newData;

            lastUpdateTick = tick;
        }
    }
}
