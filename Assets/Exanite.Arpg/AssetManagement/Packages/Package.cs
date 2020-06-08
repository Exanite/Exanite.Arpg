using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Exanite.Arpg.AssetManagement.Packages
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class Package
    {
        private AssetBundle assetBundle;
        private string name;
        private PackageType type;
        private List<PackageAssetEntry> entries = new List<PackageAssetEntry>();

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

        [JsonProperty]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        [JsonProperty]
        public PackageType Type
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

        [JsonProperty]
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

        public static Package LoadPackage(string packageInfoPath)
        {
            string assetBundlePath = Path.ChangeExtension(packageInfoPath, Constants.AssetBundleFileExtension);
            var assetBundle = AssetBundle.LoadFromFile(assetBundlePath);

            string packageInfoJson = File.ReadAllText(packageInfoPath);
            var package = JsonConvert.DeserializeObject<Package>(packageInfoJson);

            package.AssetBundle = assetBundle;

            return package;
        }
    }
}
