using Exanite.Arpg.Pathfinding;
using Exanite.Arpg.Pathfinding.Graphs;
using UniRx.Async;
using UnityEngine;

public class PathfindingTest : MonoBehaviour
{
    public NavGrid grid;

    public Transform start;
    public Transform destination;

    private Node startNode;
    private Node destinationNode;
    private Pathfinder pathfinder = new Pathfinder();

    private async UniTask Start()
    {
        while (true)
        {
            if (grid.isGenerated)
            {
                startNode = grid.GetClosestNode(start.position);
                destinationNode = grid.GetClosestNode(destination.position);

                await pathfinder.FindPathAsync(startNode, destinationNode);
            }

            await UniTask.Yield();
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

            Gizmos.DrawCube(startNode.Position + grid.NodeDrawHeightOffset, new Vector3(0.9f, 0f, 0.9f) * grid.nodeSize);
            Gizmos.DrawCube(destinationNode.Position + grid.NodeDrawHeightOffset, new Vector3(0.9f, 0f, 0.9f) * grid.nodeSize);
        }

        if (pathfinder.IsPathValid)
        {
            for (int i = 1; i < pathfinder.Path.Count; i++)
            {
                Gizmos.color = Color.red;

                Gizmos.DrawLine(pathfinder.Path[i].Position + grid.NodeDrawHeightOffset, pathfinder.Path[i - 1].Position + grid.NodeDrawHeightOffset);
            }
        }
    }
}
