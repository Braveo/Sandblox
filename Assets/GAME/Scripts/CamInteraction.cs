using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CamInteraction : MonoBehaviour {

	public Transform rayPoint;
	public PlayerInput pi;
	public LayerMask interactionMask;
	internal RaycastHit hitInfo;
	internal bool hasHit;

	void Start() {
		if (pi != null) {
			pi.actions["LClick"].started += PrimaryInteract;
			pi.actions["RClick"].started += SecondaryInteract;
		}
	}


	internal UnityEvent<InputAction.CallbackContext, RaycastHit> primary = 
		new UnityEvent<InputAction.CallbackContext, RaycastHit>();
	internal UnityEvent<InputAction.CallbackContext, RaycastHit> secondary = 
		new UnityEvent<InputAction.CallbackContext, RaycastHit>();

	void PrimaryInteract(InputAction.CallbackContext ctx) {
		if (enabled) primary.Invoke(ctx, hitInfo);
	}

	void SecondaryInteract(InputAction.CallbackContext ctx) {
		if (enabled) secondary.Invoke(ctx, hitInfo);
	}

	void LateUpdate() {
		//hasHit = Physics.Raycast(rayPoint.position, rayPoint.forward, out hitInfo, 25);
		//if (hasHit) Debug.DrawLine(rayPoint.position, hitInfo.point);

		bool hasHit = Physics.Raycast(rayPoint.position, rayPoint.forward, out hitInfo, 25, interactionMask.value);

		if (hasHit) Debug.DrawLine(rayPoint.position, hitInfo.point);
	}

	public Vector3 GetIKPosition() {
		return hasHit ? hitInfo.point : (transform.position + rayPoint.forward * 100);
	}

}
