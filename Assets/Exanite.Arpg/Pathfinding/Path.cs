using System.Collections.Generic;
using Exanite.Arpg.Pathfinding.Graphs;
using UnityEngine;

namespace Exanite.Arpg.Pathfinding
{
    public class Path
    {
        private readonly List<Node> nodes = new List<Node>();

        private readonly Dictionary<Node, Node> parent = new Dictionary<Node, Node>();
        private readonly Dictionary<Node, float> fCost = new Dictionary<Node, float>();
        private readonly Dictionary<Node, float> gCost = new Dictionary<Node, float>();

        private readonly List<Node> open = new List<Node>();
        private readonly HashSet<Node> closed = new HashSet<Node>();

        /// <summary>
        /// List of nodes in the path
        /// </summary>
        public List<Node> Nodes
        {
            get
            {
                return nodes;
            }
        }

        /// <summary>
        /// Attempts to find a path between the start and destination nodes
        /// </summary>
        public bool Find(Node start, Node destination, Heuristic heuristic = null)
        {
            if (heuristic == null)
            {
                heuristic = DefaultHeuristic;
            }

            if (destination.Type == NodeType.NonWalkable)
            {
                Nodes.Clear();

                return false;
            }

            CleanupPathfindingData();

            open.Add(start);
            fCost[start] = 0;
            gCost[start] = 0;

            Node current;

            while (open.Count > 0)
            {
                current = open[0];

                for (int i = 1; i < open.Count; i++)
                {
                    if (fCost[open[i]] < fCost[current])
                    {
                        current = open[i];
                    }
                }

                open.RemoveAt(0);
                closed.Add(current);

                if (current == destination)
                {
                    RetraceFrom(start, destination);

                    CleanupPathfindingData();

                    return true;
                }

                foreach (var neighbor in current.GetConnectedNodes())
                {
                    if (neighbor.Type == NodeType.NonWalkable || closed.Contains(neighbor))
                    {
                        continue;
                    }

                    float newGCost = gCost[current] + heuristic(current, neighbor);

                    if (!gCost.ContainsKey(neighbor) || newGCost < gCost[neighbor])
                    {
                        gCost[neighbor] = newGCost;
                        parent[neighbor] = current;

                        if (!open.Contains(neighbor) && !closed.Contains(neighbor))
                        {
                            fCost[neighbor] = newGCost + heuristic(neighbor, destination);

                            open.Add(neighbor);
                        }
                    }
                }
            }

            Nodes.Clear();

            CleanupPathfindingData();

            return false;
        }

        /// <summary>
        /// Retraces the path from <paramref name="destination"/> to <paramref name="start"/> <para/>
        /// </summary>
        private void RetraceFrom(Node start, Node destination)
        {
            Nodes.Clear();

            var current = destination;

            while (current != start)
            {
                Nodes.Add(current);

                current = parent[current];
            }

            Nodes.Reverse();
        }

        /// <summary>
        /// Cleans up pathfinding data for the next use
        /// </summary>
        private void CleanupPathfindingData()
        {
            parent.Clear();
            fCost.Clear();
            gCost.Clear();

            open.Clear();
            closed.Clear();
        }

        /// <summary>
        /// Calculates the distance between <see cref="Node"/> a and <see cref="Node"/> b<para/>
        /// By default the Heuristic returns the Euclidean distance between the two nodes
        /// </summary>
        public static float DefaultHeuristic(Node a, Node b)
        {
            return Vector3.Distance(a.Position, b.Position);
        }
    }
}
