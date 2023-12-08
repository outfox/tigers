using Unity.Mathematics;
using UnityEngine;

// ReSharper disable file InconsistentNaming

namespace Tiger.Math
{
    public static partial class mathex
    {
        /// <summary>
        /// Euclidean Division Modulus, aka "remainder" with spatial wrapping.
        /// </summary>
        public static float eumod(float a, float b) => wrap(a, -b, b);

        /// <summary>
        /// Euclidean Division Modulus, aka "remainder" with spatial wrapping.
        /// </summary>
        public static float2 eumod(float2 a, float2 b) => wrap(a, -b, b);

        /// <summary>
        /// Euclidean Division Modulus, aka "remainder" with spatial wrapping.
        /// </summary>
        public static float3 eumod(float3 a, float3 b) => wrap(a, -b, b);

        /// <summary>
        /// Wrap a float to a given range.
        /// </summary>
        public static float wrap(float a, float min, float max)
        {
            if (min > max)
            {
                (min, max) = (max, min);
            }

            if (a < min) return a + (max - min);
            if (a > max) return a - (max - min);
            return a;   
            
            //return (a < 0 ? max : min) + math.fmod(a - min, max - min);
        }

        /// <summary>
        /// Wrap a float2 to a given range.
        /// </summary>
        public static float2 wrap(float2 a, float2 min, float2 max)
        {
            return new float2(wrap(a.x, min.x, max.x), wrap(a.y, min.y, max.y));
        }

        /// <summary>
        /// Wrap a float3 to a given range.
        /// </summary>
        public static float3 wrap(float3 a, float3 min, float3 max)
        {
            return new float3(wrap(a.x, min.x, max.x), wrap(a.y, min.y, max.y), wrap(a.z, min.z, max.z));
        }
    }

    public static partial class MathEx
    {
        /// <summary>
        /// Unity.Mathematics style variant of UnityEngine.Vector3's ProjectOnPlane.
        /// Extension method that lets us use float3 like Vector3 for this purpose
        /// </summary>
        public static float3 ProjectOnPlane(this float3 vector, float3 planeNormal)
        {
            return vector - math.project(vector, planeNormal);
        }

        public static double Angle(double3 from, double3 to)
        {
            var dot = math.dot(math.normalizesafe(from), math.normalize(to));
            return math.acos(dot < -1.0 ? -1.0 : dot > 1.0 ? 1.0 : dot);
        }


        //Ray Sphere intersection
        public static bool RaySphereIntersection(float3 rayOrigin, float3 rayDirection, float3 sphereCenter, float sphereRadius, out float3 point)
        {
            float distance = 0;
            var vector = rayOrigin - sphereCenter;
            var num1 = math.dot(vector, rayDirection);
            var num2 = math.dot(vector, vector) - sphereRadius * sphereRadius;
            if (num2 > 0.0 && num1 > 0.0)
            {
                distance = 0.0f;
                point = sphereCenter;
                return false;
            }

            var num3 = num1 * num1 - num2;
            if (num3 < 0.0)
            {
                distance = 0.0f;
                point = sphereCenter;
                return false;
            }

            distance = -num1 - math.sqrt(num3);
            point = rayOrigin + rayDirection * distance;
            return true;
        }

        public static float3 ClosestPointOnOrToSphere(float3 r0, float3 rd, float3 s0, float radius)
        {
            var s0_r0 = s0 - r0;
            var t = math.dot(s0_r0, rd);
            var p = r0 + t * rd;

            var y = math.length(p - s0);

            if (t < 0) return r0; //TODO: Do vertical projection down - this isn't on the sphere

            if (y < radius)
            {
                var x = math.sqrt(radius * radius - y * y);
                var t1 = math.max(t - x, 0);
                return r0 + rd * t1;
            }
            else
            {
                return r0 + rd * t;
            }
        }
    }
}

/*
Written by Tiger Blue in 2022

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