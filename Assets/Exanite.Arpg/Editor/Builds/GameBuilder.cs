using System;
using System.IO;
using Exanite.Arpg.Versioning.Internal;
using UnityEditor;
using UnityEngine;

namespace Exanite.Arpg.Editor.Builds
{
    /// <summary>
    /// Used to build the game
    /// </summary>
    public static class GameBuilder
    {
        /// <summary>
        /// The default version<para/>
        /// Note: This is also used when the Git commit version generation fails (Git was not found in Path)
        /// </summary>
        public const string DefaultVersion = "branch/0.0.0.0";

        /// <summary>
        /// Folder that builds are built to
        /// </summary>
        public const string BuildPath = "Builds";

        /// <summary>
        /// Folder the Client build will be built to
        /// </summary>
        public const string ClientFolderName = "Client";
        /// <summary>
        /// Name of the Client executable file
        /// </summary>
        public const string ClientExecutableName = "Exanite.Arpg";

        /// <summary>
        /// Folder the Server build will be built to
        /// </summary>
        public const string ServerFolderName = "Server";
        /// <summary>
        /// Name of the Server executable file
        /// </summary>
        public const string ServerExecutableName = "Exanite.Arpg-Server";

        /// <summary>
        /// Builds a Client and Server using the active build target and the scenes defined in the Build Settings Unity menu
        /// </summary>
        public static void BuildClientAndServer()
        {
            BuildClient();
            BuildServer();
        }

        /// <summary>
        /// Builds a Client using the active build target and the scenes defined in the Build Settings Unity menu
        /// </summary>
        public static void BuildClient()
        {
            Debug.Log("Building Client");
            SetBuildVersion();

            string version = PlayerSettings.bundleVersion;
            var target = EditorUserBuildSettings.activeBuildTarget;

            var options = new BuildPlayerOptions()
            {
                target = target,
                locationPathName = GetBuildPath(false, target),

                scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes),
            };

            BuildPipeline.BuildPlayer(options);

            ResetBuildVersion();
            Debug.Log($"Finished building Client with version '{version}'");
        }

        /// <summary>
        /// Builds a Server using the active build target and the scenes defined in the Build Settings Unity menu
        /// </summary>
        public static void BuildServer()
        {
            Debug.Log("Building Server");
            SetBuildVersion();

            string version = PlayerSettings.bundleVersion;
            var target = EditorUserBuildSettings.activeBuildTarget;

            var options = new BuildPlayerOptions()
            {
                target = target,
                locationPathName = GetBuildPath(true, target),

                scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes),
                options = BuildOptions.EnableHeadlessMode,
            };

            BuildPipeline.BuildPlayer(options);

            ResetBuildVersion();
            Debug.Log($"Finished building Server with version '{version}'");
        }

        /// <summary>
        /// Generates and returns the current build version<para/>
        /// Format: branch/0.0.0.0
        /// </summary>
        public static string GenerateBuildVersion()
        {
            try
            {
                return $"{Git.GetBranchNameGithubActionsFix()}/{Git.GenerateCommitVersion()}";
            }
            catch (GitException e)
            {
                Debug.LogWarning($"Failed to generate build version\n{e}");

                return DefaultVersion;
            }
        }

        private static void SetBuildVersion()
        {
            PlayerSettings.bundleVersion = GenerateBuildVersion();
        }

        private static void ResetBuildVersion()
        {
            PlayerSettings.bundleVersion = DefaultVersion;

            AssetDatabase.SaveAssets();
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
