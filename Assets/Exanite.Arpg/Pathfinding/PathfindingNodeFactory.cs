using System.Collections.Generic;
using Exanite.Arpg.Pathfinding.Graphs;

namespace Exanite.Arpg.Pathfinding
{
    public class PathfindingNodeFactory
    {
        private readonly Dictionary<Node, PathfindingNode> activeInstances = new Dictionary<Node, PathfindingNode>();

        private static readonly Stack<PathfindingNode> instancePool = new Stack<PathfindingNode>();

        public PathfindingNode GetFor(Node node)
        {
            if (activeInstances.ContainsKey(node))
            {
                return GetExistingFor(node);
            }
            else
            {
                return GetNewFor(node);
            }
        }

        private PathfindingNode GetExistingFor(Node node)
        {
            return activeInstances[node];
        }

        private PathfindingNode GetNewFor(Node node)
        {
            PathfindingNode pathfindingNode;

            if (instancePool.Count > 0)
            {
                pathfindingNode = instancePool.Pop();
            }
            else
            {
                pathfindingNode = new PathfindingNode();
            }

            pathfindingNode.Node = node;

            activeInstances.Add(node, pathfindingNode);

            return pathfindingNode;
        }

        public void Release(PathfindingNode pathfindingNode)
        {
            activeInstances.Remove(pathfindingNode.Node);

            pathfindingNode.Reset();

            instancePool.Push(pathfindingNode);
        }
    }
}
