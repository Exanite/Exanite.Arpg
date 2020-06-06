using System.Collections.Generic;

namespace Exanite.Arpg.AssetManagement.AssetBundles
{
    public class AssetBundleInfo
    {
        public const string FileExtension = "bundleinfo";

        private List<AssetBundleEntry> entries;

        public AssetBundleInfo()
        {
            Entries = new List<AssetBundleEntry>();
        }

        public AssetBundleInfo(int capacity)
        {
            entries = new List<AssetBundleEntry>(capacity);
        }

        public List<AssetBundleEntry> Entries
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
