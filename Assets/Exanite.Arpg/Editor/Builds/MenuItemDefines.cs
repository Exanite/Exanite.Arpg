using Exanite.Arpg.Editor.Builds.Versioning;
using UnityEditor;
using UnityEngine;

namespace Exanite.Arpg.Editor.Builds
{
    /// <summary>
    /// <see cref="MenuItem"/> defines for methods in the Exanite.Arpg.Editor.Builds namespace
    /// </summary>
    public static class MenuItemDefines
    {
        [MenuItem("Build/Build Client", priority = 0)]
        public static void BuildClient()
        {
            GameBuilder.BuildClient();
        }

        [MenuItem("Build/Build Server", priority = 1)]
        public static void BuildServer()
        {
            GameBuilder.BuildServer();
        }

        [MenuItem("Build/Build Client and Server", priority = 2)]
        public static void BuildClientAndServer()
        {
            GameBuilder.BuildClientAndServer();
        }

        [MenuItem("Build/Open Builds folder", priority = 100)]
        public static void OpenBuildsFolder()
        {
            Application.OpenURL(GameBuilder.BuildPath);
        }

        [MenuItem("Build/Log current build version", priority = 200)]
        private static void LogBuildVersion()
        {
            string version = Git.GenerateCommitVersion();

            Debug.Log($"The current version of the game is '{version}'");
        }
    }
}
