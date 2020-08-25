// Notice:
// This code has been modified for use in Exanite.Arpg
//
// The original code can be found at the link below under the MIT License:
// Code: https://github.com/webbertakken/unity-builder/tree/master/action/default-build-script/Assets/Editor/Versioning
// License: https://github.com/webbertakken/unity-builder/blob/master/LICENSE

using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Exanite.Arpg.Versioning.Internal
{
    /// <summary>
    /// Used to run Git commands in C#
    /// </summary>
    public static class Git
    {
        public const string Application = @"git";

        /// <summary>
        /// Generates a version based on the latest tag and the amount of commits<para/>
        /// Format: 0.1.2.3 (where 3 is the amount of commits)
        /// </summary>
        public static string GenerateCommitVersion()
        {
            // If no tag is present in the repository then v0.0.0 is assumed
            // This would result in 0.0.0.# where # is the amount of commits

            string version;

            if (HasAnyVersionTags())
            {
                version = GetCommitInfo();
            }
            else
            {
                version = $"0.0.0.{GetTotalNumberOfCommits()}";
            }

            return version;
        }

        /// <summary>
        /// Gets the current checked out branch's name<para/>
        /// This is for retrieving the correct branch name during Github Actions CI, but can be used normally
        /// </summary>
        public static string GetBranchNameGithubActionsFix()
        {
            string headRef = Environment.GetEnvironmentVariable("GITHUB_HEAD_REF");

            if (!string.IsNullOrWhiteSpace(headRef))
            {
                return headRef;
            }

            return GetBranchName();
        }

        /// <summary>
        /// Gets the current checked out branch's name
        /// </summary>
        public static string GetBranchName()
        {
            return Run(@"name-rev --name-only HEAD");
        }

        /// <summary>
        /// Gets the total number of commits
        /// </summary>
        public static int GetTotalNumberOfCommits()
        {
            string numberOfCommitsAsString = Run(@"rev-list --count HEAD");

            return int.Parse(numberOfCommitsAsString);
        }

        /// <summary>
        /// Retrieves the build version from git based on the most recent matching tag and commit history<para/>
        /// Format: 0.1.2.3 (where 3 is the amount of commits)
        /// </summary>
        public static string GetCommitInfo()
        {
            // v0.1-2-g12345678 (where 2 is the amount of commits, g stands for git)
            string version = GetVersionString();
            // 0.1-2
            version = version.Substring(1, version.LastIndexOf('-') - 1);
            // 0.1.2
            version = version.Replace('-', '.');

            return version;
        }

        /// <summary>
        /// Gets version string<para/>
        /// Format: v0.1-2-g12345678 (where 2 is the amount of commits since the last tag)
        /// </summary>
        public static string GetVersionString()
        {
            // Reference: https://softwareengineering.stackexchange.com/questions/141973/how-do-you-achieve-a-numeric-versioning-scheme-with-git

            return Run(@"describe --tags --long --match ""v[0-9]*""");
        }

        /// <summary>
        /// Whether or not the repository has any version tags yet
        /// </summary>
        public static bool HasAnyVersionTags()
        {
            string output = Run(@"tag --list --merged HEAD");
            Regex regex = new Regex("v[0-9]*");

            var matches = regex.Matches(output);

            return matches.Count > 0;
        }

        /// <summary>
        /// Runs git binary with any given arguments and returns the output.
        /// </summary>
        public static string Run(string arguments)
        {
            using (var process = new Process())
            {
                string workingDirectory = UnityEngine.Application.dataPath;
                int exitCode = process.Run(Application, arguments, workingDirectory, out string output, out string errors);

                if (exitCode != 0)
                {
                    throw new GitException(exitCode, errors);
                }

                return output;
            }
        }
    }
}
