using Exanite.Arpg.Pathfinding;
using UnityEngine;

public class PathfindingTest : MonoBehaviour
{
    public NavGrid grid;

    public Transform start;
    public Transform destination;

    private Node startNode;
    private Node destinationNode;
    private Path path = new Path();

    private void Update()
    {
        if (grid.isGenerated)
        {
            startNode = grid.GetClosestNode(start.position);
            destinationNode = grid.GetClosestNode(destination.position);

            Path.FindPath(grid, startNode, destinationNode, path);
        }
    }

    private void OnDrawGizmos()
    {
        if (!grid.isGenerated)
        {
            return;
        }

        if (startNode != null && destinationNode != null)
        {
            Gizmos.color = Color.yellow * 0.75f;

            Gizmos.DrawCube(startNode.Position, new Vector3(0.9f, 0f, 0.9f) * grid.nodeSize);
            Gizmos.DrawCube(destinationNode.Position, new Vector3(0.9f, 0f, 0.9f) * grid.nodeSize);
        }

        for (int i = 1; i < path.Nodes.Count; i++)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawLine(path.Nodes[i].Position, path.Nodes[i - 1].Position);
        }
    }
}
