using System;
using UnityEngine;
using Random = System.Random;

namespace Exanite.MapGeneration
{
    public class MapBuilder : MapBuilder<MapBuilder, Map> { }

    public abstract class MapBuilder<TBuilder, TMap>
        where TBuilder : MapBuilder<TBuilder, TMap>
        where TMap : Map
    {
        public Layout Layout { get; set; }
        public Tileset Tileset { get; set; }
        public int? Seed { get; set; } = null;
        protected Random Random { get; set; }

        protected readonly TBuilder This;

        public MapBuilder()
        {
            This = (TBuilder)this;
        }

        public virtual TBuilder WithLayout(Layout layout)
        {
            Layout = layout;

            return This;
        }

        public virtual TBuilder WithTileset(Tileset tileset)
        {
            Tileset = tileset;

            return This;
        }

        public virtual TBuilder WithSeed(int seed)
        {
            Seed = seed;
            Random = new Random(seed);

            return This;
        }

        public virtual TBuilder WithRandomSeed()
        {
            int seed = new Guid().GetHashCode();
            WithSeed(seed);

            return This;
        }

        public virtual TMap Build()
        {
            Map map = new GameObject().AddComponent<Map>();

            //generate map

            return (TMap)map;
        }
    }
}
