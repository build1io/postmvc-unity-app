#if UNITY_EDITOR

using UnityEditor;

namespace Build1.PostMVC.Unity.App.Modules.UI.Layers
{
    [CustomEditor(typeof(UILayers))]
    public sealed class UILayersEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var list = serializedObject.FindProperty("layers");

            EditorGUILayout.PropertyField(list, false);

            for (var i = 0; i < list.arraySize; i++)
            {
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}

#endif