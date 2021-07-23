using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IInteractable {

	public void Interact(RaycastHit hitInfo);

}

public class EInteractable : MonoBehaviour, IInteractable, IInteractEnterExit {

	[System.Serializable] public class RaycastHitEvent : UnityEvent<RaycastHit> { }

	public RaycastHitEvent onInteract = new RaycastHitEvent();
	public string interactAnim = "Place";

	public void Interact(RaycastHit hitInfo) {

		onInteract.Invoke(hitInfo);

	}

	public void Enter(RaycastHit hitInfo) {
		
	}

	public void Exit(RaycastHit hitInfo) {
		
	}
}
