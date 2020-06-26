using System.Collections.Generic;
using System.Linq;
using Exanite.Arpg.Pathfinding.Graphs;
using UniRx.Async;
using UnityEngine;

namespace Exanite.Arpg.Pathfinding
{
    public class Pathfinder
    {
        private readonly Dictionary<Node, Node> parent = new Dictionary<Node, Node>();
        private readonly Dictionary<Node, float> fCost = new Dictionary<Node, float>();
        private readonly Dictionary<Node, float> gCost = new Dictionary<Node, float>();

        private readonly List<Node> open = new List<Node>();
        private readonly HashSet<Node> closed = new HashSet<Node>();

        private object syncRoot = new object();

        /// <summary>
        /// Attempts to find a path between the start and destination nodes
        /// </summary>
        public async UniTask<(bool isSuccess, Path path)> FindPathAsync(Node start, Node destination, Heuristic heuristic = null)
        {
            await UniTask.SwitchToThreadPool();

            (bool isSuccess, Path path) result;

            lock (syncRoot)
            {
                result = FindPath(start, destination, heuristic);
            }

            await UniTask.SwitchToMainThread();

            return result;
        }

        /// <summary>
        /// Attempts to find a path between the start and destination nodes
        /// </summary>
        public (bool isSuccess, Path path) FindPath(Node start, Node destination, Heuristic heuristic = null)
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

            Path path = null;

            if (success)
            {
                List<Node> nodes = SimplifyPath(RetracePath(start, destination));

                path = new Path(nodes.Select(x => x.Position).ToList()); // ! hack to get things working again
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
        /// Removes unneeded, same directional, waypoints from the path
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        private List<Node> SimplifyPath(List<Node> nodes)
        {
            if (nodes.Count <= 2)
            {
                return new List<Node>(nodes);
            }

            var result = new List<Node>();

            Vector3 currentDirection = Vector3.zero;
            Vector3 newDirection;

            result.Add(nodes[0]);

            for (int i = 1; i < nodes.Count; i++)
            {
                newDirection = nodes[i].Position - nodes[i - 1].Position;

                if (Vector3.Dot(currentDirection, newDirection) != 1)
                {
                    result.Add(nodes[i - 1]);

                    currentDirection = newDirection;
                }
            }

            result.Add(nodes[nodes.Count - 1]);

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
