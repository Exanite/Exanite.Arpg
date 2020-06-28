using UnityEngine;

namespace Exanite.Arpg.Pathfinding.Graphs
{
    public class NavGridLinecaster : MonoBehaviour
    {
        public NavGrid target;

        public KeyCode setAKey = KeyCode.A;
        public KeyCode setBKey = KeyCode.B;

        private Node nodeA;
        private Node nodeB;

        private void Update()
        {
            if (!target)
            {
                return;
            }

            if (Input.GetKey(setAKey))
            {
                if (target.RaycastNode(Camera.main.ScreenPointToRay(Input.mousePosition), out Node node))
                {
                    nodeA = node;
                }
            }

            if (Input.GetKey(setBKey))
            {
                if (target.RaycastNode(Camera.main.ScreenPointToRay(Input.mousePosition), out Node node))
                {
                    nodeB = node;
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (nodeA != null && nodeB != null)
            {
                Gizmos.color = IsDirectlyWalkable(nodeA, nodeB) ? Color.green : Color.red;

                Gizmos.DrawLine(nodeA.Position, nodeB.Position);
            }
        }

        public bool IsDirectlyWalkable(Node a, Node b)
        {
            return false;
        }
    }
}
