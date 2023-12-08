using System;
using Unity.Mathematics;

namespace Tiger.Math
{
	public static class Hermite
	{
		#region float
		public static float3 GetPosition(float3 p0, float3 p1, float3 v0, float3 v1, float t)
		{
			//f(t) = (2t^3-3t^2+1)*p0 + (t^3-2t^2+t)*v0 + (-2*t^3+3t^2)*p1 + (t^3-t^2)*v1
			return (2.0f * t * t * t - 3.0f * t * t + 1.0f) * p0
			       + (t * t * t - 2.0f * t * t + t) * v0
			       + (-2.0f * t * t * t + 3.0f * t * t) * p1
			       + (t * t * t - t * t) * v1;
		}

		public static float3 GetVelocity(float3 p0, float3 p1, float3 v0, float3 v1, float t)
		{
			//f(t) = (2t^3-3t^2+1)*p0 + (t^3-2t^2+t)*v0 + (-2*t^3+3t^2)*p1 + (t^3-t^2)*v1
			//f'(t) = (6t^2-6t)*p0 + (3t^2-4t^1+1)*v0 + (-6*t^2+6t^1)*p1 + (3t^2-2t)*v1
			return (6 * t * t - 6 * t) * p0 
			       + (3 * t * t - 4 * t + 1) * v0 
			       + (-6 * t * t + 6 * t) * p1 
			       + (3 * t * t - 2 * t) * v1;
		}

		public static float3 GetAcceleration(float3 p0, float3 p1, float3 v0, float3 v1, float t)
		{
			//f(t) = (2t^3-3t^2+1)*p0 + (t^3-2t^2+t)*v0 + (-2*t^3+3t^2)*p1 + (t^3-t^2)*v1
			//f'(t) = (6t^2-6t)*p0 + (3t^2-4t^1+1)*v0 + (-6*t^2+6t^1)*p1 + (3t^2-2t)*v1
			//f''(t) = (12t-6)*p0 + (6t-4)*v0 + (-12*t+6)*p1 + (6t-2)*v1
			return (12 * t - 6) * p0 
			       + (6 * t - 4) * v0 
			       + (-12 * t + 6) * p1 
			       + (6 * t + 2) * v1;
		}

		public static float DistanceEstimate(float3 p0, float3 p1, float3 v0, float3 v1, float t)
		{
			throw new NotImplementedException();
		}
		#endregion
		
		#region double
		public static double3 GetPosition(double3 p0, double3 p1, double3 v0, double3 v1, double t)
		{
			//f(t) = (2t^3-3t^2+1)*p0 + (t^3-2t^2+t)*v0 + (-2*t^3+3t^2)*p1 + (t^3-t^2)*v1
			return (2.0f * t * t * t - 3.0f * t * t + 1.0f) * p0
			       + (t * t * t - 2.0f * t * t + t) * v0
			       + (-2.0f * t * t * t + 3.0f * t * t) * p1
			       + (t * t * t - t * t) * v1;
		}

		public static double3 GetVelocity(double3 p0, double3 p1, double3 v0, double3 v1, double t)
		{
			//f(t) = (2t^3-3t^2+1)*p0 + (t^3-2t^2+t)*v0 + (-2*t^3+3t^2)*p1 + (t^3-t^2)*v1
			//f'(t) = (6t^2-6t)*p0 + (3t^2-4t^1+1)*v0 + (-6*t^2+6t^1)*p1 + (3t^2-2t)*v1
			return (6 * t * t - 6 * t) * p0 
			       + (3 * t * t - 4 * t + 1) * v0 
			       + (-6 * t * t + 6 * t) * p1 
			       + (3 * t * t - 2 * t) * v1;
		}

		public static double3 GetAcceleration(double3 p0, double3 p1, double3 v0, double3 v1, double t)
		{
			//f(t) = (2t^3-3t^2+1)*p0 + (t^3-2t^2+t)*v0 + (-2*t^3+3t^2)*p1 + (t^3-t^2)*v1
			//f'(t) = (6t^2-6t)*p0 + (3t^2-4t^1+1)*v0 + (-6*t^2+6t^1)*p1 + (3t^2-2t)*v1
			//f''(t) = (12t-6)*p0 + (6t-4)*v0 + (-12*t+6)*p1 + (6t-2)*v1
			return (12 * t - 6) * p0 
			       + (6 * t - 4) * v0 
			       + (-12 * t + 6) * p1 
			       + (6 * t + 2) * v1;
		}

		public static double DistanceEstimate(double3 p0, double3 p1, double3 v0, double3 v1, double t)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}