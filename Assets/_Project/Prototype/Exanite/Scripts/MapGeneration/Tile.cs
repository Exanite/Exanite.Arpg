using Exanite.MapGeneration.Extensions;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using UnityEngine;

namespace Exanite.MapGeneration
{
    public class Tile : SerializedMonoBehaviour
    {
        public const int Sides = 4;
        public const int ConnectionsPerSide = 3;

        [SerializeField, HideInInspector] private TileFlip flip;
        [SerializeField, HideInInspector] private TileRotation rotation;

        [ShowInInspector, EnumToggleButtons]
        public TileFlip Flip
        {
            get
            {
                return flip;
            }
            set
            {
                flip = value;

                transform.localScale = new Vector3(
                    flip.HasFlag(TileFlip.FlipX) ? -1 : 1,
                    1,
                    flip.HasFlag(TileFlip.FlipZ) ? -1 : 1);
            }
        }

        [ShowInInspector, EnumPaging]
        public TileRotation Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;

                transform.localRotation = Quaternion.Euler(0, (int)rotation * 90, 0);
            }
        }

        [OdinSerialize, HideInInspector] public bool[,] Connections { get; set; } = new bool[Sides, ConnectionsPerSide];

        public bool GetConnection(TileSide side, int connection)
        {
            var indexes = GetConnectionIndexes(side, connection);

            return Connections[indexes.x, indexes.y];
        }

        public void SetConnection(bool value, TileSide side, int connection)
        {
            var indexes = GetConnectionIndexes(side, connection);

            Connections[indexes.x, indexes.y] = value;
        }

        public (int x, int y) GetConnectionIndexes(TileSide side, int connection)
        {
            if (connection < 0 || connection >= ConnectionsPerSide)
            {
                throw new ArgumentOutOfRangeException(nameof(connection));
            }

            bool reverseConnection = false;

            side = side.Rotate(Rotation);

            if (Flip.HasFlag(TileFlip.FlipX))
            {
                side = side.FlipX();

                reverseConnection = !reverseConnection;
            }

            if (Flip.HasFlag(TileFlip.FlipZ))
            {
                side = side.FlipZ();

                reverseConnection = !reverseConnection;
            }

            if (reverseConnection)
            {
                switch (connection)
                {
                    case 0: connection = 2; break;
                    case 2: connection = 0; break;
                }
            }

            return ((int)side, connection);
        }
    }
}
