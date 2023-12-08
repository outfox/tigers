using Unity.Mathematics;
using Random = Unity.Mathematics.Random;


namespace Tiger.Util
{
    public static class RandomExtensions
    {
        public static float3 NextInsideSphere(this ref Random rand)
        {
            var phi = rand.NextFloat(2 * math.PI);
            var theta = math.acos(rand.NextFloat(-1f, 1f));
            var r = math.pow(rand.NextFloat(), 1f / 3f);
            var x = math.sin(theta) * math.cos(phi);
            var y = math.sin(theta) * math.sin(phi);
            var z = math.cos(theta);
            return r * new float3(x, y, z);
        }
 
        public static float3 NextOnSphereSurface(this ref Random rand)
        {
            var phi = rand.NextFloat(2 * math.PI);
            var theta = math.acos(rand.NextFloat(-1f, 1f));
            var x = math.sin(theta) * math.cos(phi);
            var y = math.sin(theta) * math.sin(phi);
            var z = math.cos(theta);
            return new float3(x, y, z);
        }
    }
}


/*
Written by Tiger Blue in 2020

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