using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CamInteraction : MonoBehaviour {

	public Transform rayPoint;
	internal RaycastHit hitInfo;
	internal bool hasHit;

	void LateUpdate() {
		hasHit = Physics.Raycast(rayPoint.position, rayPoint.forward, out hitInfo, 25);
		if (hasHit) Debug.DrawLine(rayPoint.position, hitInfo.point);
	}

}
