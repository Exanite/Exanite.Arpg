using System;
using System.Collections.Generic;
using UnityEngine;

namespace Exanite.Arpg.Pathfinding
{
    public class Path
    {
        private List<Vector3> waypoints;

        public Path(List<Vector3> waypoints)
        {
            Waypoints = waypoints ?? throw new ArgumentNullException(nameof(waypoints));
        }

        public List<Vector3> Waypoints
        {
            get
            {
                return waypoints;
            }

            set
            {
                waypoints = value;
            }
        }
    }
}
