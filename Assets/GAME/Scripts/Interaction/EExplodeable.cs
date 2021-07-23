using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IExplodable {

	public void Explode(RaycastHit hitInfo, float force);

}

public class EExplodeable : MonoBehaviour, IExplodable {

	[System.Serializable]
	public class ExplosionEvent : UnityEvent<RaycastHit, float> { };

	public ExplosionEvent onExplode = new ExplosionEvent();


	public void Explode(RaycastHit hitInfo, float force) {

		onExplode.Invoke(hitInfo, force);

	}

}
