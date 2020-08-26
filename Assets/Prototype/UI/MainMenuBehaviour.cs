using Exanite.Arpg.Versioning;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class MainMenuBehaviour : MonoBehaviour
{
    public string username = "Player";
    public string address = "127.0.0.1";
    public string port = "17175";

    private UIDocument document;
    private VisualElement root;
    private EventSystem eventSystem;

    private GameVersion gameVersion;

    [Inject]
    public void Inject(GameVersion gameVersion)
    {
        this.gameVersion = gameVersion;
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
        Debug.Log("Start Client button activated");
    }

    private void OnStartServerButtonActivated(ClickEvent e)
    {
        Debug.Log("Start Server button activated");
    }

    private void OnStartClientServerButtonActivated(ClickEvent e)
    {
        Debug.Log("Start Client and Server button activated");
    }
}
