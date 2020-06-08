using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Exanite.Arpg.AssetManagement.Packages
{
    public class Package
    {
        private AssetBundle assetBundle;
        private PackageInfo info;

        public AssetBundle AssetBundle
        {
            get
            {
                return assetBundle;
            }

            set
            {
                assetBundle = value;
            }
        }

        public PackageInfo Info
        {
            get
            {
                return info;
            }

            set
            {
                info = value;
            }
        }

        public static Package LoadPackage(string packageInfoPath)
        {
            string assetBundlePath = Path.ChangeExtension(packageInfoPath, Constants.AssetBundleFileExtension);
            var assetBundle = AssetBundle.LoadFromFile(assetBundlePath);

            string packageInfoJson = File.ReadAllText(packageInfoPath);
            var packageInfo = JsonConvert.DeserializeObject<PackageInfo>(packageInfoJson);

            var package = new Package
            {
                AssetBundle = assetBundle,
                Info = packageInfo,
            };

            return package;
        }
    }
}
