using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Braveo.QOL {

	//STATE MACHINE THAT ALLOWS FOR OBJECTS TO TARGET MULTIPLE AREAS.
	//IT'S LIKE A CAMERA SWAPPER THING

	public class TargetStateMachine : MonoBehaviour {

		public bool isCamera = false;
		public static TargetStateMachine cam;

		[System.Serializable]
		public struct AnimationData {

			public float time;
			public AnimationCurve curve;

			public AnimationData(float t, AnimationCurve c) {

				time = t;
				curve = c;

			}

			public static AnimationData defaultValue = new AnimationData(0.5f, AnimationCurve.Linear(0,0,1,1));
			
			public static AnimationData GetLinear(float t) {

				return new AnimationData(t, QOL.linearCurve);

			}

			public static AnimationData GetEased(float t) {

				return new AnimationData(t, QOL.easeInOutCurve);

			}

			public static AnimationData GetEaseOut(float t) {

				return new AnimationData(t, QOL.easeOutCurve);
			}

			public static AnimationData GetEaseIn(float t) {

				return new AnimationData(t, QOL.easeInCurve);
			}

		}

		public Transform currentTarget;
		int storedIndex = 0;
		internal QOL.TweenAnim tween = new QOL.TweenAnim();

		public AnimationData[] animData = new AnimationData[1] {AnimationData.defaultValue};

		void Awake() {

			if (isCamera) cam = this;
			SetTargetImmediate(currentTarget);

		}

		public void SetTargetFromStoredIndex(Transform t) => SetTargetFromIndex(t, storedIndex);
		public void SetStoredIndex(int i) => storedIndex = i;
		public int StoredIndex { get => storedIndex; }

		public void SetTargetFromIndex(Transform t, int index) {

			if (!(index > 0 && index < animData.Length)) return;

			SetTarget(t, animData[index]);

		}

		public void SetTarget(Transform t, AnimationData data) {

			if (!t) return;

			//IF CURRENTLY IN A TRANSITION, USE STRUCTS AS THEY DO NOT REQUIRE REFERENCE
			//TO THE PARENT YOU'RE CURRENTLY IN

			bool nullParent = !transform.parent;

			Vector3 pPosition = transform.position;
			Quaternion pRotation = transform.rotation;
			
			//USING TARGETS

			Transform oldT = currentTarget;
			Transform newT = t;

			transform.SetParent(null);

			//THE TWEENING

			tween.Tween(this, data.time, data.curve, ctx => {

				transform.position = Vector3.Lerp(nullParent ? pPosition : oldT.position, newT.position, ctx);
				transform.rotation = Quaternion.Lerp(nullParent ? pRotation : oldT.rotation, newT.rotation, ctx);

			}, () => {

				transform.SetParent(t);
				currentTarget = newT;

			}, false);

			

		}

		public void SetTargetImmediate(Transform t) {

			if (!t) return;

			transform.SetParent(t);
			transform.localPosition = Vector3.zero;
			transform.localEulerAngles = Vector3.zero;

		}

	}
}