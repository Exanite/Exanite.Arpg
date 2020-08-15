using System.Collections.Generic;
using Prototype.Networking.Players;
using UnityEditor;
using UnityEngine;

namespace Prototype.Editor
{
    [CustomEditor(typeof(ServerPlayerManager))]
    public class ServerPlayerManagerEditor : TypedEditor<ServerPlayerManager>
    {
        private bool isPlayersFoldoutOpen = false;
        private HashSet<Player> openPlayerFoldouts = new HashSet<Player>();

        public override bool RequiresConstantRepaint()
        {
            return true;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.IntField("Player Count", TypedTarget.PlayerCount);

            if (TypedTarget.PlayerCount > 0)
            {
                DrawPlayerList();
            }
        }

        private void DrawPlayerList()
        {
            isPlayersFoldoutOpen = EditorGUILayout.Foldout(isPlayersFoldoutOpen, "Players");
            if (isPlayersFoldoutOpen)
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    foreach (var player in TypedTarget.Players)
                    {
                        DrawPlayer(player);
                    }
                }
            }
        }

        private void DrawPlayer(ServerPlayer player)
        {
            if (DrawPlayerFoldout(player))
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    EditorGUILayout.Toggle("Is loading zone:", player.IsLoadingZone);
                    EditorGUILayout.TextField("Current zone:", player.CurrentZone.guid.ToString());

                    if (DrawButton("Disconnect"))
                    {
                        player.Connection.Peer.Disconnect();
                    }
                }
            }
        }

        private bool DrawPlayerFoldout(ServerPlayer player)
        {
            bool openFoldout = EditorGUILayout.Foldout(openPlayerFoldouts.Contains(player), $"Player: {player.Id.ToString()}");

            if (openFoldout)
            {
                openPlayerFoldouts.Add(player);
            }
            else
            {
                openPlayerFoldouts.Remove(player);
            }

            return openFoldout;
        }

        private bool DrawButton(string text)
        {
            bool result;

            EditorGUILayout.BeginHorizontal();
            {
                var rect = EditorGUI.IndentedRect(EditorGUILayout.GetControlRect());

                result = GUI.Button(rect, text);
            }
            EditorGUILayout.EndHorizontal();

            return result;
        }
    }
}
