using Exanite.Arpg.AssetRegistry.Providers;

namespace Exanite.Arpg.AssetRegistry
{
    public class AssetEntry<T> : AssetEntry
    {
        public AssetProvider<T> TypedProvider
        {
            get
            {
                return (AssetProvider<T>)Provider;
            }
        }

        public AssetEntry(string assetName, AssetProvider<T> provider) : base(assetName, provider) { }

        public AssetEntry(string assetName, string packageName, AssetProvider<T> provider) : base(assetName, packageName, provider) { }
    }
}
