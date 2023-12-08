//SPDX-License-Identifier: Unlicense

using System;
using Unity.Mathematics;
using UnityEngine;

namespace Tiger.Util
{
	[Serializable]
	public struct RangedFloat
	{
		/// <summary>
		/// The minimum value of this range
		/// </summary>
		public float minimum; //TODO: Turn into self-healing property

		/// <summary>
		/// The maximum value of this range
		/// </summary>
		public float maximum; //TODO: Turn into self-healing property

		/// <summary>
		/// Clamps the value between minimum and maximum.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public float Clamp(float value)
		{
			return Mathf.Clamp(value, minimum, maximum);
		}

		/// <summary>
		/// Interprets minimum and maximum as an arc, and returns the closest angle delta to the current value.
		/// </summary>
		/// <param name="current"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public float ClosestAngleDelta(float current, float value)
		{
			var midpoint = (maximum + minimum) / 2.0f;

			var combined = (current + value) % 360.0f;

			if (Inside(combined)) return value;

			//Closest point past the pivot point from both directions
			if (combined > midpoint + 180 || combined < midpoint - 180)
				return -value;

			return value;
		}

		/// <summary>
		///	Tells you if the value is inside the range.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool Inside(float value)
		{
			return value >= minimum && value <= maximum;
		}

		/// <summary>
		/// Returns a random value between minimum and maximum, inclusive.
		/// </summary>
		/// <returns></returns>
		public float Random()
		{
			return UnityEngine.Random.Range(minimum, maximum);
		}

		/// <summary>
		/// Linearly interpolates between minimum and maximum.
		/// </summary>
		/// <param name="value">from 0 .. 1</param>
		/// <returns></returns>
		public float Lerp(float value)
		{
			return Mathf.Lerp(minimum, maximum, value);
		}

		/// <summary>
		/// Linearly interpolates between minimum and maximum.
		/// </summary>
		/// <param name="value">from 0 .. 1</param>
		/// <returns></returns>
		public float LerpUnclamped(float value)
		{
			return Mathf.LerpUnclamped(minimum, maximum, value);
		}

		/// <summary>
		/// Linearly interpolates between minimum and maximum.
		/// </summary>
		/// <param name="value">from -1 .. 1</param>
		/// <returns></returns>
		public float Lerp1M1(float value)
		{
			return Mathf.Lerp(minimum, maximum, (value + 1f) / 2f);
		}

		/// <summary>
		/// Smoothly interpolates between minimum and maximum.
		/// </summary>
		/// <param name="value">from 0 to 1</param>
		/// <returns>smoothstep interpolated value between min and max</returns>
		public float SmoothStep(float value)
		{
			return math.remap(0, 1, minimum, maximum, math.smoothstep(0, 1, value));
		}

		/// <summary>
		/// Smoothly interpolates between minimum and maximum.
		/// </summary>
		/// <param name="value">from 0 to 1</param>
		/// <returns>smoothstep interpolated value between min and max</returns>
		public float SmoothStep1M1(float value)
		{
			return math.remap(0, 1, minimum, maximum, math.smoothstep(-1, 1, value));
		}
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