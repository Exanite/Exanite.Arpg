using UnityEngine;

namespace Prototype.Movement
{
    public class PlayerInterpolation
    {
        public Transform player;
        public float positionSnapThreshold;

        public PlayerStateData current;
        public PlayerStateData previous;
        public uint lastTick;

        public PlayerInterpolation(Transform player, float positionSnapThreshold = 2f)
        {
            this.player = player;
            this.positionSnapThreshold = positionSnapThreshold;
        }

        public void Update(ZoneTime time)
        {
            uint ticksSinceLastUpdate = time.CurrentTick - lastTick;
            float timeSinceLastUpdate = (ticksSinceLastUpdate * time.TimePerTick) + time.TimeSinceLastTick;

            float t = timeSinceLastUpdate / time.TimePerTick;

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

            lastTick = tick;
        }
    }
}
