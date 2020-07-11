﻿using System;
using System.Collections.Generic;
using System.Linq;
using Exanite.Arpg.DarkRift.Server;
using UnityEditor;
using UnityEngine;

namespace Exanite.Arpg.Editor.DarkRift
{
    [CustomEditor(typeof(XmlUnityServer))]
    [CanEditMultipleObjects]
    public class XmlUnityServerEditor : UnityEditor.Editor
    {
        private SerializedProperty configuration;
        private SerializedProperty createOnEnable;
        private SerializedProperty eventsFromDispatcher;

        private List<Type> pluginTypes = UnityServerHelper.SearchForPlugins().ToList();

        private void OnEnable()
        {
            configuration = serializedObject.FindProperty("configuration");
            createOnEnable = serializedObject.FindProperty("createOnEnable");
            eventsFromDispatcher = serializedObject.FindProperty("eventsFromDispatcher");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(configuration);
            EditorGUILayout.PropertyField(createOnEnable);

            //Alert to changes when this is unticked!
            bool old = eventsFromDispatcher.boolValue;
            EditorGUILayout.PropertyField(eventsFromDispatcher);

            if (eventsFromDispatcher.boolValue != old && !eventsFromDispatcher.boolValue)
            {
                eventsFromDispatcher.boolValue = !EditorUtility.DisplayDialog(
                    "Danger!",
                    "Unchecking " + eventsFromDispatcher.displayName + " will cause DarkRift to fire events from the .NET thread pool. unless you are confident using multithreading with Unity you should not disable this.\n\nAre you sure you want to proceed?",
                    "Yes",
                    "No (Save me!)"
                );
            }

            EditorGUILayout.Separator();

            if (pluginTypes.Count() > 0)
            {
                string pluginList = pluginTypes.Select(t => "\t\u2022 " + t.Name).Aggregate((a, b) => a + "\n" + b);

                EditorGUILayout.HelpBox("The following plugin types were found and will be loaded into the server:\n" + pluginList, MessageType.Info);
            }
            else
            {
                EditorGUILayout.HelpBox("No plugins were found to load!", MessageType.Info);
            }

            EditorGUILayout.Separator();

            if (GUILayout.Button("Open Configuration"))
            {
                if (configuration != null)
                    AssetDatabase.OpenAsset(configuration.objectReferenceValue);
                else
                    Debug.LogError("No configuration file specified!");
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
