﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype.Networking.Zones
{
    public class ServerZoneManager : MonoBehaviour
    {
        public Dictionary<Guid, Zone> zones = new Dictionary<Guid, Zone>();

        // ? Packet handlers here
    }
}