using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Zenject;
using ILogger = Serilog.ILogger;

namespace Exanite.Arpg.Pathfinding.Graphs
{
    public class NavGrid : MonoBehaviour, IEnumerable<Node>
    {
        private Node[,] nodes;

        [Header("Generation:")]
        [SerializeField] private bool generateOnStart = false;
        [SerializeField] private int sizeX = 10;
        [SerializeField] private int sizeY = 10;
        [SerializeField] private float gridMaxHeight = 100; // temporary
        [SerializeField] private float distanceBetweenNodes = 1;
        [SerializeField] private bool generateDiagonals = false;

        [Header("Drawing:")]
        [SerializeField] private bool enableNodeDrawing = false;
        [SerializeField] private bool enableNodeConnectionDrawing = false;
        [SerializeField] private float nodeDrawHeightOffsetAmount = 0.1f;

        private ILogger log;

        [Inject]
        public void Inject(ILogger log)
        {
            this.log = log.ForContext<NavGrid>();
        }

        public Node[,] Nodes
        {
            get
            {
                return nodes;
            }
        }

        public bool GenerateOnStart
        {
            get
            {
                return generateOnStart;
            }

            set
            {
                generateOnStart = value;
            }
        }

        public int SizeX
        {
            get
            {
                return sizeX;
            }

            set
            {
                sizeX = value;
            }
        }

        public int SizeY
        {
            get
            {
                return sizeY;
            }

            set
            {
                sizeY = value;
            }
        }

        public float DistanceBetweenNodes
        {
            get
            {
                return distanceBetweenNodes;
            }

            set
            {
                distanceBetweenNodes = value;
            }
        }

        public bool GenerateDiagonals
        {
            get
            {
                return generateDiagonals;
            }

            set
            {
                generateDiagonals = value;
            }
        }

        public bool EnableNodeDrawing
        {
            get
            {
                return enableNodeDrawing;
            }

            set
            {
                enableNodeDrawing = value;
            }
        }

        public bool EnableNodeConnectionDrawing
        {
            get
            {
                return enableNodeConnectionDrawing;
            }

            set
            {
                enableNodeConnectionDrawing = value;
            }
        }

        public float NodeDrawHeightOffsetAmount
        {
            get
            {
                return nodeDrawHeightOffsetAmount;
            }

            set
            {
                nodeDrawHeightOffsetAmount = value;
            }
        }

        public Vector3 NodeDrawHeightOffset
        {
            get
            {
                return NodeDrawHeightOffsetAmount * Vector3.up;
            }
        }

        public Plane Plane
        {
            get
            {
                return new Plane(transform.up, transform.position);
            }
        }

        private void Start()
        {
            if (GenerateOnStart)
            {
                Generate();
            }
        }

        private void OnDrawGizmos()
        {
            if (Nodes == null)
            {
                return;
            }

            foreach (var node in Nodes)
            {
                if (node == null)
                {
                    continue;
                }

                if (EnableNodeDrawing)
                {
                    DrawNode(node);
                }

                if (EnableNodeConnectionDrawing)
                {
                    DrawNodeConnections(node);
                }
            }
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

            Gizmos.DrawCube(node.Position + NodeDrawHeightOffset, new Vector3(0.9f, 0, 0.9f) * DistanceBetweenNodes);
        }

        private void DrawNodeConnections(Node node)
        {
            Gizmos.color = Color.white;

            if (node.Type == NodeType.Walkable)
            {
                foreach (var other in node.GetWalkableConnectedNodes())
                {
                    Gizmos.DrawLine(node.Position + NodeDrawHeightOffset, other.Position + NodeDrawHeightOffset);
                }
            }
        }

        public bool Raycast(Ray ray, out Vector3 hitPosition)
        {
            hitPosition = Vector3.zero;

            if (Plane.Raycast(ray, out float enter))
            {
                hitPosition = ray.GetPoint(enter);

                return true;
            }

            return false;
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
            int x = Mathf.RoundToInt(position.x / DistanceBetweenNodes);
            int y = Mathf.RoundToInt(position.z / DistanceBetweenNodes);

            if (x < 0 || x >= SizeX || y < 0 || y >= sizeY)
            {
                return null;
            }
            else
            {
                return nodes[x, y];
            }
        }

        public Node GetWalkableNodeAlongLine(Vector3 lineEnd, Vector3 lineStart)
        {
            float searchIncrement = DistanceBetweenNodes / 2;

            Vector3 direction = (lineStart - lineEnd).normalized;
            Vector3 currentPosition = lineEnd;

            Node closestNode;

            while (currentPosition != lineStart)
            {
                closestNode = GetClosestNode(currentPosition);

                if (closestNode != null && closestNode.Type == NodeType.Walkable)
                {
                    return closestNode;
                }

                currentPosition = Vector3.MoveTowards(currentPosition, lineStart, searchIncrement);
            }

            return null;
        }

        // Crude method of testing walkability for now, does not support height differences in the path
        public bool IsDirectlyWalkable(Node a, Node b)
        {
            if (a.Type != NodeType.Walkable || b.Type != NodeType.Walkable)
            {
                return false;
            }

            float searchIncrement = DistanceBetweenNodes / 2;

            Vector3 direction = (b.Position - a.Position).normalized;
            Vector3 currentPosition = a.Position;

            Node closestNode;

            while (currentPosition != b.Position)
            {
                closestNode = GetClosestNode(currentPosition);

                if (closestNode == null || closestNode.Type != NodeType.Walkable)
                {
                    return false;
                }

                currentPosition = Vector3.MoveTowards(currentPosition, b.Position, searchIncrement);
            }

            return true;
        }

        public IList<Node> GetNodesBetween(Node start, Node end)
        {
            return GetNodesBetweenNonAlloc(new List<Node>(), start, end);
        }

        public IList<Node> GetNodesBetweenNonAlloc(IList<Node> results, Node start, Node end)
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

                results.Add(Nodes[currentX, currentY]);
            }

            return results;
        }

        public void Generate()
        {
            log.Information("Creating grid of size ({SizeX}, {SizeY})", SizeX, SizeY);

            Stopwatch watch = new Stopwatch();
            watch.Start();

            ClearGrid();

            nodes = new Node[SizeX, SizeY];

            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    var node = new Node(this, new Vector2Int(x, y));

                    if (Physics.Raycast(new Vector3(x * DistanceBetweenNodes, gridMaxHeight, y * DistanceBetweenNodes), Vector3.down, out RaycastHit hit))
                    {
                        node.Height = hit.point.y;
                    }
                    else
                    {
                        node.Type = NodeType.NonWalkable;
                    }

                    if (x > 0)
                    {
                        node.AddConnection(Nodes[x - 1, y]);
                    }

                    if (y > 0)
                    {
                        node.AddConnection(Nodes[x, y - 1]);
                    }

                    if (GenerateDiagonals)
                    {
                        if (x > 0 && y > 0)
                        {
                            node.AddConnection(Nodes[x - 1, y - 1]);
                        }

                        if (x > 0 && y < SizeY - 1)
                        {
                            node.AddConnection(Nodes[x - 1, y + 1]);
                        }
                    }

                    if (Physics.OverlapSphere(node.Position + Vector3.up * 0.1f, 0).Length > 0
                        || !Physics.Raycast(node.Position + Vector3.up * 1f, Vector3.down, 2f))
                    {
                        node.Type = NodeType.NonWalkable;
                    }

                    Nodes[x, y] = node;
                }
            }

            watch.Stop();

            log.Information("Finished grid creation, took {Milliseconds} milliseconds", watch.ElapsedMilliseconds);
        }

        public void ClearGrid()
        {
            nodes = null;
        }

        public IEnumerator<Node> GetEnumerator()
        {
            for (int x = 0; x < Nodes.GetLength(0); x++)
            {
                for (int y = 0; y < Nodes.GetLength(1); y++)
                {
                    yield return Nodes[x, y];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
