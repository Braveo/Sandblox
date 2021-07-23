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
	internal UnityEvent<IInteractEnterExit, RaycastHit> crosshairEntering = 
		new UnityEvent<IInteractEnterExit, RaycastHit>();

	void PrimaryInteract(InputAction.CallbackContext ctx) {
		if (enabled) primary.Invoke(ctx, hitInfo);
	}

	void SecondaryInteract(InputAction.CallbackContext ctx) {
		if (enabled) secondary.Invoke(ctx, hitInfo);
	}

	RaycastHit prevHit;
	IInteractEnterExit iee;
	void LateUpdate() {
		//hasHit = Physics.Raycast(rayPoint.position, rayPoint.forward, out hitInfo, 25);
		//if (hasHit) Debug.DrawLine(rayPoint.position, hitInfo.point);
		prevHit = hitInfo;

		hasHit = Physics.Raycast(rayPoint.position, rayPoint.forward, out hitInfo, 25, interactionMask.value);

		if (prevHit.collider != hitInfo.collider) {

			IInteractEnterExit iee = null;

			if (hitInfo.collider) {

				iee = hitInfo.collider.GetComponent<IInteractEnterExit>();

			}

			crosshairEntering.Invoke(iee, hitInfo);

		}
			

		if (hasHit) Debug.DrawLine(rayPoint.position, hitInfo.point);
	}

	public Vector3 GetIKPosition() {
		return hasHit ? hitInfo.point : (transform.position + rayPoint.forward * 100);
	}

}
