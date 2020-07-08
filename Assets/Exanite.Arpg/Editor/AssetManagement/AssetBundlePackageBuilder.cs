using System;
using System.IO;
using System.Linq;
using Exanite.Arpg.AssetManagement;
using Exanite.Arpg.AssetManagement.Packages;
using Newtonsoft.Json;
using UnityEditor;

namespace Exanite.Arpg.Editor.AssetManagement
{
    public class AssetBundlePackageBuilder : PackageBuilder
    {
        private string assetsFolder;

        private string[] assetNames;
        private string[] addressableNames;

        private string lowerPackageName;

        public string AssetsFolder
        {
            get
            {
                return assetsFolder;
            }

            set
            {
                assetsFolder = value;
            }
        }

        [MenuItem("PackageBuilder/Build AssetBundlePackage (Test)")]
        public static void TestBuild()
        {
            new AssetBundlePackageBuilder
            {
                PackageName = "ExampleMod",
                AssetsFolder = "Assets/Mods/ExampleMod/Assets",
                BuildDirectory = @"D:\Repositories\Exanite.Arpg\Assets\Mods\ExampleMod\Bundles",
            }
            .Build();
        }

        public override void Build()
        {
            base.Build();

            assetNames = GetAssetNames();
            addressableNames = GetAddressableNames();
            lowerPackageName = PackageName.ToLower();

            BuildAssetBundle();
            BuildPackage();

            AssetDatabase.Refresh();
        }

        protected override void ValidateProperties()
        {
            base.ValidateProperties();

            if (string.IsNullOrWhiteSpace(AssetsFolder))
            {
                throw new ArgumentException(nameof(AssetsFolder));
            }
        }

        private void BuildAssetBundle()
        {
            var build = new AssetBundleBuild[1];
            build[0].assetBundleName = lowerPackageName;
            build[0].assetNames = assetNames;
            build[0].addressableNames = addressableNames;

            var buildOptions = BuildAssetBundleOptions.DisableLoadAssetByFileName
                | BuildAssetBundleOptions.DisableLoadAssetByFileNameWithExtension
                | BuildAssetBundleOptions.ForceRebuildAssetBundle;

            BuildPipeline.BuildAssetBundles(BuildDirectory, build, buildOptions, BuildTarget.StandaloneWindows64);

            string oldPath = Path.Combine(BuildDirectory, lowerPackageName);
            string newPath = Path.Combine(BuildDirectory, $"{lowerPackageName}.{Constants.AssetBundleFileExtension}");

            if (File.Exists(newPath))
            {
                File.Delete(newPath);
            }

            File.Move(oldPath, newPath);
        }

        private void BuildPackage()
        {
            var package = new Package()
            {
                Name = PackageName,
                Type = PackageType.AssetBundle,
            };

            PackageAssetEntry entry;
            for (int i = 0; i < assetNames.Length; i++)
            {
                entry = new PackageAssetEntry
                {
                    Key = addressableNames[i],
                    Type = AssetDatabase.GetMainAssetTypeAtPath(assetNames[i]),
                };

                package.Entries.Add(entry);
            }

            var json = JsonConvert.SerializeObject(package, Formatting.Indented);
            File.WriteAllText(Path.Combine(BuildDirectory, $"{lowerPackageName}.{Constants.PackageFileExtension}"), json);
        }

        private string[] GetAssetNames()
        {
            return AssetDatabase.FindAssets("", new[] { AssetsFolder })
                .Select(x => AssetDatabase.GUIDToAssetPath(x))
                .Distinct()
                .Where(x => AssetDatabase.GetMainAssetTypeAtPath(x) != typeof(DefaultAsset))
                .ToArray();
        }

        private string[] GetAddressableNames()
        {
            string[] addressableNames = new string[AssetsFolder.Trim('/').Length];

            for (int i = 0; i < assetNames.Length; i++)
            {
                string current = assetNames[i];

                current = current.Remove(0, AssetsFolder.Length + 1);
                current = Path.ChangeExtension(current, null);

                addressableNames[i] = current;
            }

            return addressableNames;
        }
    }
}
