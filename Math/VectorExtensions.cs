using Unity.Mathematics;
using UnityEngine;

namespace Tiger.Math
{
    public static class VectorExtensions
    {
        public static float ManhattanDistance(this Vector2 v, Vector2 other = default)
        {
            return math.csum(math.abs(v - other));
        }

        public static float ManhattanDistance(this Vector3 v, Vector3 other = default)
        {
            return math.csum(math.abs(v - other));
        }

        public static float ManhattanDistance(this Vector4 v, Vector4 other = default)
        {
            return math.csum(math.abs(v - other));
        }

        public static Vector2 Round(this Vector2 v)
        {
            return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
        }

        public static Vector3 Round(this Vector3 v)
        {
            return new Vector3(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));
        }

        public static Vector3Int RoundToInt(this Vector3 v)
        {
            return new Vector3Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y), Mathf.RoundToInt(v.z));
        }

        public static Vector2 Clamp(this Vector2 v, Rect r)
        {
            return new Vector2(Mathf.Clamp(v.x, r.xMin, r.xMax), Mathf.Clamp(v.y, r.yMin, r.yMax));
        }

        public static float Average(this Vector3 v)
        {
            return (v.x + v.y + v.z) / 3.0f;
        }

        public static float CMax(this Vector3 v)
        {
            return Mathf.Max(v.x, v.y, v.z);
        }

        public static float Min(this Vector3 v)
        {
            return Mathf.Min(v.x, v.y, v.z);
        }
    }
    
    public class V3
    {
        public static Vector3 From(double3 v)
        {
            return new Vector3((float) v.x, (float) v.y, (float) v.z);
        }
    }

    public class V2
    {
        public static readonly Vector2 zero = Vector2.zero;
        public static readonly Vector2 one = Vector2.one;
    }
}

/*
Written by Tiger Blue in 2018, 2023

This is free and unencumbered software released into the public domain.

Anyone is free to copy, modify, publish, use, compile, sell, or
distribute this software, either in source code form or as a compiled
binary, for any purpose, commercial or non-commercial, and by any
means.

In jurisdictions that recognize copyright laws, the author or authors
of this software dedicate any and all copyright interest in the
software to the public domain. We make this dedication for the benefit
of the public at large and to the detriment of our heirs and
successors. We intend this dedication to be an overt act of
relinquishment in perpetuity of all present and future rights to this
software under copyright law.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.

For more information, please refer to <http://unlicense.org>
*/