using Prototype.Networking.Players.Data;
using Prototype.Networking.Zones;
using UnityEngine;
using Zenject;

namespace Prototype.Networking.Players
{
    public class PlayerInterpolationBehaviour : MonoBehaviour
    {
        public PlayerUpdateData current;
        public PlayerUpdateData previous;
        public int lastUpdateTick;

        private Player player;
        private Zone zone;

        [Inject]
        public void Inject(Player player, Zone zone)
        {
            this.player = player;
            this.zone = zone;
        }

        private void Update()
        {
            int ticksSinceLastUpdate = zone.Tick - lastUpdateTick;

            float timeSinceLastUpdate = ticksSinceLastUpdate * zone.TimePerTick + zone.TimeSinceLastTick;
            float t = timeSinceLastUpdate / zone.TimePerTick;

            transform.position = Vector3.LerpUnclamped(previous.playerPosition, previous.playerPosition, t);
        }

        public void UpdateData(PlayerUpdateData newData)
        {
            previous = current;
            current = newData;

            lastUpdateTick = zone.Tick;
        }
    } 
}
