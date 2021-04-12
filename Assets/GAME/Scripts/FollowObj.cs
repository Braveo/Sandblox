using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FollowObj : MonoBehaviour {

	public Transform t;

	void LateUpdate() {
		transform.position = t.position;
	}

}
