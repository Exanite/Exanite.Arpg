using System.Collections.Generic;
using Exanite.Arpg.Pathfinding.Graphs;
using UniRx.Async;
using UnityEngine;

namespace Exanite.Arpg.Pathfinding
{
    public class Pathfinder
    {
        /// <summary>
        /// Stores the parents of Nodes: the preceding node in the path
        /// </summary>
        private readonly Dictionary<Node, Node> parent = new Dictionary<Node, Node>();
        /// <summary>
        /// Stores the fCost of Nodes: the total cost of the node<para/>
        /// This is equal to gCost + heuristic(current, end)
        /// </summary>
        private readonly Dictionary<Node, float> fCost = new Dictionary<Node, float>();
        /// <summary>
        /// Stores the gCost of Nodes: the accurate, accumulated cost to the node
        /// </summary>
        private readonly Dictionary<Node, float> gCost = new Dictionary<Node, float>();

        private readonly List<Node> open = new List<Node>();
        private readonly HashSet<Node> closed = new HashSet<Node>();

        private object syncRoot = new object();

        /// <summary>
        /// Attempts to find a path between the start and destination nodes
        /// </summary>
        public async UniTask<(bool isSuccess, Path path)> FindPathAsync(NavGrid grid, Node start, Node destination, Heuristic heuristic = null)
        {
            await UniTask.SwitchToThreadPool();

            (bool isSuccess, Path path) result;

            lock (syncRoot)
            {
                result = FindPath(grid, start, destination, heuristic);
            }

            await UniTask.SwitchToMainThread();

            return result;
        }

        /// <summary>
        /// Attempts to find a path between the start and destination nodes
        /// </summary>
        public (bool isSuccess, Path path) FindPath(NavGrid grid, Node start, Node destination, Heuristic heuristic = null)
        {
            if (heuristic == null)
            {
                heuristic = Heuristics.Default;
            }

            if (start == null
                || destination == null
                || destination.Type == NodeType.NonWalkable)
            {
                return (false, null);
            }

            CleanupPathfindingData();

            open.Add(start);
            fCost[start] = 0;
            gCost[start] = 0;

            Node current;
            bool success = false;
            int lineOfSightCheckCounter = 0;
            int openNodeCounter = 1;

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

                open.Remove(current);
                closed.Add(current);

                // Lazy Theta* implementation, comment out Theta* to use
                //if (parent.ContainsKey(current) && parent.ContainsKey(parent[current]))
                //{
                //    lineOfSightCheckCounter++;

                //    if (grid.IsDirectlyWalkable(parent[parent[current]], current))
                //    {
                //        float newGCost = gCost[parent[parent[current]]] + heuristic(parent[parent[current]], current);

                //        if (newGCost < gCost[current])
                //        {
                //            gCost[current] = newGCost;
                //            parent[current] = parent[parent[current]];

                //            openNodeCounter++;
                //        }
                //    }
                //}

                if (current == destination)
                {
                    success = true;

                    break;
                }

                foreach (var neighbor in current.GetConnectedNodes())
                {
                    if (neighbor.Type == NodeType.NonWalkable || closed.Contains(neighbor))
                    {
                        continue;
                    }

                    if (!open.Contains(neighbor))
                    {
                        openNodeCounter++;

                        open.Add(neighbor);

                        gCost[neighbor] = float.PositiveInfinity;
                        parent[neighbor] = current;
                    }

                    // Theta* implementation
                    // Setting this if statement to false disables Theta* and changes the algorithm back to A*
                    if (parent.ContainsKey(current) && lineOfSightCheckCounter++ != 0 && grid.IsDirectlyWalkable(parent[current], neighbor))
                    {
                        float newGCost = gCost[parent[current]] + heuristic(parent[current], neighbor);

                        if (newGCost < gCost[neighbor])
                        {
                            gCost[neighbor] = newGCost;
                            parent[neighbor] = parent[current];
                        }
                    }
                    else
                    {
                        float newGCost = gCost[current] + heuristic(current, neighbor);

                        if (newGCost < gCost[neighbor])
                        {
                            gCost[neighbor] = newGCost;
                            parent[neighbor] = current;
                        }
                    }

                    fCost[neighbor] = gCost[neighbor] + heuristic(neighbor, destination);
                }
            }

            Debug.Log($"Finished pathfinding. Opened {openNodeCounter} nodes, closed {closed.Count} nodes, and performed {lineOfSightCheckCounter} line of sight checks");

            Path path = null;

            if (success)
            {
                List<Node> nodes = RetracePath(start, destination);

                path = new Path(nodes);
            }

            CleanupPathfindingData();

            return (success, path);
        }

        /// <summary>
        /// Retraces the path from <paramref name="destination"/> to <paramref name="start"/>
        /// </summary>
        private List<Node> RetracePath(Node start, Node destination)
        {
            var result = new List<Node>();
            var current = destination;

            while (current != start)
            {
                result.Add(current);

                current = parent[current];
            }

            result.Reverse();

            return result;
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
    }
}
