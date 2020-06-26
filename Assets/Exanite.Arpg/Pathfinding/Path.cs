using System;
using System.Collections.Generic;
using Exanite.Arpg.Pathfinding.Graphs;
using UnityEngine;

namespace Exanite.Arpg.Pathfinding
{
    /// <summary>
    /// Represents a path defined by a collection of waypoints
    /// </summary>
    public class Path
    {
        private List<Vector3> waypoints;

        /// <summary>
        /// Creates a new <see cref="Path"/> using a List of waypoints
        /// </summary>
        public Path(List<Vector3> waypoints)
        {
            Waypoints = waypoints ?? throw new ArgumentNullException(nameof(waypoints));
        }

        /// <summary>
        /// Creates a new <see cref="Path"/> using a List of <see cref="Node"/>s
        /// </summary>
        public Path(List<Node> nodes)
        {
            waypoints = new List<Vector3>(nodes.Count);

            foreach (var node in nodes)
            {
                waypoints.Add(node.Position);
            }
        }

        /// <summary>
        /// Waypoints contained in this <see cref="Path"/>
        /// </summary>
        public List<Vector3> Waypoints
        {
            get
            {
                return waypoints;
            }

            private set
            {
                waypoints = value;
            }
        }
    }
}
