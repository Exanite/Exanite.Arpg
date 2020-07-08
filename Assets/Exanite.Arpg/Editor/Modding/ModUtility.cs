//using System.IO;
//using Exanite.Core.Extensions;
//using UnityEditor;

//namespace Exanite.Arpg.Editor.Modding
//{
//    public static class ModUtility
//    {
//        public const string DefaultModBuildPath = "Builds/Mods/";

//        public static void SetUpModFolders(ModInfo mod)
//        {
//            if (!TryGetModPath(mod, out string modPath))
//            {
//                return;
//            }

//            if (string.IsNullOrWhiteSpace(modPath))
//            {
//                return;
//            }

//            DirectoryInfo sourceCodeDirectory = new DirectoryInfo(Path.Combine(modPath, ModInfo.SourceCodeFolderName));
//            DirectoryInfo assetsDirectory = new DirectoryInfo(Path.Combine(modPath, ModInfo.AssetsFolderName));

//            sourceCodeDirectory.Create();
//            assetsDirectory.Create();

//            AssetDatabase.Refresh();
//        }

//        public static void Build(ModInfo mod, string buildPath, bool overwrite = false)
//        {
//            DirectoryInfo buildDirectory = new DirectoryInfo(buildPath);

//            if (!buildDirectory.IsEmpty() && overwrite)
//            {
//                foreach (var file in buildDirectory.EnumerateFiles())
//                {
//                    file.Delete();
//                }

//                foreach (var directory in buildDirectory.EnumerateDirectories())
//                {
//                    directory.Delete();
//                }
//            }
//            else
//            {
//                return;
//            }

//            buildDirectory.Create();
//        }

//        public static bool TryGetModPath(ModInfo mod, out string path)
//        {
//            string assetPath = AssetDatabase.GetAssetPath(mod);

//            if (string.IsNullOrWhiteSpace(assetPath))
//            {
//                path = null;
//                return false;
//            }

//            path = Path.GetDirectoryName(assetPath);
//            return true;
//        }

//        public static void CreateDefaultModBuildPath()
//        {
//            DirectoryInfo buildDirectory = new DirectoryInfo(DefaultModBuildPath);
//            buildDirectory.Create();
//        }
//    }
//}
