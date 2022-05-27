using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Braveo.QOL {

	//ARRAY OF EVENTS, BUT SPECIFICALLY FOR ANIMATION

	public class EventArrayAnimation : MonoBehaviour {

		public QOL.AnimEvent[] events;

		public void Invoke(AnimationEvent ev) {
			if (ev.intParameter >= events.Length) return;
			events[ev.intParameter].Invoke(ev);
		}
		public void InvokeEvent(AnimationEvent ev) {
			Invoke(ev);
		}

		public void InvokeIndex(int index) {

			if (index >= events.Length) return;
			events[index].Invoke(new AnimationEvent());

		}

	}
}