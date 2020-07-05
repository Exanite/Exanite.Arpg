using UnityEngine;
using Zenject;
using ILogger = Serilog.ILogger;

namespace Exanite.Arpg.Gameplay
{
    /// <summary>
    /// Used to log the current game version
    /// </summary>
    public class LogGameVersion : MonoBehaviour
    {
        private ILogger log;

        [Inject]
        public void Inject(ILogger log)
        {
            this.log = log.ForContext<LogGameVersion>();

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