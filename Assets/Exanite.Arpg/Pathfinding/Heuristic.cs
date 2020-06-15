using Exanite.Arpg.Pathfinding.Graphs;

namespace Exanite.Arpg.Pathfinding
{
    /// <summary>
    /// Calculates the distance between <see cref="Node"/> a and <see cref="Node"/> b
    /// </summary>
    public delegate float Heuristic(Node a, Node b);
}
