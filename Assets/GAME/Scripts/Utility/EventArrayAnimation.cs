using Braveo.QOL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventArrayAnimation : MonoBehaviour {

	public QOL.AnimEvent[] events;

	public void InvokeEvent(AnimationEvent ev) {
		if (ev.intParameter >= events.Length) return;
		events[ev.intParameter].Invoke(ev);
	}

}
