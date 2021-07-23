using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour {

	public CamInteraction camInteraction;
	public GameObject blockPrefab;

	public ItemData currentItem;

	void Awake() {
		
		if (camInteraction) {

			camInteraction.primary.AddListener(PrimaryInteract);
			camInteraction.secondary.AddListener(SecondaryInteract);

		}

	}

	void PrimaryInteract(InputAction.CallbackContext ctx, RaycastHit hit) {

		if (hit.collider) {

			currentItem.PrimaryInteract(this, ctx, hit);

		}

	}

	void SecondaryInteract(InputAction.CallbackContext ctx, RaycastHit hit) {

		if (hit.collider) {

			if (hit.collider.TryGetComponent(out IInteractable b)) {

				b.Interact(hit);

			} else {

				currentItem.SecondaryInteract(this, ctx, hit);

			}

			

		}

	}

	

}
