using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tiger.Util
{
	public static class TransformExtensions
	{
		/// <summary>
		/// Finds a transform with a given name recursively.
		/// </summary>
		/// <param name="parent">Root transform to start seaching at.</param>
		/// <param name="name">Transform's name to search for.</param>
		/// <returns>Transform with the first child that matches the name, or null if none is found.</returns>
		public static Transform FindDeep(this Transform parent, string name)
		{
			var result = parent.Find(name);
		
			if (result != null) return result;
		
			foreach(Transform child in parent)
			{
				result = child.FindDeep(name);
			
				if (result != null) return result;
			}

			return null;
		}
		
		/// <summary>
		/// Adds another transform as a child.
		/// There is a choice between local or world space additions.
		/// </summary>
		/// <param name="transform">the parent</param>
		/// <param name="child">the child</param>
		/// <param name="worldPositionStays">false: keep the local position; true: preserve the world position</param>
		// ReSharper disable once InconsistentNaming
		public static void Add(this Transform transform, Transform child, bool worldPositionStays = true)
		{
			child.SetParent(transform, worldPositionStays);
		}
		
		/// <summary>
		/// A reverse enumeration of the transform. Useful for destructive or additive operations.
		/// </summary>
		/// <param name="transform">the transform whose children to enumerate</param>
		/// <returns>reverse enumerator over the transform's children</returns>
		public static IEnumerator<Transform> GetReverseEnumerator(this Transform transform)
		{
			for (var i = transform.childCount-1; i >= 0; i--) yield return transform.GetChild(i);
		}
		
		/// <summary>
		/// Destroy all children of the given transform.
		/// </summary>
		/// <param name="transform">the transform</param>
		public static void DestroyAllChildren(this Transform transform)
		{
			for (var i = transform.childCount-1; i >= 0; i--) Object.Destroy(transform.GetChild(i));		   
		}

		/// <summary>
		/// Returns the Children of the Transform as a read-only collection
		/// (an IReadOnlyList backed by an Array, which is allocated for this purpose)
		/// </summary>
		/// <param name="transform">the transform whose children to get</param>
		/// <returns>a collection containing the child transforms</returns>
		public static IReadOnlyList<Transform> Children(this Transform transform)
		{
			var children = new Transform[transform.childCount];
			for (var i = 0; i < transform.childCount; i++)
			{
				children[i] = transform.GetChild(i);
			}
			return children;
		}		
	}
}

/*
Written by Tiger Blue in 2018, 2022

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