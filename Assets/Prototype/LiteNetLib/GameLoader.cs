using Exanite.Arpg.Logging;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

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
        var loadSceneParameters = new LoadSceneParameters(LoadSceneMode.Additive, LocalPhysicsMode.Physics3D);

        SceneManager.LoadScene(serverSceneName, loadSceneParameters);

        for (int i = 0; i < numberOfClients; i++)
        {
            SceneManager.LoadScene(clientSceneName, loadSceneParameters);
        }
    }
}
