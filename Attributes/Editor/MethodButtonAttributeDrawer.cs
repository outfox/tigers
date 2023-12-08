using System.Reflection;
using Tiger.Util;
using UnityEditor;
using UnityEngine;

namespace Tiger.Attributes.Editor
{
    [CustomPropertyDrawer(typeof(MethodButtonAttribute))]
    public class MethodButtonAttributeDrawer : PropertyDrawer
    {
        private int _buttonCount;
        private readonly float _buttonHeight = EditorGUIUtility.singleLineHeight * 2;
        private MethodButtonAttribute _attr;

        public override void OnGUI(Rect position, SerializedProperty editorFoldout, GUIContent label)
        {
            if (editorFoldout.name.Equals("editorFoldout") == false)
            {
                LogErrorMessage(editorFoldout);
                return;
            }

            _buttonCount = 0;

            var foldoutRect = new Rect(position.x, position.y, position.width, 5 + _buttonHeight);

            editorFoldout.boolValue = EditorGUI.Foldout(foldoutRect, editorFoldout.boolValue, "Buttons", true);

            //Hide if not expanded
            if (!editorFoldout.boolValue) return;
            
            _buttonCount++;

            _attr = (MethodButtonAttribute) base.attribute;

            foreach (var name in _attr.MethodNames)
            {
                _buttonCount++;

                var buttonRect = new Rect(position.x, position.y + ((1 + _buttonHeight) * (_buttonCount - 1)), position.width, _buttonHeight - 1);
                if (GUI.Button(buttonRect, name))
                {
                    InvokeMethod(editorFoldout, name);
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true) + (_buttonHeight) * (_buttonCount);
        }

        private void InvokeMethod(SerializedProperty property, string name)
        {
            var target = property.serializedObject.targetObject;
            target.GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)?.Invoke(target, null);
        }

        private static void LogErrorMessage(SerializedProperty editorFoldout)
        {
            Debug.LogError("<color=red><b>Possible improper usage of method button attribute!</b></color>");
#if NET_4_6
        Debug.LogError($"Got field name: <b>{editorFoldout.name}</b>, Expected: <b>editorFoldout</b>");
        Debug.LogError($"Please see <b>{"Usage"}</b> at <b><i><color=blue>{"https://github.com/GlassToeStudio/UnityMethodButtonAttribute/blob/master/README.md"}</color></i></b>");
#else
            Debug.LogError($"Got field name: <b>{editorFoldout.name}</b>, Expected: <b>editorFoldout</b>");
            Debug.LogError("Please see <b>\"Usage\"</b> at <b><i><color=blue>\"https://github.com/GlassToeStudio/UnityMethodButtonAttribute/blob/master/README.md \"</color></i></b>");
#endif
        }
    }
}
