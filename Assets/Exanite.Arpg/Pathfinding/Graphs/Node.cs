using System;
using System.Collections.Generic;
using UnityEngine;

namespace Exanite.Arpg.Pathfinding.Graphs
{
    public class Node
    {
        private readonly NavGrid grid;
        private readonly Vector2Int gridPosition;

        private float height;
        private NodeType type = NodeType.Walkable;

        private HashSet<Node> connectedNodes = new HashSet<Node>();

        public Node(NavGrid grid, Vector2Int gridPosition)
        {
            this.grid = grid ?? throw new ArgumentNullException(nameof(grid));
            this.gridPosition = gridPosition;
        }

        public NavGrid Grid
        {
            get
            {
                return grid;
            }
        }

        public Vector3 Position
        {
            get
            {
                return new Vector3(GridPosition.x * Grid.DistanceBetweenNodes, Height, GridPosition.y * Grid.DistanceBetweenNodes);
            }
        }

        public Vector2Int GridPosition
        {
            get
            {
                return gridPosition;
            }
        }

        public float Height
        {
            get
            {
                return height;
            }

            set
            {
                height = value;
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
