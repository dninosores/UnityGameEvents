﻿#pragma warning disable 0649
using dninosores.UnityAccessors;

namespace dninosores.UnityGameEvents
{
	/// <summary>
	/// Sets a float value using an Accessor.
	/// </summary>
	public class SetFloat : AddMultiplySetValueEvent<float>
	{
		public AnyFloatAccessor accessor;
		public FloatOrConstantAccessor Value;
		protected override Accessor<float> valueAccessor => accessor;

		protected override float value => Value.Value;
	}
}
