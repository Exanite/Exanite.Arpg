using Exanite.Arpg.Logging;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class PhysicsSceneManager : MonoBehaviour
{
    public string clientSceneName = "Client";
    public string serverSceneName = "Server";

    public PhysicsScene clientPhysicsScene;
    public PhysicsScene serverPhysicsScene;

    private Scene clientScene;
    private Scene serverScene;

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

        clientScene = SceneManager.LoadScene(clientSceneName, loadSceneParameters);
        serverScene = SceneManager.LoadScene(serverSceneName, loadSceneParameters);

        clientPhysicsScene = clientScene.GetPhysicsScene();
        serverPhysicsScene = serverScene.GetPhysicsScene();
    }

    private void FixedUpdate()
    {
        clientPhysicsScene.Simulate(Time.fixedDeltaTime);
        serverPhysicsScene.Simulate(Time.fixedDeltaTime);
    }
}
