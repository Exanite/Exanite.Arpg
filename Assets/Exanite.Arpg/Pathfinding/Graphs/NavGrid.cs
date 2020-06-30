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

        [SerializeField] private bool generateOnStart = false;

        [SerializeField] private int sizeX = 10;
        [SerializeField] private int sizeY = 10;
        [SerializeField] private float distanceBetweenNodes = 1;
        [SerializeField] private bool generateDiagonals = false;

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
            Node closest = nodes[Mathf.RoundToInt(position.x / DistanceBetweenNodes), Mathf.RoundToInt(position.z / DistanceBetweenNodes)];

            return closest;
        }

        public Node GetClosestWalkableNode(Vector3 position, float maxDistance = float.PositiveInfinity)
        {
            Node closest = null;
            float closestDistance = float.PositiveInfinity;

            float currentDistance;

            foreach (var node in this)
            {
                if (node.Type == NodeType.NonWalkable)
                {
                    continue;
                }

                currentDistance = (node.Position - position).sqrMagnitude;

                if (currentDistance < closestDistance)
                {
                    closest = node;
                    closestDistance = currentDistance;
                }
            }

            if (closestDistance < maxDistance * maxDistance)
            {
                return closest;
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

            while (currentPosition != b.Position)
            {
                if (GetClosestNode(currentPosition).Type != NodeType.Walkable)
                {
                    return false;
                }

                currentPosition = Vector3.MoveTowards(currentPosition, b.Position, searchIncrement);
            }

            return true;
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
