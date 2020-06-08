using System.IO;
using System.Linq;
using Exanite.Arpg.AssetManagement.Packages;
using Newtonsoft.Json;
using UnityEditor;

namespace Exanite.Arpg.AssetManagement.Editor
{
    public class AssetBundlePackageBuilder : PackageBuilder
    {
        public string assetsFolder;

        private string[] assetNames;
        private string[] addressableNames;

        private string lowerPackageName;

        [MenuItem("PackageBuilder/Build AssetBundlePackage (Test)")]
        public static void TestBuild()
        {
            new AssetBundlePackageBuilder
            {
                packageName = "ExampleMod",
                assetsFolder = "Assets/Mods/ExampleMod/Assets",
                buildDirectory = @"D:\Repositories\Exanite.Arpg\Assets\Mods\ExampleMod\Bundles",
            }
            .Build();
        }

        public override void Build()
        {
            assetNames = GetAssetNames();
            addressableNames = GetAddressableNames();
            lowerPackageName = packageName.ToLower();

            BuildAssetBundle();
            BuildPackage();

            AssetDatabase.Refresh();
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

            BuildPipeline.BuildAssetBundles(buildDirectory, build, buildOptions, BuildTarget.StandaloneWindows64);

            string oldPath = Path.Combine(buildDirectory, lowerPackageName);
            string newPath = Path.Combine(buildDirectory, $"{lowerPackageName}.{Constants.AssetBundleFileExtension}");

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
                Name = packageName,
                Type = PackageType.AssetBundle,
            };

            PackageAssetEntry entry;
            for (int i = 0; i < assetNames.Length; i++)
            {
                entry = new PackageAssetEntry
                {
                    Key = addressableNames[i],
                    AssetType = AssetDatabase.GetMainAssetTypeAtPath(assetNames[i]),
                };

                package.Entries.Add(entry);
            }

            var json = JsonConvert.SerializeObject(package, Formatting.Indented);
            File.WriteAllText(Path.Combine(buildDirectory, $"{lowerPackageName}.{Constants.PackageFileExtension}"), json);
        }

        private string[] GetAssetNames()
        {
            return AssetDatabase.FindAssets("", new[] { assetsFolder })
                .Select(x => AssetDatabase.GUIDToAssetPath(x))
                .Distinct()
                .Where(x => AssetDatabase.GetMainAssetTypeAtPath(x) != typeof(DefaultAsset))
                .ToArray();
        }

        private string[] GetAddressableNames()
        {
            string[] addressableNames = new string[assetsFolder.Trim('/').Length];

            for (int i = 0; i < assetNames.Length; i++)
            {
                string current = assetNames[i];

                current = current.Remove(0, assetsFolder.Length + 1);
                current = Path.ChangeExtension(current, null);

                addressableNames[i] = current;
            }

            return addressableNames;
        }
    }
}
