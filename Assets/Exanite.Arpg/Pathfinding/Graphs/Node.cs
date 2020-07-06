using System;
using System.Collections.Generic;
using UnityEngine;

namespace Exanite.Arpg.Pathfinding.Graphs
{
    public class Node
    {
        private readonly NavGrid grid;
        private readonly Vector2Int gridPosition;

        private Vector3 position;
        private float height;
        private NodeType type = NodeType.Walkable;

        private HashSet<Node> connectedNodes = new HashSet<Node>();

        public Node(NavGrid grid, Vector2Int gridPosition)
        {
            this.grid = grid ?? throw new ArgumentNullException(nameof(grid));
            this.gridPosition = gridPosition;

            CalculateNewPosition();
        }

        public NavGrid Grid
        {
            get
            {
                return grid;
            }
        }

        public Vector2Int GridPosition
        {
            get
            {
                return gridPosition;
            }
        }

        public Vector3 Position
        {
            get
            {
                return position;
            }

            private set
            {
                position = value;
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

                CalculateNewPosition();
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

        public IEnumerable<Node> GetWalkableConnectedNodes(float maxStepAngle = float.PositiveInfinity)
        {
            return GetWalkableConnectedNodesNonAlloc(new List<Node>(), maxStepAngle);
        }

        public IList<Node> GetWalkableConnectedNodesNonAlloc(IList<Node> results, float maxStepAngle = float.PositiveInfinity)
        {
            results.Clear();

            foreach (var other in connectedNodes)
            {
                if (other.type == NodeType.Walkable 
                    && Mathf.Abs(GetAngleBetween(this, other)) <= maxStepAngle)
                {
                    results.Add(other);
                }
            }

            return results;
        }

        private void CalculateNewPosition()
        {
            Position = new Vector3(GridPosition.x * Grid.DistanceBetweenNodes, Height, GridPosition.y * Grid.DistanceBetweenNodes);
        }

        private static float GetVerticalDistance(Node a, Node b)
        {
            return a.Height - b.Height;
        }

        private static float GetHorizontalDistance(Node a, Node b)
        {
            Vector3 displacement = a.Position - b.Position;

            displacement.y = 0;

            return displacement.magnitude;
        }

        private static float GetAngleBetween(Node a, Node b)
        {
            return Mathf.Atan((GetVerticalDistance(a, b)) / GetHorizontalDistance(a, b)) * Mathf.Rad2Deg;
        }
    }
}
