using Exanite.Arpg.Logging;
using UnityEngine;
using Zenject;

namespace Exanite.Arpg.Versioning
{
    /// <summary>
    /// Used to log the current game version
    /// </summary>
    public class GameVersion : MonoBehaviour
    {
        private ILog log;

        [Inject]
        public void Inject(ILog log)
        {
            this.log = log.ForContext<GameVersion>();

            LogCurrentVersion();
        }

        /// <summary>
        /// Logs the current game version
        /// </summary>
        public void LogCurrentVersion()
        {
            log.Information("The current version of the game is '{Version}'", Application.version);
        }
    }
}