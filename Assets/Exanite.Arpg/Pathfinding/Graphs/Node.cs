using System.Collections.Generic;
using System.Linq;
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
