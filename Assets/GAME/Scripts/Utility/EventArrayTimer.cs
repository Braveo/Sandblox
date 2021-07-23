using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(EventArray))]
public class EventArrayTimer : MonoBehaviour {

	internal EventArray ea;

	public float timeBetweenEvent = 0.5f;
	internal float currentTime;
	internal int currentIndex = 0;

	void Awake() {
		ea = GetComponent<EventArray>();
	}

	void Update() {

		currentTime += Time.deltaTime;

		if (currentTime > timeBetweenEvent) {

			ea.InvokeEvent(currentIndex);

			currentTime = 0;

			currentIndex += 1;

			if (currentIndex >= ea.events.Length) currentIndex = 0;
			if (currentIndex < 0) currentIndex = 0;

		}

		

	}

}
