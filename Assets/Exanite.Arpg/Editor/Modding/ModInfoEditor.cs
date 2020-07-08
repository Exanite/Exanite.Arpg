//using System;
//using System.IO;
//using Exanite.Core.Extensions;
//using Sirenix.OdinInspector.Editor;
//using UnityEditor;
//using UnityEngine;

//namespace Exanite.Arpg.Editor.Modding
//{
//    [CustomEditor(typeof(ModInfo))]
//    public class ModInfoEditor : OdinEditor
//    {
//        public ModInfo TypedTarget;

//        protected override void OnEnable()
//        {
//            TypedTarget = target as ModInfo;
//        }

//        public override void OnInspectorGUI()
//        {
//            base.OnInspectorGUI();

//            EditorGUILayout.BeginVertical();
//            {
//                if (AssetDatabase.Contains(TypedTarget))
//                {
//                    DrawButton("Set up mod folders", () => ModUtility.SetUpModFolders(TypedTarget));

//                    DrawButton("Build mod", () => BuildMod());
//                }
//            }
//            EditorGUILayout.EndVertical();
//        }

//        private void DrawButton(string name, Action action)
//        {
//            if (GUILayout.Button(name))
//            {
//                action?.Invoke();
//            }
//        }

//        private void BuildMod()
//        {
//            ModUtility.CreateDefaultModBuildPath();

//            string buildPath = EditorUtility.SaveFolderPanel("Choose mod build directory", ModUtility.DefaultModBuildPath, TypedTarget.ModID);

//            if (string.IsNullOrWhiteSpace(buildPath))
//            {
//                return;
//            }

//            DirectoryInfo buildDirectory = new DirectoryInfo(buildPath);

//            if (!buildDirectory.IsEmpty())
//            {
//                if (!EditorUtility.DisplayDialog(
//                    "Overwrite directory?",
//                    "Pressing 'Overwrite' will overwrite all contents of this directory. Are you sure you want to continue?",
//                    "Overwrite",
//                    "Cancel"))
//                {
//                    return;
//                }
//            }

//            Debug.Log($"Starting build of {TypedTarget.ModID}");

//            ModUtility.Build(TypedTarget, buildPath, true);

//            Debug.Log($"Build finished");
//        }
//    }
//}
