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
                grid.GetNodesBetweenNonAlloc(nodeLine, nodeA, nodeB);
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
    }
}
