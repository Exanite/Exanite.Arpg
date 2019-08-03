using Exanite.Core.DataStructures;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Exanite.MapGeneration
{
    public class Map : SerializedMonoBehaviour
    {
        private Grid2D<GameObject> Tiles { get; set; }

        /// <summary>
        /// Creates a Tile at (<paramref name="x"/>, <paramref name="y"/>) <para/> Note: '<paramref name="z"/>' is height
        /// </summary>
        public GameObject CreateTileAt(GameObject prefab, int x, int y, int z = 0)
        {
            if (Tiles[x, y] != null)
            {
                throw new ArgumentException($"Unable to create tile, a tile already exists at ({x}, {y})");
            }

            var tile = Instantiate(prefab);
            tile.transform.SetParent(transform, true);
            tile.transform.localPosition = new Vector3(x, z, y);

            return tile;
        }

        public void Clear()
        {
            foreach (var tile in Tiles)
            {
                Destroy(tile);
            }

            Tiles.Clear();
        }
    } 
}
