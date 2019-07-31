using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Exanite.MapGeneration
{
    [CreateAssetMenu(fileName = "Tileset", menuName = "Map Generation/Tileset", order = 0)]
    public class Tileset : SerializedScriptableObject, IEnumerable<Tile>
    {
        [InlineEditor, ListDrawerSettings]
        [ShowInInspector, OdinSerialize] public List<Tile> Tiles { get; set; } = new List<Tile>();

        public IEnumerator<Tile> GetEnumerator()
        {
            return Tiles.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    } 
}
