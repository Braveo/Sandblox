using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IExplodable {

	public void Explode(RaycastHit hitInfo, float force);

}

public class EExplodeable : MonoBehaviour, IExplodable {

	public QOL.FloatEvent onExplode = new QOL.FloatEvent();

	public void Explode(RaycastHit hitInfo, float force) {

		onExplode.Invoke(force);

	}

}
