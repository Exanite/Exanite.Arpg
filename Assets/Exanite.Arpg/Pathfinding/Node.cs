using System.Collections.Generic;
using UnityEngine;

namespace Exanite.Arpg.Pathfinding
{
    public class Node
    {
        private Vector3 position;
        private NodeType type = NodeType.Walkable;

        private HashSet<Node> connectedNodes = new HashSet<Node>();

        public Node() { }

        public Node(Vector3 position, NodeType type)
        {
            Position = position;
            Type = type;
        }

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

        public IEnumerable<Node> ConnectedNodes
        {
            get
            {
                return connectedNodes;
            }
        }

        public void AddConnection(Node node)
        {
            if (!connectedNodes.Contains(node) && node != this && node != null)
            {
                connectedNodes.Add(node);
            }
        }

        public void RemoveConnection(Node node)
        {
            connectedNodes.Remove(node);
        }
    }
}
