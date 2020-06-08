using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEditor;

namespace Exanite.Arpg.AssetManagement.Packages
{
    public class PackageBuilder
    {
        [MenuItem("PackageBuilder/Test Build")]
        public static void TestBuild()
        {
            Build("examplemod", "Assets/Mods/ExampleMod/Assets", @"D:\Repositories\Exanite.Arpg\Assets\Mods\ExampleMod\Bundles");
        }

        public static void Build(string packageName, string assetFolder, string buildDirectory)
        {
            packageName = packageName.ToLower();
            var assetNames = GetAssetNamesForDirectory(assetFolder);
            var addressableNames = FormatAddressableNames(assetFolder, assetNames);
            var assetBundleInfo = BuildAssetBundleInfo(assetNames, addressableNames);

            // build assetbundle

            var build = new AssetBundleBuild[1];
            build[0].assetBundleName = packageName;
            build[0].assetNames = assetNames;
            build[0].addressableNames = addressableNames;

            var buildOptions = BuildAssetBundleOptions.DisableLoadAssetByFileName
                | BuildAssetBundleOptions.DisableLoadAssetByFileNameWithExtension;

            BuildPipeline.BuildAssetBundles(buildDirectory, build, buildOptions, BuildTarget.StandaloneWindows64);

            string oldPath = Path.Combine(buildDirectory, packageName);
            string newPath = Path.Combine(buildDirectory, $"{packageName}.{Constants.AssetBundleFileExtension}");

            if (File.Exists(newPath))
            {
                File.Delete(newPath);
            }

            File.Move(oldPath, newPath);

            // build packageinfo

            var json = JsonConvert.SerializeObject(assetBundleInfo, Formatting.Indented);
            File.WriteAllText(Path.Combine(buildDirectory, $"{packageName}.{Constants.PackageInfoFileExtension}"), json);

            AssetDatabase.Refresh();
        }

        private static string[] GetAssetNamesForDirectory(string assetFolder)
        {
            return AssetDatabase.FindAssets("", new[] { assetFolder })
                .Select(x => AssetDatabase.GUIDToAssetPath(x))
                .Distinct()
                .Where(x => AssetDatabase.GetMainAssetTypeAtPath(x) != typeof(DefaultAsset))
                .ToArray();
        }

        private static string[] FormatAddressableNames(string assetFolder, string[] assetNames)
        {
            assetFolder = assetFolder.Trim('/');

            string[] addressableNames = new string[assetNames.Length];

            for (int i = 0; i < assetNames.Length; i++)
            {
                string current = assetNames[i];

                current = current.Remove(0, assetFolder.Length + 1);
                current = Path.ChangeExtension(current, null);

                addressableNames[i] = current;
            }

            return addressableNames;
        }

        private static PackageInfo BuildAssetBundleInfo(string[] assetNames, string[] addressableNames)
        {
            if (assetNames.Length != addressableNames.Length)
            {
                throw new ArgumentException($"'{nameof(assetNames)}' and '{nameof(addressableNames)}' must have the same length");
            }

            var result = new PackageInfo(assetNames.Length);
            PackageAssetEntry entry;

            for (int i = 0; i < assetNames.Length; i++)
            {
                entry = new PackageAssetEntry
                {
                    Key = addressableNames[i],
                    AssetType = AssetDatabase.GetMainAssetTypeAtPath(assetNames[i]),
                };

                result.Entries.Add(entry);
            }

            return result;
        }
    }
}