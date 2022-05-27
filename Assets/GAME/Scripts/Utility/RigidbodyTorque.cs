using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RigidbodyTorque : MonoBehaviour {

	public Vector3 torque = Vector3.right * 270;
	Vector3 currentRot;
	Rigidbody rb;

	void Start() {
		if (TryGetComponent(out Rigidbodyable rba)) {

			rba.TurnIntoRigidbody();
			rba.Kinematic();

		}
		rb = GetComponent<Rigidbody>();
	}

	void FixedUpdate() {
		rb.angularVelocity = torque;
		//currentRot += torque * Time.fixedDeltaTime;
		//rb.rotation = Quaternion.Euler(currentRot);
	}

}
