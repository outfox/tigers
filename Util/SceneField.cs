#if UNITY_EDITOR
#endif
using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tiger.Util
 {
     [System.Serializable]
     public class SceneField
     {
         [SerializeField] private Object _sceneAsset;
         [SerializeField] private string _sceneName = "";

         //FIXME: This doesn't work if the Scene Asset Name changes!
         public string name => _sceneName;

         // makes it work with the existing Unity methods (LoadLevel/LoadScene)
         public static implicit operator string(SceneField sceneField)
         {
             //FIXME: This doesn't work if the Scene Asset Name changes!
             return sceneField.name;
         }
     }
 
 #if UNITY_EDITOR
     [CustomPropertyDrawer(typeof(SceneField))]
     public class SceneFieldPropertyDrawer : PropertyDrawer
     {
         public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
         {
             EditorGUI.BeginProperty(position, GUIContent.none, property);
             var scene_asset = property.FindPropertyRelative("_sceneAsset");
             var scene_name = property.FindPropertyRelative("_sceneName");
             
             position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
             if (scene_asset != null)
             {
                 EditorGUI.BeginChangeCheck();
                 var value = EditorGUI.ObjectField(position, scene_asset.objectReferenceValue, typeof(SceneAsset), false);
                 if (EditorGUI.EndChangeCheck())
                 {
                     scene_asset.objectReferenceValue = value;
                     if (scene_asset.objectReferenceValue != null)
                     {
                         var scene_path = AssetDatabase.GetAssetPath(scene_asset.objectReferenceValue);
                         var assets_index = scene_path.IndexOf("Assets", StringComparison.Ordinal) + 7;
                         var extension_index = scene_path.LastIndexOf(".unity", StringComparison.Ordinal);
                         scene_path = scene_path.Substring(assets_index, extension_index - assets_index);
                         scene_name.stringValue = scene_path;
                     }
                 }
             }
             EditorGUI.EndProperty();
         }
     }
 #endif
 }