// Notice:
// This code has been modified for use in Exanite.Arpg
//
// The original code can be found at the link below under the MIT License:
// Code: https://github.com/webbertakken/unity-builder/tree/master/action/default-build-script/Assets/Editor/Versioning
// License: https://github.com/webbertakken/unity-builder/blob/master/LICENSE

using System;

namespace Exanite.Arpg.Editor.Builds.Versioning
{
    /// <summary>
    /// The exception that is thrown when Git fails to exit successfully
    /// </summary>
    public class GitException : InvalidOperationException
    {
        private readonly int exitCode;

        /// <summary>
        /// Creates a new <see cref="GitException"/>
        /// </summary>
        public GitException(int exitCode, string errors) : base($"\n{errors}")
        {
            this.exitCode = exitCode;
        }

        /// <summary>
        /// Exit code specified by Git
        /// </summary>
        public int ExitCode
        {
            get
            {
                return exitCode;
            }
        }
    }
}
