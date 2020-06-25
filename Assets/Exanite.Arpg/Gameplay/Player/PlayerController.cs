using Exanite.Arpg.Pathfinding;
using Exanite.Arpg.Pathfinding.Graphs;
using UnityEngine;

namespace Exanite.Arpg.Gameplay.Player
{
    public class PlayerController : MonoBehaviour
    {
        public NavGrid grid;

        public KeyCode moveKey = KeyCode.Mouse0;

        public float moveSpeed = 1;

        private Path path;

        private Pathfinder pathfinder = new Pathfinder();

        private void Update()
        {
            if (Input.GetKey(moveKey) && grid.Nodes != null)
            {
                if (grid.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out Vector3 hitPosition))
                {
                    Node destination = grid.GetClosestWalkableNode(hitPosition, 3);

                    if (destination != null)
                    {
                        var result = pathfinder.FindPath(grid.GetClosestNode(transform.position), destination);

                        if (result.isSuccess)
                        {
                            path = result.path;
                        }
                        else
                        {
                            path = null;
                        }
                    }
                }
            }

            if (path != null)
            {
                if (path.Waypoints.Count > 0)
                {
                    transform.position = Vector3.MoveTowards(transform.position, path.Waypoints[0], moveSpeed * Time.deltaTime);

                    if (Vector3.Distance(transform.position, path.Waypoints[0]) < 0.1f)
                    {
                        path.Waypoints.RemoveAt(0);
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (path != null)
            {
                for (int i = 1; i < path.Waypoints.Count; i++)
                {
                    Gizmos.color = Color.red;

                    Gizmos.DrawLine(path.Waypoints[i] + grid.NodeDrawHeightOffset, path.Waypoints[i - 1] + grid.NodeDrawHeightOffset);
                }
            }
        }
    }
}
