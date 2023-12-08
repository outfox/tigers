using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Tiger.Util.Editor
{
    public class RebindModelsUtility : EditorWindow
    {
        public GameObject[] models;
        private string _replaceWith;
        private string _searchPattern;

        [MenuItem("Tiger/Rebind Models")]
        private static void CreateReplaceWithPrefab()
        {
            EditorWindow.GetWindow<RebindModelsUtility>("Rebind Models");
        }

        private void OnGUI()
        {
            ScriptableObject scriptable_obj = this;
            SerializedObject serial_obj = new SerializedObject (scriptable_obj);
            SerializedProperty serial_prop = serial_obj.FindProperty ("models");
 
            EditorGUILayout.PropertyField (serial_prop, true);
            serial_obj.ApplyModifiedProperties();

            if (GUILayout.Button("Rebind for Selected MeshFilters"))
            {
                PerformRebind(Selection.gameObjects, models);
            }
            
            GUI.enabled = false;
            EditorGUILayout.LabelField("Selection count: " + Selection.objects.Length);
        }

        private static void PerformRebind(IReadOnlyList<GameObject> selection, GameObject[] models)
        {
            //Build Lookup Database
            var lookup = new Dictionary<string, MeshFilter>();
            foreach (var model in models)
            {
                //Gracefully handle nulls
                if (!model) continue;
                
                foreach (var mesh_filter in model.GetComponentsInChildren<MeshFilter>())
                {
                    if (lookup.ContainsKey(mesh_filter.sharedMesh.name))
                    {
                        Debug.LogError("MeshFilter collision for:", mesh_filter.gameObject);
                        Debug.LogError("Preexisting in lookup:", lookup[mesh_filter.sharedMesh.name].gameObject);
                        return; //Abort :)
                    }
                    lookup[mesh_filter.sharedMesh.name] = mesh_filter;
                }
            }
            
            //Replace All MeshFilter sharedMeshes with those in our lookup
            var count = 0;
            foreach (var selected in selection)
            {
                foreach (var mesh_filter in selected.GetComponentsInChildren<MeshFilter>())
                {
                    if (lookup.TryGetValue(mesh_filter.sharedMesh.name, out var value))
                    {
                        var other_mesh = value.sharedMesh;
                        if (mesh_filter.sharedMesh == other_mesh) continue;
                        
                        EditorUtility.SetDirty(selected);
                        EditorUtility.SetDirty(mesh_filter.gameObject);
                        mesh_filter.sharedMesh = other_mesh;
                        count++;
                    }
                    else
                    {
                        Debug.Log("Lookup miss for " + mesh_filter.sharedMesh.name, mesh_filter.gameObject);
                    }
                }
            }
            Debug.Log("Rebound " + count + " meshes.");
        }
    }
}