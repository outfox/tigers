using System.Collections.Generic;
using UnityEditor;

using UnityEngine;

namespace Tiger.Util.Editor
{
    public class SearchReplaceUtility : EditorWindow
    {
        private GameObject _prefab;
        private string _replaceWith;
        private string _searchPattern;

        [MenuItem("Tiger/Search+Replace")]
        private static void CreateReplaceWithPrefab()
        {
            EditorWindow.GetWindow<SearchReplaceUtility>("Search+Replace");
        }

        private void OnGUI()
        {
            _prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", _prefab, typeof(GameObject), false);

            if (GUILayout.Button("Replace"))
            {
                PerformReplace(Selection.gameObjects, _prefab);
            }

            var stage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
            if (stage)
            {
                if (GUILayout.Button("Auto Replace by Name"))
                {
                    var prefabs = Selection.gameObjects;
                    foreach(var prefab in prefabs)
                    {
                        var selection = FindAll(stage.prefabContentsRoot, prefab.name);
                        PerformReplace(selection, prefab);
                    }
                }
                EditorGUILayout.LabelField("Selection count: " + Selection.objects.Length);
            }

            if (stage)
            {

                GUILayout.Label("Search Names for Substring");
                _searchPattern = EditorGUILayout.TextField("", _searchPattern);
            
                GUILayout.Label("Replace with");
                _replaceWith = EditorGUILayout.TextField("", _replaceWith);

                if (GUILayout.Button("Replace Substrings"))
                {
                    RenameAll(stage.prefabContentsRoot, _searchPattern, _replaceWith);
                }
            }
        
            GUI.enabled = false;
            EditorGUILayout.LabelField("Selection count: " + Selection.objects.Length);
        }

        private static void PerformReplace(IReadOnlyList<GameObject> selection, GameObject prefab)
        {
            for (var i = selection.Count - 1; i >= 0; --i)
            {
                var selected = selection[i];
                var prefabType = PrefabUtility.GetPrefabAssetType(prefab);
                GameObject newObject;

                if (prefabType != PrefabAssetType.NotAPrefab)
                {
                    newObject = (GameObject) PrefabUtility.InstantiatePrefab(prefab);
                }
                else
                {
                    newObject = Instantiate(prefab);
                    newObject.name = prefab.name;
                }

                if (newObject == null)
                {
                    Debug.LogError("Error instantiating prefab");
                    break;
                }

                Undo.RegisterCreatedObjectUndo(newObject, "Replace With Prefabs");
                newObject.transform.parent = selected.transform.parent;
                newObject.transform.localPosition = selected.transform.localPosition;
                newObject.transform.localRotation = selected.transform.localRotation;
                newObject.transform.localScale = selected.transform.localScale;
                newObject.transform.SetSiblingIndex(selected.transform.GetSiblingIndex());
                Undo.DestroyObjectImmediate(selected);
            }
        }
    

        private static List<GameObject> FindAll(GameObject root, string searchName)
        {
            var result = new List<GameObject>();
        
            for(var i = 0; i < root.transform.childCount; i++)
            {
                var child = root.transform.GetChild(i).gameObject;
                if (child.name == searchName)
                    result.Add(child);
                else
                    result.AddRange(FindAll(child, searchName));
            }

            return result;
        }
    
        private static void RenameAll(GameObject root, string searchPattern, string replaceWith)
        {
            for(var i = 0; i < root.transform.childCount; i++)
            {
                var child = root.transform.GetChild(i).gameObject;
                child.name = child.name.Replace(searchPattern, replaceWith);
                RenameAll(child, searchPattern, replaceWith);
            }
        }
    
    }
}