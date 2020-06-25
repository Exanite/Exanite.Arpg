using Exanite.Arpg.Pathfinding.Graphs;
using UnityEngine;

namespace Exanite.Arpg.Pathfinding
{
    public class Heuristics
    {
        /// <summary>
        /// Calculates the distance between two <see cref="Node"/>s
        /// By default the Heuristic returns the Euclidean distance between the two nodes
        /// </summary>
        public static float Default(Node a, Node b)
        {
            return Euclidean(a, b);
        }

        /// <summary>
        /// Returns the Euclidean Distance between two <see cref="Node"/>s
        /// </summary>
        public static float Euclidean(Node a, Node b)
        {
            return Vector3.Distance(a.Position, b.Position);
        }

        /// <summary>
        /// Returns the Manhattan Distance between two <see cref="Node"/>s
        /// </summary>
        public static float Manhattan(Node a, Node b)
        {
            float dx = Mathf.Abs(a.Position.x - b.Position.x);
            float dy = Mathf.Abs(a.Position.y - b.Position.y);
            float dz = Mathf.Abs(a.Position.z - b.Position.z);

            return dx + dy + dz;
        }
    }
}
