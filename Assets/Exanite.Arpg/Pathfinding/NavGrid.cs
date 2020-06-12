﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Exanite.Arpg.Pathfinding
{
    public class NavGrid : MonoBehaviour, IEnumerable<Node>
    {
        public Node[,] nodes;
        public float nodeSize = 1;
        public bool isGenerated = false;

        public Plane Plane
        {
            get
            {
                return new Plane(transform.up, transform.position);
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

                DrawNode(node);

                DrawNodeConnections(node);
            }
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
            return this.Aggregate((x, y) => ((position - x.Position).sqrMagnitude < (position - y.Position).sqrMagnitude) ? x : y);
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

            Gizmos.DrawCube(node.Position, new Vector3(0.9f, 0, 0.9f) * nodeSize);
        }

        private void DrawNodeConnections(Node node)
        {
            Gizmos.color = Color.white;

            if (node.Type == NodeType.Walkable)
            {
                foreach (var other in node.ConnectedNodes)
                {
                    if (other.Type == NodeType.Walkable)
                    {
                        Gizmos.DrawLine(node.Position, other.Position);
                    }
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
