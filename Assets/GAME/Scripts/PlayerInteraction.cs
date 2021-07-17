using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour {

	public CamInteraction camInteraction;
	public GameObject blockPrefab;

	public ItemData item;

	void Awake() {
		
		if (camInteraction) {

			camInteraction.primary.AddListener(PrimaryInteract);
			camInteraction.secondary.AddListener(SecondaryInteract);

		}

	}

	void PrimaryInteract(InputAction.CallbackContext ctx, RaycastHit hit) {

		if (hit.collider) {

			if (hit.collider.TryGetComponent(out IBreakable b)) {

				b.Break(hit);

			}

		}

	}

	void SecondaryInteract(InputAction.CallbackContext ctx, RaycastHit hit) {

		if (hit.collider) {

			Vector3 rawPos = hit.point + hit.normal * 0.5f;

			GameObject block = Instantiate(blockPrefab, 
				RoundVector3(rawPos + Vector3.one * 0.5f) - Vector3.one * 0.5f, 
				Quaternion.identity, null);




		}

	}

	public static Vector3 RoundVector3(Vector3 val) {

		return new Vector3(Mathf.Round(val.x), Mathf.Round(val.y), Mathf.Round(val.z));

	}

}
