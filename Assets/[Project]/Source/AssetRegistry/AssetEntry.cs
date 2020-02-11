using System;
using Exanite.Arpg.AssetRegistry.Providers;

namespace Exanite.Arpg.AssetRegistry
{
    public abstract class AssetEntry
    {
        public const string DefaultPackageName = "Default";

        private readonly Key key;
        private readonly string packageName;
        private readonly string assetName;
        private readonly AssetProvider provider;

        public Key Key
        {
            get
            {
                return key;
            }
        }

        public string PackageName
        {
            get
            {
                return packageName;
            }
        }

        public string AssetName
        {
            get
            {
                return assetName;
            }
        }

        public AssetProvider Provider
        {
            get
            {
                return provider;
            }
        }

        public AssetEntry(string assetName, AssetProvider provider) : this(assetName, DefaultPackageName, provider) { }

        public AssetEntry(string assetName, string packageName, AssetProvider provider)
        {
            this.packageName = packageName ?? throw new ArgumentNullException(nameof(packageName));
            this.assetName = assetName ?? throw new ArgumentNullException(nameof(assetName));
            this.provider = provider ?? throw new ArgumentNullException(nameof(provider));

            key = new Key($"{packageName}:{assetName}");
        }
    }
}
