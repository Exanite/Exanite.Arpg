using System.Collections.Generic;
using UnityEngine;

namespace Exanite.Arpg.Pathfinding.Graphs
{
    public class NavGridLines : MonoBehaviour
    {
        public NavGrid grid;

        public KeyCode selectNodeKey = KeyCode.Mouse0;

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

            if (Input.GetKey(selectNodeKey))
            {
                if (grid.RaycastNode(Camera.main.ScreenPointToRay(Input.mousePosition), out node))
                {
                    nodeA = node;
                }
            }

            if (grid.RaycastNode(Camera.main.ScreenPointToRay(Input.mousePosition), out node))
            {
                nodeB = node;
            }
        }

        private void OnDrawGizmos()
        {
            if (!grid || nodeA == null || nodeB == null)
            {
                return;
            }

            Gizmos.color = Color.red;

            Gizmos.DrawLine(nodeA.Position, nodeB.Position);

            Gizmos.color = Color.yellow;

            foreach (var node in nodeLine)
            {
                Gizmos.DrawCube(node.Position, new Vector3(0.9f, 0, 0.9f) * grid.DistanceBetweenNodes);
            }
        }

        public IList<Node> GetLine(IList<Node> results, NavGrid grid, Node a, Node b)
        {
            return results;
        }
    }
}
