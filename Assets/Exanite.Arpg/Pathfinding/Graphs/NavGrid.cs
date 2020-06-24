using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Exanite.Arpg.Pathfinding.Graphs
{
    public class NavGrid : MonoBehaviour, IEnumerable<Node>
    {
        public Node[,] nodes;
        public float nodeSize = 1;
        public bool isGenerated = false;
        public bool drawNodes = true;
        public bool drawNodeConnections = false;
        public float nodeDrawHeightOffsetAmount = 0.1f;

        public Plane Plane
        {
            get
            {
                return new Plane(transform.up, transform.position);
            }
        }

        public Vector3 NodeDrawHeightOffset
        {
            get
            {
                return nodeDrawHeightOffsetAmount * Vector3.up;
            }
        }

        private void OnDrawGizmos()
        {
            if (nodes == null)
            {
                return;
            }

            foreach (var node in nodes)
            {
                if (node == null)
                {
                    continue;
                }

                if (drawNodes)
                {
                    DrawNode(node);
                }

                if (drawNodeConnections)
                {
                    DrawNodeConnections(node);
                }
            }
        }

        public bool Raycast(Ray ray, out Vector3 hitPosition)
        {
            hitPosition = Vector3.zero;

            if (Plane.Raycast(ray, out float enter))
            {
                hitPosition = ray.GetPoint(enter);

                return true;
            }

            return false;
        }

        public bool RaycastNode(Ray ray, out Node node)
        {
            node = null;

            if (Plane.Raycast(ray, out float enter))
            {
                Vector3 hitPosition = ray.GetPoint(enter);

                node = GetClosestNode(hitPosition);

                return true;
            }

            return false;
        }

        public Node GetClosestNode(Vector3 position)
        {
            Node closest = null;
            float closestDistance = float.PositiveInfinity;

            float currentDistance;

            foreach (var node in this)
            {
                currentDistance = (node.Position - position).sqrMagnitude;

                if (currentDistance < closestDistance)
                {
                    closest = node;
                    closestDistance = currentDistance;
                }
            }

            return closest;
        }

        public Node GetClosestWalkableNode(Vector3 position, float maxDistance = float.PositiveInfinity)
        {
            Node closest = null;
            float closestDistance = float.PositiveInfinity;

            float currentDistance;

            foreach (var node in this)
            {
                if (node.Type == NodeType.NonWalkable)
                {
                    continue;
                }

                currentDistance = (node.Position - position).sqrMagnitude;

                if (currentDistance < closestDistance)
                {
                    closest = node;
                    closestDistance = currentDistance;
                }
            }

            if (closestDistance < maxDistance * maxDistance)
            {
                return closest;
            }

            return null;
        }

        public void ClearGrid()
        {
            nodes = null;
            isGenerated = false;
        }

        private void DrawNode(Node node)
        {
            switch (node.Type)
            {
                case NodeType.Walkable:
                {
                    Gizmos.color = Color.green * 0.5f;
                    break;
                }

                case NodeType.NonWalkable:
                {
                    Gizmos.color = Color.red * 0.5f;
                    break;
                }
            }

            Gizmos.DrawCube(node.Position + NodeDrawHeightOffset, new Vector3(0.9f, 0, 0.9f) * nodeSize);
        }

        private void DrawNodeConnections(Node node)
        {
            Gizmos.color = Color.white;

            if (node.Type == NodeType.Walkable)
            {
                foreach (var other in node.GetWalkableConnectedNodes())
                {
                    Gizmos.DrawLine(node.Position + NodeDrawHeightOffset, other.Position + NodeDrawHeightOffset);
                }
            }
        }

        public IEnumerator<Node> GetEnumerator()
        {
            for (int x = 0; x < nodes.GetLength(0); x++)
            {
                for (int y = 0; y < nodes.GetLength(1); y++)
                {
                    yield return nodes[x, y];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
