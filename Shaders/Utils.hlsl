#ifndef TIGER_SHADER_UTILS
#define TIGER_SHADER_UTILS

#define MixFogAlpha_float MixFogAlpha

void MixFogAlpha(real4 color, const real fog_coord, out real4 out_color)
{
    #ifdef SHADERGRAPH_PREVIEW
    out_color = color;
    #else
    out_color = real4(lerp(color.rgb, MixFog(color.rgb, fog_coord), unity_FogColor.a), color.a);
    #endif    
}

#define MixFogAlphaLuminous_float MixFogAlphaLuminous

void MixFogAlphaLuminous(real4 color, const real fog_coord, out real4 out_color)
{
    #ifdef SHADERGRAPH_PREVIEW
    out_color = color;
    #else
    out_color = real4(lerp(color.rgb * (1+fog_coord), MixFog(color.rgb * (1+fog_coord), fog_coord), unity_FogColor.a), color.a);
    #endif    
}

#define SampleCubemap_float SampleCubemap

void SampleCubemap(UnityTextureCube cube, const real3 dir, out real4 out_color)
{
    out_color = texCUBE(
    	cube,
    	dir
    );
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