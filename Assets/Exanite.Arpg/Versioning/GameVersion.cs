using Exanite.Arpg.Logging;
using Exanite.Arpg.Versioning.Internal;
using UnityEngine;
using Zenject;

namespace Exanite.Arpg.Versioning
{
    /// <summary>
    /// Used to log the current game version
    /// </summary>
    public class GameVersion : MonoBehaviour
    {
        private string version;
        private string branch;
        private string number;

        private ILog log;

        [Inject]
        public void Inject(ILog log)
        {
            this.log = log.ForContext<GameVersion>();

            GetVersion();

            LogCurrentVersion();
        }

        /// <summary>
        /// The version of the game<para/>
        /// Format: branch/0.0.0.0
        /// </summary>
        public string Version
        {
            get
            {
                return version;
            }
        }

        /// <summary>
        /// The branch the game is built on<para/>
        /// Format: branch
        /// </summary>
        public string Branch
        {
            get
            {
                return branch;
            }
        }

        /// <summary>
        /// The version number of the game<para/>
        /// Format: 0.0.0.0
        /// </summary>
        public string Number
        {
            get
            {
                return number;
            }
        }

        /// <summary>
        /// Logs the current game version
        /// </summary>
        public void LogCurrentVersion()
        {
            log.Information("The current version of the game is '{Version}'", Version);
        }

        private void GetVersion()
        {
            if (Application.isEditor)
            {
                version = $"{Git.GetBranchName()}/{Git.GenerateCommitVersion()}";
            }
            else
            {
                version = Application.version;
            }

            int index = version.LastIndexOf('/');

            branch = version.Substring(0, index);
            number = version.Substring(index + 1);
        }
    }
}
