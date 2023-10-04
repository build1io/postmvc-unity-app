#if UNITY_EDITOR

using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Build1.PostMVC.Unity.App.Modules.UI.Layers
{
    [CustomPropertyDrawer(typeof(UILayerInfo))]
    public sealed class UILayerInfoDrawer : PropertyDrawer
    {
        private Rect               _position;
        private SerializedProperty _property;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _position = position;
            _property = property;
            
            EditorGUI.BeginProperty(position, label, property);
            
            var indent = EditorGUI.indentLevel;
            var height = position.height - 2;
            
            EditorGUI.indentLevel = 0;
            
            var regex = new Regex(@"\[([0-9]+)\]");
            var match = regex.Match(property.propertyPath);
            var index = int.Parse(match.Groups[1].Value);

            var x = position.x - 8;
            x = DrawLabel($"Layer {index:00}", x, 70, height);
            x = DrawLabel("|", x, 23, height);
            x = DrawProperty("Id:", "id", x, 80, height, 20);
            x = DrawProperty("Name:", "name", x, 180, height, 44);
            x = DrawProperty("Game Object:", "gameObject", x, -1, height, 83);

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        private float DrawLabel(string label, float x, float width, float height)
        {
            EditorGUI.LabelField(new Rect(x, _position.y, width, height), label);
            return x + width;
        }
        
        private float DrawProperty(string label, string name, float x, float width, float height, float labelWidth)
        {
            var propertyWidth = 0F;
            var padding = 0;
            
            if (width == -1)
                propertyWidth = _position.x + _position.width - x - labelWidth - padding;
            else
                propertyWidth = width - labelWidth - padding;
            
            EditorGUI.LabelField(new Rect(x, _position.y, labelWidth, height), label);
            EditorGUI.PropertyField(new Rect(x + labelWidth + padding, _position.y, propertyWidth, height), _property.FindPropertyRelative(name), GUIContent.none);

            return x + labelWidth + 10 + propertyWidth;
        }
    }
}

#endif