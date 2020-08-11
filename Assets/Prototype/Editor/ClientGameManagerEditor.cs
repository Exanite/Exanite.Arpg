using Prototype.Networking.Client;
using UnityEditor;
using UnityEngine;

namespace Prototype.Editor
{
    [CustomEditor(typeof(ClientGameManager))]
    public class ClientGameManagerEditor : TypedEditor<ClientGameManager>
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Disconnect"))
            {
                TypedTarget.Disconnect();
            }
        }
    } 
}
