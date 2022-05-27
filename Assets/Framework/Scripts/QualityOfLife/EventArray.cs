using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Braveo.QOL {

	//ARRAY OF EVENTS

	public class EventArray : MonoBehaviour {

		public UnityEvent[] events;

		public void InvokeEvent(int index) {
			if (index >= events.Length) return;
			events[index].Invoke();
		}

	}
}