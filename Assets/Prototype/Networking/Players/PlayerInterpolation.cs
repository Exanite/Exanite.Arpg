using Prototype.Networking.Players.Data;
using Prototype.Networking.Zones;
using UnityEngine;
using Zenject;

namespace Prototype.Networking.Players
{
    public class PlayerInterpolation : MonoBehaviour
    {
        public PlayerUpdateData current;
        public PlayerUpdateData previous;
        public int lastUpdateTick;

        private Zone zone;

        [Inject]
        public void Inject(Zone zone)
        {
            this.zone = zone;
        }

        private void Update()
        {
            int ticksSinceLastUpdate = zone.Tick - lastUpdateTick;

            float timeSinceLastUpdate = ticksSinceLastUpdate * zone.TimePerTick + zone.TimeSinceLastTick;
            float t = timeSinceLastUpdate / zone.TimePerTick;

            transform.position = Vector3.LerpUnclamped(previous.playerPosition, previous.playerPosition, t);
        }

        public void UpdateData(PlayerUpdateData newData, bool snapInterpolation = false)
        {
            previous = snapInterpolation ? newData : current;
            current = newData;

            lastUpdateTick = zone.Tick;
        }
    }
}
