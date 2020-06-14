//using System;
//using System.Collections.Generic;

//namespace Exanite.Arpg.Pathfinding
//{
//    /// <summary>
//    /// Represents a <see cref="NavGrid"/> <see cref="Node"/> for the purposes of pathfinding
//    /// </summary>
//    public class PathfindingNode : IComparable<PathfindingNode>
//    {
//        private Node node;
//        private PathfindingNode parent;

//        private float gCost;
//        private float hCost;

//        private static readonly Stack<PathfindingNode> inactiveInstances = new Stack<PathfindingNode>();
//        private static readonly Dictionary<Node, PathfindingNode> activeInstances = new Dictionary<Node, PathfindingNode>();

//        /// <summary>
//        /// The <see cref="Node"/> this <see cref="PathfindingNode"/> is representing
//        /// </summary>
//        public Node Node
//        {
//            get
//            {
//                return node;
//            }

//            set
//            {
//                node = value;
//            }
//        }

//        /// <summary>
//        /// Parent of this node. The node that led to this node being opened
//        /// </summary>
//        public PathfindingNode Parent
//        {
//            get
//            {
//                return parent;
//            }

//            set
//            {
//                parent = value;
//            }
//        }

//        /// <summary>
//        /// The sum of GCost and HCost
//        /// </summary>
//        public float FCost
//        {
//            get
//            {
//                return GCost + HCost;
//            }
//        }

//        /// <summary>
//        /// The accurate distance from the start to this node
//        /// </summary>
//        public float GCost
//        {
//            get
//            {
//                return gCost;
//            }

//            set
//            {
//                gCost = value;
//            }
//        }

//        /// <summary>
//        /// The estimated distance from this node to the destination
//        /// </summary>
//        public float HCost
//        {
//            get
//            {
//                return hCost;
//            }

//            set
//            {
//                hCost = value;
//            }
//        }

//        private PathfindingNode() { }

//        public void Reset()
//        {
//            Node = null;
//            Parent = null;
//            GCost = 0;
//            HCost = 0;
//        }

//        public int CompareTo(PathfindingNode other)
//        {
//            return FCost.CompareTo(other.FCost);
//        }

//        public static PathfindingNode GetFor(Node node)
//        {
//            if (activeInstances.ContainsKey(node))
//            {
//                return GetExistingFor(node);
//            }
//            else
//            {
//                return GetNewFor(node);
//            }
//        }

//        private static PathfindingNode GetExistingFor(Node node)
//        {
//            return activeInstances[node];
//        }

//        private static PathfindingNode GetNewFor(Node node)
//        {
//            PathfindingNode pathfindingNode;

//            if (inactiveInstances.Count > 0)
//            {
//                pathfindingNode = inactiveInstances.Pop();
//            }
//            else
//            {
//                pathfindingNode = new PathfindingNode();
//            }

//            pathfindingNode.Node = node;

//            activeInstances.Add(node, pathfindingNode);

//            return pathfindingNode;
//        }

//        public static void Release(PathfindingNode pathfindingNode)
//        {
//            activeInstances.Remove(pathfindingNode.Node);

//            pathfindingNode.Reset();

//            inactiveInstances.Push(pathfindingNode);
//        }
//    }
//}
