﻿using Prototype.LiteNetLib.Players;
using UnityEditor;

[CustomEditor(typeof(PlayerManager))]
public class PlayerManagerEditor : Editor
{
    public PlayerManager TypedTarget;

    public bool isPlayersFoldoutOpen = false;

    private void OnEnable()
    {
        TypedTarget = (PlayerManager)target;
    }

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
