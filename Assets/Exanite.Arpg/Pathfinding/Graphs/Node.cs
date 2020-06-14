using System;
using System.Collections.Generic;
using UnityEngine;

namespace Exanite.Arpg.Pathfinding.Graphs
{
    public class Node
    {
        private Vector3 position;
        private NodeType type = NodeType.Walkable;

        private HashSet<Node> connectedNodes = new HashSet<Node>();

        public Vector3 Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        public NodeType Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }

        // temp

        private Node parent;

        private float gCost;
        private float hCost;

        /// <summary>
        /// Parent of this node. The node that led to this node being opened
        /// </summary>
        public Node Parent
        {
            get
            {
                return parent;
            }

            set
            {
                parent = value;
            }
        }

        /// <summary>
        /// The sum of GCost and HCost
        /// </summary>
        public float FCost
        {
            get
            {
                return GCost + HCost;
            }
        }

        /// <summary>
        /// The accurate distance from the start to this node
        /// </summary>
        public float GCost
        {
            get
            {
                return gCost;
            }

            set
            {
                gCost = value;
            }
        }

        /// <summary>
        /// The estimated distance from this node to the destination
        /// </summary>
        public float HCost
        {
            get
            {
                return hCost;
            }

            set
            {
                hCost = value;
            }
        }

        // end temp

        public void AddConnection(Node node)
        {
            if (!connectedNodes.Contains(node) && node != this && node != null)
            {
                connectedNodes.Add(node);
                node.connectedNodes.Add(this);
            }
        }

        public void RemoveConnection(Node node)
        {
            connectedNodes.Remove(node);
        }

        public IEnumerable<Node> GetConnectedNodes()
        {
            return connectedNodes;
        }

        public IEnumerable<Node> GetWalkableConnectedNodes()
        {
            foreach (var node in connectedNodes)
            {
                if (node.type == NodeType.Walkable)
                {
                    yield return node;
                }
            }
        }
    }
}
