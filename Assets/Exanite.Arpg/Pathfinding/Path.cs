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

        public void DrawWithGizmos(NavGrid grid)
        {
            Gizmos.color = Color.red;

            for (int i = 1; i < Waypoints.Count; i++)
            {
                Gizmos.DrawLine(Waypoints[i] + grid.NodeDrawHeightOffset, Waypoints[i - 1] + grid.NodeDrawHeightOffset);
            }
        }

        public void DrawWithGizmos(NavGrid grid, Vector3 currentPosition)
        {
            if (Waypoints.Count == 0)
            {
                return;
            }

            Gizmos.color = Color.red;
            Gizmos.DrawLine(currentPosition + grid.NodeDrawHeightOffset, Waypoints[0]);
            DrawWithGizmos(grid);
        }

        public void DrawWithGL(Material material, NavGrid grid)
        {
            if (Waypoints.Count < 2)
            {
                return;
            }

            material.SetPass(0);

            GL.Begin(GL.LINE_STRIP);
            {
                GL.Color(Color.red);

                for (int i = 0; i < Waypoints.Count; i++)
                {
                    GL.Vertex(Waypoints[i] + grid.NodeDrawHeightOffset);
                }
            }
            GL.End();
        }

        public void DrawWithGL(Material material, NavGrid grid, Vector3 currentPosition)
        {
            if (Waypoints.Count == 0)
            {
                return;
            }

            material.SetPass(0);

            GL.Begin(GL.LINES);
            {
                GL.Color(Color.red);

                GL.Vertex(currentPosition + grid.NodeDrawHeightOffset);
                GL.Vertex(Waypoints[0] + grid.NodeDrawHeightOffset);
            }
            GL.End();

            DrawWithGL(material, grid);
        }
    }
}
