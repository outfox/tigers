using UnityEditor;
using UnityEngine;

namespace Tiger.Util.Editor
{
    public class TextureArrayCreator : MonoBehaviour 
    {
        [MenuItem("Assets/Texture Array 2D")]
        private static void Create()
        {
            var slices = Selection.objects.Length;

            var first = Selection.objects[0] as Texture2D;
        
            var texture_array = new Texture2DArray(first.width, first.height, slices, TextureFormat.RGB24, false);
            texture_array.filterMode = FilterMode.Point;
            texture_array.wrapMode = TextureWrapMode.Clamp;
            
            for (var i = 0; i < slices; i++)
            {
                var tex = Selection.objects[i] as Texture2D;
                texture_array.SetPixels(tex.GetPixels(0), i, 0);
            }
            texture_array.Apply();
            
            var path = "Assets/Pipeline/Palettes/FactionColors.asset";
            AssetDatabase.CreateAsset(texture_array, path);
        }
    }
}

