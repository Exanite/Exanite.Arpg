using Prototype.Networking.Players;
using UnityEditor;

namespace Prototype.Editor
{
    [CustomEditor(typeof(ServerPlayerManager))]
    public class ServerPlayerManagerEditor : TypedEditor<ServerPlayerManager>
    {
        public bool isPlayersFoldoutOpen = false;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.IntField("Player Count", TypedTarget.PlayerCount);

            if (TypedTarget.PlayerCount > 0)
            {
                isPlayersFoldoutOpen = EditorGUILayout.Foldout(isPlayersFoldoutOpen, "Players");
                if (isPlayersFoldoutOpen)
                {
                    EditorGUI.indentLevel++;

                    foreach (var player in TypedTarget.Players)
                    {
                        EditorGUILayout.TextField("Name", player.Id.ToString());
                    }

                    EditorGUI.indentLevel--;
                }
            }
        }
    } 
}
