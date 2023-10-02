#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Build1.PostMVC.Unity.App.Modules.UI.Layers
{
    [CustomEditor(typeof(UILayers))]
    public sealed class UILayersEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUILayout.Space(5);
            
            EditorGUILayout.HelpBox("The first layer on the list will be the top one.", MessageType.Info);
            
            GUILayout.Space(5);
            
            EditorGUILayout.PropertyField(serializedObject.FindProperty("layers"));
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}

#endif