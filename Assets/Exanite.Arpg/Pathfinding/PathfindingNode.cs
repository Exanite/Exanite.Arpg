using Exanite.Arpg.Pathfinding.Graphs;

namespace Exanite.Arpg.Pathfinding
{
    /// <summary>
    /// Represents a <see cref="NavGrid"/> <see cref="Node"/> for the purposes of pathfinding
    /// </summary>
    public class PathfindingNode
    {
        private Node node;
        private PathfindingNode parent;

        private float gCost;
        private float hCost;

        /// <summary>
        /// The <see cref="Node"/> this <see cref="PathfindingNode"/> is representing
        /// </summary>
        public Node Node
        {
            get
            {
                return node;
            }

            set
            {
                node = value;
            }
        }

        /// <summary>
        /// Parent of this node. The node that led to this node being opened
        /// </summary>
        public PathfindingNode Parent
        {
            get
            {
                return parent;
            }

            set
            {
                parent = value;
            }
        }

        /// <summary>
        /// The sum of GCost and HCost
        /// </summary>
        public float FCost
        {
            get
            {
                return GCost + HCost;
            }
        }

        /// <summary>
        /// The accurate distance from the start to this node
        /// </summary>
        public float GCost
        {
            get
            {
                return gCost;
            }

            set
            {
                gCost = value;
            }
        }

        /// <summary>
        /// The estimated distance from this node to the destination
        /// </summary>
        public float HCost
        {
            get
            {
                return hCost;
            }

            set
            {
                hCost = value;
            }
        }

        /// <summary>
        /// Sets all properties to the default values
        /// </summary>
        public void Reset()
        {
            Node = null;
            Parent = null;
            GCost = 0;
            HCost = 0;
        }
    }
}
