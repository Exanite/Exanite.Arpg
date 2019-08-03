using System;

namespace Exanite.MapGeneration
{
    [Flags, Serializable]
    public enum TileFlip
    {
        Normal = 0 << 0,
        FlipX = 1 << 0,
        FlipZ = 1 << 1,
        FlipXZ = FlipX | FlipZ,
    }
}
