﻿using dninosores.UnityAccessors;
using UnityEngine;

namespace dninosores.UnityGameEvents
{
	/// <summary>
	/// Interpolates a Vector3 between two values.
	/// </summary>
	public abstract class AbstractVector3Interpolator : AbstractInterpolator<Vector3>
	{
		protected override Vector3 Interpolate(Vector3 start, Vector3 end, float fraction)
		{
			return InterpolateVector3(start, end, fraction);
		}

		protected override Vector3 Multiply(Vector3 a, Vector3 b)
		{
			return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
		}


		public static Vector3 InterpolateVector3(Vector3 start, Vector3 end, float fraction)
		{
			return (end - start) * (fraction) + start;
		}
	}
}