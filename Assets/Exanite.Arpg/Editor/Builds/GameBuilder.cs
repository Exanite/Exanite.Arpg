using System;
using System.IO;
using UnityEditor;

namespace Exanite.Arpg.Editor.Builds
{
    public static class GameBuilder
    {
        public const string BuildPath = "Builds";

        public const string ClientFolderName = "Client";
        public const string ClientExecutableName = "Exanite.Arpg";

        public const string ServerFolderName = "Server";
        public const string ServerExecutableName = "Exanite.Arpg-Server";

        public static void BuildClient()
        {
            var target = EditorUserBuildSettings.activeBuildTarget;

            var options = new BuildPlayerOptions()
            {
                target = target,
                locationPathName = GetBuildPath(false, target),

                scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes),
            };

            BuildPipeline.BuildPlayer(options);
        }

        public static void BuildServer()
        {
            var target = EditorUserBuildSettings.activeBuildTarget;

            var options = new BuildPlayerOptions()
            {
                target = target,
                locationPathName = GetBuildPath(true, target),

                scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes),
                options = BuildOptions.EnableHeadlessMode,
            };

            BuildPipeline.BuildPlayer(options);
        }

        private static string GetBuildPath(bool isServer, BuildTarget target)
        {
            string folderName = isServer ? ServerFolderName : ClientFolderName;
            string executableName = isServer ? ServerExecutableName : ClientExecutableName;

            return Path.Combine(BuildPath, $"{folderName}-{target}", $"{executableName}{GetExtension(target)}");
        }

        private static string GetExtension(BuildTarget target)
        {
            string extension;

            switch (target)
            {
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    extension = ".exe";
                    break;
                case BuildTarget.StandaloneOSX:
                    extension = ".app";
                    break;
                case BuildTarget.StandaloneLinux64:
                    extension = ".x64";
                    break;
                default: throw new NotSupportedException($"{target} is not a supported {typeof(BuildTarget).Name}");
            }

            return extension;
        }
    }
}
