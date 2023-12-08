using UnityEditor;
using UnityEngine;

namespace Tiger.Util.Editor
{
    [InitializeOnLoad]
    public static class HierarchyIcons
    {
        const string IgnoreIcons = "d_GameObject Icon, d_Prefab Icon, d_PrefabVariant Icon";

        static HierarchyIcons()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
        }

        private static void HandleHierarchyWindowItemOnGUI(int instance_id, Rect selection_rect)
        {
            var content = EditorGUIUtility.ObjectContent(EditorUtility.InstanceIDToObject(instance_id), null);

            if (content.image != null && !IgnoreIcons.Contains(content.image.name))
            {
                GUI.DrawTexture(new Rect(selection_rect.xMax - 16, selection_rect.yMin, 16, 16), content.image);
            }
        }
    }
}