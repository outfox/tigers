/*#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Tiger.Util.Editor
{
    [CustomPropertyDrawer(typeof(AutoAttribute))]
    public class AutoAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var text = $"Ⓐ {label.text}";
            GUI.Label(position, text);
        }
    }
}
#endif*/