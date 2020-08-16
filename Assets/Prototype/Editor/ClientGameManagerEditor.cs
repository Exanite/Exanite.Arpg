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

            EditorGUILayout.IntField("Id", TypedTarget.localPlayer?.Id ?? -1);
            EditorGUILayout.TextField("Current zone", TypedTarget.localPlayer?.CurrentZone?.guid.ToString());

            if (GUILayout.Button("Disconnect"))
            {
                TypedTarget.Disconnect();
            }
        }
    } 
}
