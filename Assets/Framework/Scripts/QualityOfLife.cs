using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

//Quality of life improvements, especially with coroutines and such.
public static class QOL {

	[System.Serializable] public class FloatEvent : UnityEvent<float> { };
	[System.Serializable] public class IntEvent : UnityEvent<int> { };
	[System.Serializable] public class BoolEvent : UnityEvent<bool> { };
	[System.Serializable] public class StringEvent : UnityEvent<string> { };
	[System.Serializable] public class AnimEvent : UnityEvent<AnimationEvent> { };
	
	public static WaitForEndOfFrame WaitForEndOfFrame;
	public static void PlayCoroutine(this MonoBehaviour m, IEnumerator current, IEnumerator function) {
		if (WaitForEndOfFrame == null) WaitForEndOfFrame = new WaitForEndOfFrame();
		if (current != null) m.StopCoroutine(current);
		current = function;
		m.StartCoroutine(current);
	}

	public static void EvaluateComponents<T>(System.Action<T> toEvaluate) where T : Object {
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

		public void Tween(MonoBehaviour m, float time, AnimationCurve ease, System.Action<float> action, System.Action onEnd = null) {
			if (ease == null) return;
			if (action == null) return;

			m.PlayCoroutine(currentTween, EnTween(time, ease, action, onEnd));
		}

		IEnumerator currentTween;
		IEnumerator EnTween(float time, AnimationCurve easing, System.Action<float> action, System.Action onEnd = null) {
			for (float i = 0; i < time; i += Time.deltaTime) {
				action.Invoke(easing.Evaluate(i/time));
				yield return WaitForEndOfFrame;
			}
			action.Invoke(easing.Evaluate(1));
			if (onEnd != null) onEnd.Invoke();
		}
	}

	[System.Serializable]
	public abstract class Buffer {

		internal bool active;
		internal float timeSinceAct;
		public float bufferTime = 0.2f;

		public Buffer(float bTime = 0.2f) {
			active = false;
			timeSinceAct = 0;
			bufferTime = bTime;
		}

		public abstract void Process(bool pressed, float delta);

		public void CancelBuffer() => timeSinceAct = bufferTime;
		public bool GetActive() => active;

	}
	[System.Serializable]	//When handling inputs: defaults off, window of on, then off
	public class InputBuffer : Buffer {

		public InputBuffer(float bTime = 0.2f) : base(bTime) { }

		public override void Process(bool pressed, float delta) {
			timeSinceAct = pressed ? (timeSinceAct + delta) : 0;
			active = timeSinceAct < bufferTime && timeSinceAct > 0;
		}

	}
	[System.Serializable]	//Defaults on, but off after time
	public class BooleanBuffer : Buffer {

		public BooleanBuffer(float bTime = 0.2f) : base(bTime) { }

		public override void Process(bool pressed, float delta) {
			timeSinceAct = pressed ? (timeSinceAct + delta) : 0;
			active = timeSinceAct < bufferTime;
		}

	}
}

[System.Serializable]
public struct Optional<T> {

	[SerializeField] bool enabled;
	[SerializeField] T value;

	public Optional(T initial, bool startEnabled = true) {

		enabled = startEnabled;
		value = initial;

	}

	public bool Enabled => enabled;
	public T val {
		get => value;
		set => this.value = value;
	}

	public static implicit operator bool(Optional<T> o) => o.Enabled;
	public static explicit operator T(Optional<T> o) => o.value;

}