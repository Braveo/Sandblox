using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestroyTimer : MonoBehaviour {

	public bool playOnAwake = true;
	public float timeToDestroy = 3;

	void Start() {

		if (playOnAwake) Destroy();

	}

	public void Destroy() {

		Destroy(gameObject, timeToDestroy);

	}

}
