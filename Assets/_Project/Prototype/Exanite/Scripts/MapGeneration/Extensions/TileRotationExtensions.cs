namespace Exanite.MapGeneration.Extensions
{
    public static class TileRotationExtensions
    {
        /// <summary>
        /// Returns the inverse of the rotation
        /// </summary>
        public static TileRotation Inverse(this TileRotation rotation)
        {
            switch (rotation)
            {
                case TileRotation.Clockwise90: return TileRotation.Clockwise270;
                case TileRotation.Clockwise270: return TileRotation.Clockwise90;
                default: return rotation;
            }
        }
    }
}
