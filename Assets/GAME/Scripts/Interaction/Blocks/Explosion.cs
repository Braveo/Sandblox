using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Explosion : MonoBehaviour {

	public float radius = 3.5f;
	public float force = 150;

	void Start() {

		Collider[] insideSphere = Physics.OverlapSphere(transform.position, radius);

		RaycastHit nil = new RaycastHit();
		nil.point = transform.position;

		float radiusSq = radius * radius;

		for (int i = 0; i < insideSphere.Length; i++) {

			nil.normal = insideSphere[i].transform.position - transform.position;
			float distFromColSq = Vector3.SqrMagnitude(nil.normal);
			
			if (insideSphere[i].TryGetComponent(out IExplodable expl)) {

				expl.Explode(nil, force * (distFromColSq / radiusSq));

			}

			if (insideSphere[i].attachedRigidbody) {

				//insideSphere[i].attachedRigidbody.AddForce(force * nil.normal, ForceMode.Impulse);
				insideSphere[i].attachedRigidbody.AddExplosionForce(force, transform.position, radius, 0, ForceMode.Impulse);

			}

			if (insideSphere[i].TryGetComponent(out PlayerMovement pm)) {

				pm.ApplyExternalForce(force * nil.normal / pm.rb.mass / 2f);
				pm.isGrounded = false;
				pm.trueGrounded = false;

			}

		}

	}

	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, radius);
	}

}
