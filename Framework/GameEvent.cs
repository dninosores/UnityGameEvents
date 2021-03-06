﻿using dninosores.UnityAccessors;
using System.Collections;
using UnityEngine;

namespace dninosores.UnityGameEvents
{

	/// <summary>
	/// Class similar to a coroutine that allows for methods to be executed instantly or over multiple frames.
	/// </summary>
	public abstract class GameEvent : MonoBehaviour
	{
		/// <summary>
		/// The GameEvent that this GameEvent is nested inside of, or null if there is no nesting.
		/// </summary>
		public GameEvent ParentEvent
		{
			get;
			private set;
		}


		/// <summary>
		/// Is the GameEvent currently running?
		/// </summary>
		public bool InProgress
		{
			get;
			private set;
		}


		/// <summary>
		/// Should the game wait for the GameEvent to finish before moving on?
		/// </summary>
		public bool waitForCompletion = true;


		/// <summary>
		/// Should the event play itself when the game starts?
		/// </summary>
		public bool playOnStart = false;


		/// <summary>
		/// Should the GameEvent be executed in a single frame? If so, use RunInstant(), otherwise use Run()
		/// </summary>
		public bool Instant => !isActiveAndEnabled || InstantInternal || !waitForCompletion;

		
		/// <summary>
		/// Is the game event currently fast-forwarding?
		/// </summary>
		protected bool fastForwarding;

		/// <summary>
		/// Should the RunInternal() method be executed in a single frame?
		/// </summary>
		protected abstract bool InstantInternal
		{
			get;
		}


		protected virtual void Start()
		{
			if (playOnStart)
			{
				StartRunning();
			}
		}


		/// <summary>
		/// Start running event in appropriate mode.
		/// </summary>
		public void StartRunning()
		{
			if (Instant)
			{
				RunInstant();
			}
			else
			{
				StartCoroutine(Run());
			}
		}


		/// <summary>
		/// Run at the beginning of scene before awake. Don't forget to call base.Awake() in your override!
		/// </summary>
		protected virtual void Awake()
		{
			InProgress = false;
		}


		/// <summary>
		/// Resets fields back to default values. Automatically resets all Accessors. Don't forget to call base.Reset() in your override!
		/// </summary>
		protected virtual void Reset()
		{
			ParentEvent = null;
			InProgress = false;
			waitForCompletion = true;
			playOnStart = false;
			ResetAccessors.Reset(this, this);
		}


		/// <summary>
		/// Coroutine that will run GameEvent and exit when the event is complete. Ensures that only one instance of coroutine is active at any time.
		/// </summary>
		/// <returns></returns>
		public IEnumerator Run()
		{
			if (!InProgress)
			{
				InProgress = true;
				yield return RunInternal();
				InProgress = false;
			}
			else
			{
				yield break;
			}
		}


		/// <summary>
		/// Runs entire event in a single frame or starts the event in parallel depending on the event's settings.
		/// If attempting to run the entire RunInternal coroutine in a single frame would create an infinite loop,
		/// make sure to override the ForceRunInstant method.
		/// </summary>
		public void RunInstant()
		{
			if (isActiveAndEnabled)
			{
				if (!InstantInternal && !waitForCompletion)
				{
					StartCoroutine(Run());
				}
				else if (!InProgress)
				{
					ForceRunInstant();
				}
			}
		}


		/// <summary>
		/// Runs entire Run() Coroutine in a single frame.
		/// </summary>
		public virtual void ForceRunInstant()
		{
			ForceRunInstant(Run());
		}


		/// <summary>
		/// Runs entire coroutine in a single frame. In order to run nested coroutines, yield the IEnumerator directly;
		/// yielding StartCoroutine() will start the coroutine but not wait for completion.
		/// </summary>
		/// <param name="routine"></param>
		public void ForceRunInstant(IEnumerator routine)
		{
			while (routine.MoveNext())
			{
				if (routine.Current is IEnumerator nested)
				{
					ForceRunInstant(nested);
				}
				if (routine.Current is Coroutine rout)
				{
					
				}
			}
		}


		/// <summary>
		/// Coroutine that defines the actual behavior of the GameEvent. If attempting to run the entire Coroutine in a single frame
		/// would lead to an infinite loop, you must also override the ForceRunInstant method to specify how the GameEvent should behave
		/// if it's supposed to be executed in one frame for fast-forwarding.
		/// </summary>
		protected abstract IEnumerator RunInternal();


		/// <summary>
		/// Tell GameEvent which GameEvent it's nested in, or set to null if GameEvent isn't nested.
		/// </summary>
		/// <param name="gameEvent">GameEvent that this GameEvent is nested in.</param>
		public void SetParentEvent(GameEvent gameEvent)
		{
			ParentEvent = gameEvent;
		}


		/// <summary>
		/// Immediately stops the GameEvent, leaving all affected objects in their current state.
		/// </summary>
		public abstract void Stop();


		/// <summary>
		/// Tells all GameEvents in the same sequence as this one to start running instantly until StopFastForward is called.
		/// </summary>
		public virtual void StartFastForward()
		{
			ParentEvent?.StartFastForward();
			fastForwarding = true;
		}


		/// <summary>
		/// Tells all GameEvents in the same sequence as this one to stop fast-forwarding.
		/// </summary>
		public virtual void StopFastForward()
		{
			ParentEvent?.StopFastForward();
			fastForwarding = false;
		}


		/// <summary>
		/// Fast-forwards GameEvent. This is similar to ForceRunInstant(), but allows for a return to normal execution partway through.
		/// This should only be used for testing.
		/// </summary>
		/// <returns></returns>
		public virtual IEnumerator FastForward()
		{
			ForceRunInstant();
			yield break;
		}
	}
}
