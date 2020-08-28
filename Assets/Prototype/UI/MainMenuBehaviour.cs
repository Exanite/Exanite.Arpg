using System;
using System.Net;
using Exanite.Arpg;
using Exanite.Arpg.Logging;
using Exanite.Arpg.Versioning;
using Prototype.Networking.Startup;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace Prototype.UI
{
    public class MainMenuBehaviour : MonoBehaviour
    {
        public string username = "Player";
        public string address = "127.0.0.1";
        public string port = "17175";

        private UIDocument document;
        private VisualElement root;
        private EventSystem eventSystem;

        private ILog log;
        private GameVersion gameVersion;
        private SceneLoader sceneLoader;

        [Inject]
        public void Inject(ILog log, GameVersion gameVersion, SceneLoader sceneLoader)
        {
            this.log = log;
            this.gameVersion = gameVersion;
            this.sceneLoader = sceneLoader;
        }

        private void Start()
        {
            document = GetComponent<UIDocument>();
            root = document.rootVisualElement;

            DisplayVersion();

            RegisterTextFields();
            RegisterButtons();
        }

        private void DisplayVersion()
        {
            root.Query<Label>("version").ForEach((label) =>
            {
                label.text = $"{gameVersion.Company}.{gameVersion.Product} {gameVersion.Version}";
            });
        }

        private void RegisterTextFields()
        {
            root.Query<TextField>().ForEach((field) =>
            {
                switch (field.name)
                {
                    case "username-field":
                        field.value = username;
                        field.RegisterValueChangedCallback(OnUsernameFieldChanged); break;

                    case "address-field":
                        field.value = address;
                        field.RegisterValueChangedCallback(OnAddressFieldChanged); break;

                    case "port-field":
                        field.value = port;
                        field.RegisterValueChangedCallback(OnPortFieldChanged); break;

                    default: break;
                }
            });
        }

        private void RegisterButtons()
        {
            root.Query<Button>().ForEach((button) =>
            {
                switch (button.name)
                {
                    case "start-client-button":
                        button.RegisterCallback<ClickEvent>(OnStartClientButtonActivated); break;

                    case "start-server-button":
                        button.RegisterCallback<ClickEvent>(OnStartServerButtonActivated); break;

                    case "start-client-server-button":
                        button.RegisterCallback<ClickEvent>(OnStartClientServerButtonActivated); break;

                    default: break;
                }
            });
        }

        private void OnUsernameFieldChanged(ChangeEvent<string> e)
        {
            username = e.newValue;

            root.Query<TextField>("username-field").ForEach((field) =>
            {
                field.SetValueWithoutNotify(username);
            });
        }

        private void OnAddressFieldChanged(ChangeEvent<string> e)
        {
            address = e.newValue;

            root.Query<TextField>("address-field").ForEach((field) =>
            {
                field.SetValueWithoutNotify(address);
            });
        }

        private void OnPortFieldChanged(ChangeEvent<string> e)
        {
            port = e.newValue;

            root.Query<TextField>("port-field").ForEach((field) =>
            {
                field.SetValueWithoutNotify(port);
            });
        }

        private void OnStartClientButtonActivated(ClickEvent e)
        {
            log.Debug("Start Client button activated");

            StartClient();

            gameObject.SetActive(false);
        }

        private void OnStartServerButtonActivated(ClickEvent e)
        {
            log.Debug("Start Server button activated");

            StartServer();

            gameObject.SetActive(false);
        }

        private void OnStartClientServerButtonActivated(ClickEvent e)
        {
            log.Debug("Start Client and Server button activated");

            StartServer();

            for (int i = 0; i < 10; i++)
            {
                StartClient();
            }

            gameObject.SetActive(false);
        }

        private void StartClient()
        {
            var settings = new GameStartSettings(GameStartSettings.GameType.Client, GetUsername(), GetAddress(), GetPort());

            Action<DiContainer> bindings = (container) =>
            {
                container.Bind<GameStartSettings>().FromInstance(settings).AsSingle();
            };

            sceneLoader.LoadAdditiveSceneAsync("Client", gameObject.scene, bindings);
        }

        private void StartServer()
        {
            var settings = new GameStartSettings(GameStartSettings.GameType.Server, GetUsername(), GetAddress(), GetPort());

            Action<DiContainer> bindings = (container) =>
            {
                container.Bind<GameStartSettings>().FromInstance(settings).AsSingle();
            };

            sceneLoader.LoadAdditiveSceneAsync("Server", gameObject.scene, bindings);
        }

        private string GetUsername()
        {
            return username;
        }

        private ushort GetPort()
        {
            return ushort.Parse(port);
        }

        private IPAddress GetAddress()
        {
            return IPAddress.Parse(address);
        }

    }
}
