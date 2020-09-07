using UnityEngine;
using Zenject;

namespace Prototype.Networking.Zones
{
    [DefaultExecutionOrder(-50)]
    public class ZoneTickBehaviour : MonoBehaviour
    {
        private Zone zone;

        [Inject]
        public void Inject(Zone zone)
        {
            this.zone = zone;
        }

        private void FixedUpdate()
        {
            zone.tick++;
        }
    }
}
