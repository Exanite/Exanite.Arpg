using System.Collections.Generic;
using UnityEngine;

namespace Exanite.Arpg.AssetRegistry
{
    public class AssetRegistry
    {
        private readonly Dictionary<Key, AssetEntry> entries;
        private readonly Dictionary<Key, AssetBundle> bundles;

        public Dictionary<Key, AssetEntry> Entries
        {
            get
            {
                return entries;
            }
        }

        public Dictionary<Key, AssetBundle> Bundles
        {
            get
            {
                return bundles;
            }
        }
    }
}
