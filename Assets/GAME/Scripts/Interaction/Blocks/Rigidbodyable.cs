using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Rigidbodyable : MonoBehaviour {

	public float mass = 15;
	Rigidbody rb;


	public void TurnIntoRigidbody() {

		if (rb) return;

		rb = gameObject.AddComponent<Rigidbody>();

		if (rb) {

			rb.mass = mass;
			rb.interpolation = RigidbodyInterpolation.Interpolate;

		}

	}

}
