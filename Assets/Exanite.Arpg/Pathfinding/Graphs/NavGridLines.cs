using System.Collections.Generic;
using UnityEngine;

namespace Exanite.Arpg.Pathfinding.Graphs
{
    public class NavGridLines : MonoBehaviour
    {
        public NavGrid grid;

        public KeyCode selectNodeAKey = KeyCode.A;
        public KeyCode selectNodeBKey = KeyCode.B;

        private Node nodeA;
        private Node nodeB;

        private List<Node> nodeLine = new List<Node>();

        private void Update()
        {
            if (!grid)
            {
                return;
            }

            Node node;

            if (Input.GetKey(selectNodeAKey))
            {
                if (grid.RaycastNode(Camera.main.ScreenPointToRay(Input.mousePosition), out node))
                {
                    nodeA = node;
                }
            }

            if (Input.GetKey(selectNodeBKey))
            {
                if (grid.RaycastNode(Camera.main.ScreenPointToRay(Input.mousePosition), out node))
                {
                    nodeB = node;
                }
            }

            UpdateLine();
        }

        private void UpdateLine()
        {
            if (nodeA != null && nodeB != null)
            {
                GetNodesBetweenNonAlloc(nodeLine, grid, nodeA, nodeB);
            }
        }

        private void OnDrawGizmos()
        {
            if (!grid || nodeA == null || nodeB == null)
            {
                return;
            }

            for (int i = 0; i < nodeLine.Count; i++)
            {
                Gizmos.color = i % 2 == 0 ? Color.yellow : Color.blue;
                Gizmos.color *= 0.5f;

                Gizmos.DrawCube(nodeLine[i].Position, new Vector3(0.9f, 0, 0.9f) * grid.DistanceBetweenNodes);
            }

            Gizmos.color = Color.red;

            Gizmos.DrawLine(nodeA.Position, nodeB.Position);
        }

        public IList<Node> GetNodesBetween(NavGrid grid, Node start, Node end)
        {
            return GetNodesBetweenNonAlloc(new List<Node>(), grid, start, end);
        }

        public IList<Node> GetNodesBetweenNonAlloc(IList<Node> results, NavGrid grid, Node start, Node end)
        {
            results.Clear();

            if (start == end)
            {
                results.Add(start);

                return results;
            }

            int differenceX = end.GridPosition.x - start.GridPosition.x;
            int differenceY = end.GridPosition.y - start.GridPosition.y;
            int totalDistance = Mathf.Abs(differenceX) + Mathf.Abs(differenceY);

            float dx = (float)differenceX / totalDistance;
            float dy = (float)differenceY / totalDistance;

            int currentX = start.GridPosition.x;
            int currentY = start.GridPosition.y;

            float x = 0;
            float y = 0;

            int moveDirectionX = differenceX < 0 ? -1 : 1;
            int moveDirectionY = differenceY < 0 ? -1 : 1;

            results.Add(start);

            for (int i = 0; i < totalDistance; i++)
            {
                x += Mathf.Abs(dx);
                y += Mathf.Abs(dy);

                if (x > y)
                {
                    x--;
                    currentX += moveDirectionX;
                }
                else
                {
                    y--;
                    currentY += moveDirectionY;
                }

                results.Add(grid.Nodes[currentX, currentY]);
            }

            return results;
        }
    }
}
