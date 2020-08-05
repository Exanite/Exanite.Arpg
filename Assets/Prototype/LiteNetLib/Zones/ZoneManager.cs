using System;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype.LiteNetLib.Zones
{
    public class ZoneManager : MonoBehaviour
    {
        public Dictionary<Guid, Zone> zones = new Dictionary<Guid, Zone>();

        // ? Packet handlers here
    }
}
