using System.Collections.Generic;
using Exanite.Arpg.AssetManagement.Packages;

namespace Exanite.Arpg.AssetManagement.Registry
{
    public class AssetRegistry
    {
        public Dictionary<Key, AssetEntry> entries;
        public Dictionary<Key, Package> packages;

        public void LoadPackage(Package package)
        {
            packages.Add(package.Name, package);

            Key key;
            AssetEntry assetEntry;
            foreach (var packageAssetEntry in package.Entries)
            {
                key = packageAssetEntry.Key;

                if (entries.ContainsKey(key))
                {
                    assetEntry = entries[key];
                }
                else
                {
                    assetEntry = new AssetEntry();
                    entries[key] = assetEntry;
                }

                assetEntry.Key = key;
                assetEntry.Package = package;
                assetEntry.Type = packageAssetEntry.Type;
            }
        }
    }
}
