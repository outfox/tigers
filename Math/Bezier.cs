using Unity.Mathematics;
using UnityEngine;

namespace Tiger.Math
{
	public static class Bezier
	{
		public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
		{
			t = Mathf.Clamp01(t);
			var one_minus_t = 1f - t;
			return
				one_minus_t * one_minus_t * p0 +
				2f * one_minus_t * t * p1 +
				t * t * p2;
		}

		public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, float t)
		{
			return
				2f * (1f - t) * (p1 - p0) +
				2f * t * (p2 - p1);
		}

		public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
		{
			t = Mathf.Clamp01(t);
			var one_minus_t = 1f - t;
			return
				one_minus_t * one_minus_t * one_minus_t * p0 +
				3f * one_minus_t * one_minus_t * t * p1 +
				3f * one_minus_t * t * t * p2 +
				t * t * t * p3;
		}

		public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
		{
			t = Mathf.Clamp01(t);
			var one_minus_t = 1f - t;
			return
				3f * one_minus_t * one_minus_t * (p1 - p0) +
				6f * one_minus_t * t * (p2 - p1) +
				3f * t * t * (p3 - p2);
		}


		public static float3 GetPoint(float3 p0, float3 p1, float3 p2, float3 p3, float t)
		{
			t = math.saturate(t);
			var one_minus_t = 1f - t;
			return
				one_minus_t * one_minus_t * one_minus_t * p0 +
				3f * one_minus_t * one_minus_t * t * p1 +
				3f * one_minus_t * t * t * p2 +
				t * t * t * p3;
		}

		public static Vector3 GetFirstDerivative(float3 p0, float3 p1, float3 p2, float3 p3, float t)
		{
			t = math.saturate(t);
			var one_minus_t = 1f - t;
			return
				3f * one_minus_t * one_minus_t * (p1 - p0) +
				6f * one_minus_t * t * (p2 - p1) +
				3f * t * t * (p3 - p2);
		}

		public static double3 GetPoint(double3 p0, double3 p1, double3 p2, double3 p3, double t)
		{
			t = math.saturate(t);
			var one_minus_t = 1f - t;
			return
				one_minus_t * one_minus_t * one_minus_t * p0 +
				3f * one_minus_t * one_minus_t * t * p1 +
				3f * one_minus_t * t * t * p2 +
				t * t * t * p3;
		}

		public static double3 GetFirstDerivative(double3 p0, double3 p1, double3 p2, double3 p3, double t)
		{
			t = math.saturate(t);
			var one_minus_t = 1f - t;
			return
				3f * one_minus_t * one_minus_t * (p1 - p0) +
				6f * one_minus_t * t * (p2 - p1) +
				3f * t * t * (p3 - p2);
		}
	}
}