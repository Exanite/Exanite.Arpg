using System;

namespace Exanite.Arpg.AssetManagement.Packages
{
    public class PackageAssetEntry
    {
        private string key;
        private Type type;

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

        public Type Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }
    }
}
