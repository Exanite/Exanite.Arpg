using Exanite.Core.Helpers;
using System;

namespace Exanite.MapGeneration.Extensions
{
    public static class TileSideExtensions
    {
        /// <summary>
        /// Rotates the side by the specified rotation
        /// </summary>
        public static TileSide Rotate(this TileSide side, TileRotation rotation)
        {
            switch (rotation)
            {
                case TileRotation.Normal: return side;
                case TileRotation.Clockwise90: return side.RotateClockwise90();
                case TileRotation.Clockwise180: return side.RotateClockwise180();
                case TileRotation.Clockwise270: return side.RotateClockwise270();
                default: throw new ArgumentOutOfRangeException(nameof(rotation));
            }
        }

        /// <summary>
        /// Rotates the side by 90 degrees clockwise
        /// </summary>
        public static TileSide RotateClockwise90(this TileSide side)
        {
            side++;

            if ((int)side > EnumHelper<TileSide>.Max)
            {
                side = 0;
            }

            return side;
        }

        /// <summary>
        /// Rotates the side by 180 degrees clockwise
        /// </summary>
        public static TileSide RotateClockwise180(this TileSide side)
        {
            return side.FlipX().FlipZ();
        }

        /// <summary>
        /// Rotates the side by 270 degrees clockwise
        /// </summary>
        public static TileSide RotateClockwise270(this TileSide side)
        {
            side--;

            if (side < 0)
            {
                side = (TileSide)EnumHelper<TileSide>.Max;
            }

            return side;
        }

        /// <summary>
        /// Flips the side by the specified flip
        /// </summary>
        public static TileSide Flip(this TileSide side, TileFlip flip)
        {
            if (flip.HasFlag(TileFlip.FlipX))
            {
                side = side.FlipX();
            }

            if (flip.HasFlag(TileFlip.FlipZ))
            {
                side = side.FlipZ();
            }

            return side;
        }

        /// <summary>
        /// Flips the side on the X-Axis, this does nothing if the side is on the Z-Axis
        /// </summary>
        public static TileSide FlipX(this TileSide side)
        {
            switch (side)
            {
                case TileSide.NegativeX: return TileSide.PositiveX;
                case TileSide.PositiveX: return TileSide.NegativeX;
            }

            return side;
        }

        /// <summary>
        /// Flips the side on the Z-Axis, this does nothing if the side is on the X-Axis
        /// </summary>
        public static TileSide FlipZ(this TileSide side)
        {
            switch (side)
            {
                case TileSide.NegativeZ: return TileSide.PositiveZ;
                case TileSide.PositiveZ: return TileSide.NegativeZ;
            }

            return side;
        }
    }
}
