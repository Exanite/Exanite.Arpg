using System.Collections.Generic;

namespace Exanite.Arpg.AssetManagement.Packages
{
    public class PackageInfo
    {
        private List<PackageAssetEntry> entries;

        public PackageInfo()
        {
            Entries = new List<PackageAssetEntry>();
        }

        public PackageInfo(int capacity)
        {
            entries = new List<PackageAssetEntry>(capacity);
        }

        public List<PackageAssetEntry> Entries
        {
            get
            {
                return entries;
            }

            set
            {
                entries = value;
            }
        }
    }
}
