using Exanite.Arpg.Logging;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Prototype.Networking.Startup
{
    public class GameLoader : MonoBehaviour
    {
        public string serverSceneName = "Server";
        public string clientSceneName = "Client";

        public int numberOfClients = 1;

        private ILog log;

        [Inject]
        public void Inject(ILog log)
        {
            this.log = log;
        }

        private void Awake()
        {
            log.Information("Disabling physics auto-simulation");

            Physics.autoSimulation = false;
        }

        private void Start()
        {
            CreateServer();

            for (int i = 0; i < numberOfClients; i++)
            {
                CreateClient();
            }
        }

        public void CreateServer()
        {
            var loadSceneParameters = new LoadSceneParameters(LoadSceneMode.Additive, LocalPhysicsMode.Physics3D);

            SceneManager.LoadScene(serverSceneName, loadSceneParameters);
        }

        [ContextMenu("Create new Client")]
        public void CreateClient()
        {
            var loadSceneParameters = new LoadSceneParameters(LoadSceneMode.Additive, LocalPhysicsMode.Physics3D);

            SceneManager.LoadScene(clientSceneName, loadSceneParameters);
        }
    }
}
