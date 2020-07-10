﻿using System.Collections;
using UnityEngine;

namespace dninosores.UnityGameEvents
{
	/// <summary>
	/// Base class for game events that will always be executed in one frame. 
	/// Can optionally specify a delay in seconds to wait for after the event is executed.
	/// </summary>
	public abstract class InstantGameEvent : GameEvent
	{
		protected override bool InstantInternal => true;
		public float delay = 0;
		private bool cancelled;

		public override void Stop()
		{
			cancelled = true;
		}

		protected override IEnumerator RunInternal()
		{
			cancelled = false;
			InstantEvent();
			float time = delay;
			while (time > 0 && !cancelled)
			{
				yield return null;
				time -= Time.deltaTime;
			}

		}

		/// <summary>
		/// Method that this GameEvent executes.
		/// </summary>
		protected abstract void InstantEvent();
	}
}