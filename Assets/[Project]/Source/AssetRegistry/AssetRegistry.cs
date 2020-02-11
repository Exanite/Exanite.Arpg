using System.Collections.Generic;
using Exanite.Arpg.AssetRegistry.Providers;

namespace Exanite.Arpg.AssetRegistry
{
    public class AssetRegistry
    {
        private readonly Dictionary<Key, AssetEntry> entries;

        public AssetRegistry()
        {
            entries = new Dictionary<Key, AssetEntry>();
        }

        public void AddEntry<T>(Key key, AssetEntry<T> entry)
        {
            entries.Add(key, entry);
        }

        public void RemoveEntry(Key key)
        {
            entries.Remove(key);
        }

        public AssetProvider<T> GetProvider<T>(Key key)
        {
            return GetEntry<T>(key).TypedProvider;
        }

        public AssetEntry<T> GetEntry<T>(Key key)
        {
            if (entries.ContainsKey(key))
            {
                return (AssetEntry<T>)entries[key];
            }

            return null;
        }
    }
}
