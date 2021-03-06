﻿using dninosores.UnityAccessors;
using System.Collections;
using UnityEngine;

namespace dninosores.UnityGameEvents
{
	/// <summary>
	/// Waits for given time in seconds.
	/// </summary>
	public class WaitSeconds : GameEvent
	{
		public FloatOrConstantAccessor time;
		protected override bool InstantInternal => time.Value == 0;

		private bool cancelled;

		public override void Stop()
		{
			cancelled = true;
		}

		protected override IEnumerator RunInternal()
		{
			float t = time.Value;
			cancelled = false;
			while (t > 0 && !cancelled)
			{
				yield return null;
				t -= Time.deltaTime;
			}
		}

		public override void ForceRunInstant()
		{
			// Do nothing, just skip the wait.
		}
	}
}