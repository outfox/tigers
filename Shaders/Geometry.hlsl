#ifndef TIGER_SHADER_GEOMETRY
#define TIGER_SHADER_GEOMETRY

#define UnitSphereIntersection_float UnitSphereIntersection

void UnitSphereIntersection(const float3 r0, const float3 rd, const float3 s0, const float sr, out float t1, out float t2, out float thickness, out float3 surf, out float depth)
{
    const float3 s0_r0 = s0-r0;
    const float t = dot(s0_r0, rd);

    const float3 p = r0 + t * rd;
    const float y = length(p-s0);

    if (y < sr)
    {
        float x = sqrt(sr*sr-y*y);
        t1 = t-x;
        t2 = min(t+x,0);
        thickness = saturate((t2-t1)/2);
        surf = r0+t2*rd;
    }
    else
    {
        thickness = 0;
        surf = 0;
    }
    depth = (length(s0_r0));
}


#endif

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