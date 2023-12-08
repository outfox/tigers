using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Tiger.Util
{
    public static class Reflector
    {
        public static List<Type> GetSubTypes<T>() where T : class
        {
            var types = new List<Type>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.FullName.StartsWith("UnityScript"))
                    continue;

                if (assembly.FullName.StartsWith("System"))
                    continue;

                if (assembly.FullName.StartsWith("I18N"))
                    continue;

                if (assembly.FullName.StartsWith("UnityEngine"))
                    continue;

                if (assembly.FullName.StartsWith("UnityEditor"))
                    continue;

                if (assembly.FullName.StartsWith("Mono.Cecil"))
                    continue;

                if (assembly.FullName.StartsWith("mscorlib"))
                    continue;

                foreach (Type type in assembly.GetTypes())
                {
                    if (!type.IsClass)
                        continue;

                    if (type.IsAbstract)
                        continue;

                    if (!type.IsSubclassOf(typeof(T)))
                        continue;

                    types.Add(type);
                }
            }

            return types;
        }
    
#if UNITY_EDITOR
        public static AssetBundle GetEditorAssetBundle()
        {
            var t = Type.GetType("UnityEditor.EditorGUIUtility,UnityEditor.dll");
            if (t == null) return null;
            var m = t.GetMethod("GetEditorAssetBundle", BindingFlags.Static | BindingFlags.NonPublic);
            if (m == null) return null;
            return m.Invoke(null,null) as AssetBundle;        
        }
#endif
    }
}    


/*
Written by Tiger Blue in 2018

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