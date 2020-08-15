using Prototype.Networking.Players;
using UnityEngine;

namespace Prototype.Networking.Zones
{
    /// <summary>
    /// Base class used by <see cref="ClientZoneManager"/> and <see cref="ServerZoneManager"/>
    /// </summary>
    public abstract class ZoneManager : MonoBehaviour
    {
        /// <summary>
        /// Returns the zone the <see cref="Player"/> is currently in
        /// </summary>
        public abstract Zone GetPlayerCurrentZone(Player player);

        /// <summary>
        /// Returns if the <see cref="Player"/> is currently loading a zone
        /// </summary>
        public abstract bool IsPlayerLoading(Player player);
    }
}
