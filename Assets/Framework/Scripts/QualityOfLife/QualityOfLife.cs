using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

namespace Braveo.QOL { 

	//Quality of life improvements, especially with coroutines and such.
	//UnityEvents, Coroutines (Please use RecurringCoroutine), Evaluations,
	//AnimationCurves, UI Animations, Tweens, Buffers (for premature jumping)

	public static class QOL {

		public enum UpdateType { Update, FixedUpdate, LateUpdate, Manual };
		public enum FrameType { Time, Frames };

		[System.Serializable] public class FloatEvent : UnityEvent<float> { };
		[System.Serializable] public class IntEvent : UnityEvent<int> { };
		[System.Serializable] public class BoolEvent : UnityEvent<bool> { };
		[System.Serializable] public class StringEvent : UnityEvent<string> { };
		[System.Serializable] public class AnimEvent : UnityEvent<AnimationEvent> { };
	
		public static WaitForEndOfFrame WaitForEndOfFrame;
		public static void PlayCoroutine(this MonoBehaviour m, IEnumerator current, IEnumerator function) {
			if (WaitForEndOfFrame == null) WaitForEndOfFrame = new WaitForEndOfFrame();
			if (current != null) {
				m.StopCoroutine(current);
			}
			current = function;
			m.StartCoroutine(current);
		}
		/*public static void PlayCoroutine(this Monobehaviour m, Coroutine current, IEnumerator function) {
			if (WaitForEndOfFrame == null) WaitForEndOfFrame = new WaitForEndOfFrame();
			if (current != null) {
				current.StopCoroutine();
			}
			DO SOMETHING ABOUT THIS
		}*/
		public class RecurringCoroutine {

			[HideInInspector] public bool isPlaying = false;

			IEnumerator current;

			public void Play(MonoBehaviour m, IEnumerator func) {

				if (!m) return;

				if (WaitForEndOfFrame == null) WaitForEndOfFrame = new WaitForEndOfFrame();
				if (isPlaying) {
					m.StopCoroutine(current);
				}
				current = EnPlay(func);
				m.StartCoroutine(current);
			}

			IEnumerator EnPlay(IEnumerator func) {

				isPlaying = true;

				yield return func;

				isPlaying = false;

			}

			public void Stop(MonoBehaviour m) {

				m.StopCoroutine(current);
				isPlaying = false;

			}


		}

		public static void EvaluateAll<T>(System.Action<T> toEvaluate) where T : Object {
			foreach (T t in Object.FindObjectsOfType<T>()) {
				if (t == null) continue;
				toEvaluate.Invoke(t);
			}
		}

		public static void EvaluateData<T>(T[] elements, System.Action<T> toEvaluate) {
			foreach (T t in elements) {
				toEvaluate.Invoke(t);
			}
		}
	
		public static AnimationCurve easeInCurve = new AnimationCurve()
			{keys = new Keyframe[] {new Keyframe(0,0,0,0), new Keyframe(1,1,-2f,2f)} };
		public static AnimationCurve easeOutCurve = new AnimationCurve()
			{keys = new Keyframe[] { new Keyframe(0,0,-2f,2f), new Keyframe(1,1,0,0)} };
		public static AnimationCurve easeInOutCurve = AnimationCurve.EaseInOut(0,0,1,1);
		public static AnimationCurve linearCurve = AnimationCurve.Linear(0,0,1,1);

		[System.Serializable]
		public class ShakeAnimUI {

			public Vector2 multiplier = Vector2.one * 5;
			public float duration = 1;
			public AnimationCurve x = AnimationCurve.Linear(0,0,1,0);
			public AnimationCurve y = AnimationCurve.Linear(0,0,1,0);
			IEnumerator currentAnim;

			public static void Shake(MonoBehaviour m, ShakeAnimUI data, RectTransform t) {
				if (t == null) return;
				if (data == null) return;

				m.PlayCoroutine(data.currentAnim, data.DoAnimation(data, t));
			
			}
			IEnumerator DoAnimation(ShakeAnimUI data, RectTransform t) {
				Vector2 initial = t.localPosition;
				Vector2 addVector = Vector2.zero;
				for (float i = 0; i < data.duration; i += Time.deltaTime) {
					t.localPosition = initial + addVector;
					addVector.x = data.x.Evaluate(i/data.duration) * data.multiplier.x;
					addVector.y = data.y.Evaluate(i/data.duration) * data.multiplier.y;
					yield return WaitForEndOfFrame;
				}
				t.localPosition = initial;
			}

		}
		[System.Serializable]
		public class TweenAnim {

			public TweenAnim() { }

			[HideInInspector] public bool isTweening;

			RecurringCoroutine currentT = new RecurringCoroutine();
			public void Tween(MonoBehaviour m, float time, AnimationCurve ease, System.Action<float> action, System.Action onEnd = null, bool unscaledTime = false) {
				if (ease == null) return;
				if (action == null) return;

				//m.PlayCoroutine(currentTween, EnTween(time, ease, action, onEnd));

				currentT.Play(m, EnTween(time, ease, action, onEnd));
				
			}

			public void Stop(MonoBehaviour m) {
				if (!m) return;
				//if (currentTween == null) return;
				isTweening = false;
				//m.StopCoroutine(currentTween);
				currentT.Stop(m);
			}

			IEnumerator EnTween(float time, AnimationCurve easing, System.Action<float> action, System.Action onEnd = null, bool unscaledTime = false) {

				isTweening = true;

				for (float i = 0; i < time; i += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime) {
					action.Invoke(easing.Evaluate(i/time));
					yield return WaitForEndOfFrame;
				}

				isTweening = false;

				action.Invoke(easing.Evaluate(1));
				if (onEnd != null) onEnd.Invoke();
			}
		}

		[System.Serializable]	
		public abstract class Buffer {

			[HideInInspector] public bool active;
			[HideInInspector] public float timeSinceAct;
			public float bufferTime = 0.2f;

			public Buffer(float bTime = 0.2f) {
				active = false;
				timeSinceAct = 0;
				bufferTime = bTime;
			}

			public void Process(bool pressed, float delta) {
				timeSinceAct = pressed ? (timeSinceAct + delta) : 0;
				active = GetActiveValue(bufferTime);
			}

			/// <summary>
			/// Turns the time back to 0
			/// </summary>
			public void ResetBuffer() => timeSinceAct = 0;
			public void CancelBuffer() => timeSinceAct = bufferTime;
			public bool GetActive() => active;

			public abstract bool GetActiveValue(float buffer);

		}
		[System.Serializable]	//When handling inputs: defaults off, window of on, then off
		public class InputBuffer : Buffer {

			public InputBuffer(float bTime = 0.2f) : base(bTime) { }

			public override bool GetActiveValue(float buffer) {
				return timeSinceAct < buffer && timeSinceAct > 0;
			}

		}
		[System.Serializable]	//Defaults on, but off after time
		public class BooleanBuffer : Buffer {

			public BooleanBuffer(float bTime = 0.2f) : base(bTime) { }

			public override bool GetActiveValue(float buffer) {
				return timeSinceAct < buffer;
			}

		}
	}


}