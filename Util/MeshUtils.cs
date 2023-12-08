using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace Tiger.Util
{
    public static class MeshUtils
    {
        public static Mesh Copy(this Mesh mesh, Vector3 scale = default)
        {
            //We presume nobody wants to scale to zero.
            if (scale == default) scale = Vector3.one;
            
            var result = new Mesh
            {
                vertices = mesh.vertices.Select(v => Vector3.Scale(v, scale)).ToArray(),
                triangles = mesh.triangles,
                colors = mesh.colors,
                normals = mesh.normals,
                tangents = mesh.tangents,
                uv = mesh.uv,
                uv2 = mesh.uv2,
                uv3 = mesh.uv3,
                uv4 = mesh.uv4,
                bindposes = mesh.bindposes,
                boneWeights = mesh.boneWeights,
                bounds = mesh.bounds
            };
            
            return result;
        }

        /// <summary>
        /// Generates a simple quad of any size
        /// </summary>
        /// <param name="size">The size of the quad</param>
        /// <param name="pivot">Where the mesh pivots</param>
        /// <returns>The quad mesh</returns>
        public static Mesh GenerateQuad(float2 size, float2 pivot)
        {
            var scaled_pivot = size * pivot;
            Vector3[] vertices =
            {
                new Vector3(size.x - scaled_pivot.x, size.y - scaled_pivot.y, 0),
                new Vector3(size.x - scaled_pivot.x, -scaled_pivot.y, 0),
                new Vector3(-scaled_pivot.x, -scaled_pivot.y, 0),
                new Vector3(-scaled_pivot.x, size.y - scaled_pivot.y, 0),
            };

            Vector2[] uv =
            {
                new Vector2(1, 1),
                new Vector2(1, 0),
                new Vector2(0, 0),
                new Vector2(0, 1)
            };

            int[] triangles =
            {
                0, 1, 2,
                2, 3, 0
            };

            return new Mesh
            {
                vertices = vertices,
                uv = uv,
                triangles = triangles
            };
        }
    }
}