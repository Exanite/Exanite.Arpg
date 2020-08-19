// Notice:
// This code has been modified for use in Exanite.Arpg
//
// The original code can be found at the link below under the MIT License:
// Code: https://github.com/webbertakken/unity-builder/tree/master/action/default-build-script/Assets/Editor/System
// License: https://github.com/webbertakken/unity-builder/blob/master/LICENSE

using System.Diagnostics;
using System.Text;

namespace Exanite.Arpg.Editor
{
    public static class ProcessExtensions
    {
        /// <summary>
        /// Executes an application with given arguments
        /// </summary>
        /// <param name="process">The <see cref="Process"/> to use</param>
        /// <param name="application">Application for the <see cref="Process"/> to run</param>
        /// <param name="arguments">Arguments to supply to the <see cref="Process"/></param>
        /// <param name="workingDirectory">Directory the <see cref="Process"/> will run in</param>
        /// <param name="output"><see cref="string"/> containing output from the <see cref="Process"/></param>
        /// <param name="errors"><see cref="string"/> containing errors from the <see cref="Process"/></param>
        /// <returns>The process exit code</returns>
        public static int Run(this Process process,
            string application, string arguments, string workingDirectory,
            out string output, out string errors)
        {
            // Configure how to run the application
            process.StartInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                FileName = application,
                Arguments = arguments,
                WorkingDirectory = workingDirectory
            };

            // Read the output
            var outputBuilder = new StringBuilder();
            var errorsBuilder = new StringBuilder();
            process.OutputDataReceived += (_, args) => outputBuilder.AppendLine(args.Data);
            process.ErrorDataReceived += (_, args) => errorsBuilder.AppendLine(args.Data);

            // Run the application and wait for it to complete
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            // Format the output
            output = outputBuilder.ToString().TrimEnd();
            errors = errorsBuilder.ToString().TrimEnd();

            return process.ExitCode;
        }
    }
}
