using Exanite.Arpg.Pathfinding;
using Exanite.Arpg.Pathfinding.Graphs;
using UnityEngine;

namespace Exanite.Arpg.Gameplay.Player
{
    public class PlayerController : MonoBehaviour
    {
        public NavGrid grid;

        public float moveSpeed = 1;

        public KeyCode moveKey = KeyCode.Mouse0;

        private Pathfinder pathfinder = new Pathfinder();

        private void Update()
        {
            if (Input.GetKey(moveKey) && grid.isGenerated)
            {
                if (grid.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out Vector3 hitPosition))
                {
                    Node destination = grid.GetClosestWalkableNode(hitPosition, 3);

                    if (destination != null)
                    {
                        pathfinder.FindPath(grid.GetClosestNode(transform.position), destination);
                    }
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

                    Gizmos.DrawLine(pathfinder.Path[i] + grid.NodeDrawHeightOffset, pathfinder.Path[i - 1] + grid.NodeDrawHeightOffset);
                }
            }
        }
    }
}
