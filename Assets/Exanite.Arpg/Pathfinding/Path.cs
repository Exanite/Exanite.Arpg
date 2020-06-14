using System.Collections.Generic;
using Exanite.Arpg.Pathfinding.Graphs;
using UnityEngine;

namespace Exanite.Arpg.Pathfinding
{
    public class Path
    {
        private List<Node> nodes = new List<Node>();

        public List<Node> Nodes
        {
            get
            {
                return nodes;
            }
        }

        public static bool FindPath(NavGrid graph, Node start, Node destination, Path path, Heuristic heuristic = null)
        {
            if (heuristic == null)
            {
                heuristic = DefaultHeuristic;
            }

            if (destination.Type == NodeType.NonWalkable)
            {
                path.Nodes.Clear();

                return false;
            }

            List<Node> open = new List<Node>();
            HashSet<Node> closed = new HashSet<Node>();
            Node current;

            open.Add(start);

            while (open.Count > 0)
            {
                current = open[0];

                for (int i = 1; i < open.Count; i++)
                {
                    if (open[i].FCost.CompareTo(current.FCost) < 0)
                    {
                        current = open[i];
                    }
                }

                open.Remove(current);
                closed.Add(current);

                if (current == destination)
                {
                    RetraceFrom(start, destination, path);

                    return true;
                }

                foreach (var neighbor in current.GetWalkableConnectedNodes())
                {
                    if (closed.Contains(neighbor))
                    {
                        continue;
                    }

                    float newGCost = current.GCost + heuristic(current, neighbor);

                    if (newGCost < neighbor.GCost || !open.Contains(neighbor))
                    {
                        neighbor.GCost = newGCost;
                        neighbor.HCost = heuristic(neighbor, destination);
                        neighbor.Parent = current;

                        if (!open.Contains(neighbor) && !closed.Contains(neighbor))
                        {
                            open.Add(neighbor);
                        }
                    }
                }
            }

            path.Nodes.Clear();

            return false;
        }

        /// <summary>
        /// Retraces the path from <paramref name="destination"/> to <paramref name="start"/> <para/>
        /// </summary>
        private static Path RetraceFrom(Node start, Node destination, Path path)
        {
            path.Nodes.Clear();

            var current = destination;

            while (current != start)
            {
                path.Nodes.Add(current);

                current = current.Parent;
            }

            path.Nodes.Add(start);

            path.Nodes.Reverse();

            return path;
        }

        public static float DefaultHeuristic(Node a, Node b)
        {
            return Vector3.Distance(a.Position, b.Position);
        }
    }
}
