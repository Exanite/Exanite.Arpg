using Exanite.Arpg.Logging;
using Exanite.Arpg.Versioning.Internal;
using UnityEngine;
using Zenject;

namespace Exanite.Arpg.Versioning
{
    /// <summary>
    /// Contains information about the game and its current version
    /// </summary>
    public class GameVersion : MonoBehaviour
    {
        private string company;
        private string product;

        private string version;
        private string branch;
        private string number;

        private ILog log;

        [Inject]
        public void Inject(ILog log)
        {
            this.log = log.ForContext<GameVersion>();

            GetGameInfo();

            LogCurrentVersion();
        }

        /// <summary>
        /// Game company name
        /// </summary>
        public string Company
        {
            get
            {
                return company;
            }

            set
            {
                company = value;
            }
        }

        /// <summary>
        /// Game product name
        /// </summary>
        public string Product
        {
            get
            {
                return product;
            }

            set
            {
                product = value;
            }
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

        private void GetGameInfo()
        {
            GetProductInfo();
            GetVersionInfo();
        }

        private void GetProductInfo()
        {
            company = Application.companyName;
            product = Application.productName;
        }

        private void GetVersionInfo()
        {
            if (Application.isEditor)
            {
                try
                {
                    version = $"{Git.GetBranchName()}/{Git.GenerateCommitVersion()}";
                }
                catch (GitException e)
                {
                    log.Error(e, "Failed to generate build version");
                }
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
