using Braveo.QOL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FollowObj : MonoBehaviour {

	public Transform t;

	public Optional<float> damping = new Optional<float>(0.2f, false);

	internal Vector3 targetPos, currentPos, velocityPos;

	void LateUpdate() {

		if (damping) {

			//transform.position = Vector3.Lerp(transform.position, t.position, Time.deltaTime * damping.val);
			targetPos = t.position;
			currentPos = Vector3.SmoothDamp(currentPos, targetPos, ref velocityPos, damping.val);

			transform.position = currentPos;

		} else {

			transform.position = t.position;

		}

	}

}
