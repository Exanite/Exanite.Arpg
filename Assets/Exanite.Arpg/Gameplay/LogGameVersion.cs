using UnityEngine;
using Zenject;
using ILogger = Serilog.ILogger;

namespace Exanite.Arpg.Gameplay
{
    public class LogGameVersion : MonoBehaviour
    {
        private ILogger log;

        [Inject]
        public void Inject(ILogger log)
        {
            this.log = log.ForContext<LogGameVersion>();

            LogCurrentVersion();
        }

        private void LogCurrentVersion()
        {
            log.Information("The current version of the game is {Version}", Application.version);
        }
    } 
}
