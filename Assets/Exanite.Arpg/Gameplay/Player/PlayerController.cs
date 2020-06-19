using Exanite.Arpg.Pathfinding;
using Exanite.Arpg.Pathfinding.Graphs;
using UnityEngine;

namespace Exanite.Arpg.Gameplay.Player
{
    public class PlayerController : MonoBehaviour
    {
        public NavGrid graph;
        public Pathfinder pathfinder = new Pathfinder();

        public float moveSpeed = 1;

        public KeyCode moveKey = KeyCode.Mouse0;

        private void Update()
        {
            if (Input.GetKey(moveKey) && graph.isGenerated)
            {
                if (graph.RaycastNode(Camera.main.ScreenPointToRay(Input.mousePosition), out Node targetNode))
                {
                    pathfinder.FindPath(graph.GetClosestNode(transform.position), targetNode);
                }
            }

            if (pathfinder.IsPathValid)
            {
                if (pathfinder.Path.Count > 0)
                {
                    transform.position = Vector3.MoveTowards(transform.position, pathfinder.Path[0], moveSpeed * Time.deltaTime);

                    if (Vector3.Distance(transform.position, pathfinder.Path[0]) < 0.1f)
                    {
                        pathfinder.Path.RemoveAt(0);
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (pathfinder.IsPathValid)
            {
                for (int i = 1; i < pathfinder.Path.Count; i++)
                {
                    Gizmos.color = Color.red;

                    Gizmos.DrawLine(pathfinder.Path[i] + graph.NodeDrawHeightOffset, pathfinder.Path[i - 1] + graph.NodeDrawHeightOffset);
                }
            }
        }
    }
}
