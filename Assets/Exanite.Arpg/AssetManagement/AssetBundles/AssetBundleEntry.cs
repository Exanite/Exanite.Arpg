using System;

namespace Exanite.Arpg.AssetManagement.AssetBundles
{
    public class AssetBundleEntry
    {
        private string key;
        private Type assetType;

        public string Key
        {
            get
            {
                return key;
            }

            set
            {
                key = value;
            }
        }

        public Type AssetType
        {
            get
            {
                return assetType;
            }

            set
            {
                assetType = value;
            }
        }
    }
}
