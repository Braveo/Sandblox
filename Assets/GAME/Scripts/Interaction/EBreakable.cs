using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IBreakable {

	public void Break(RaycastHit hitInfo);

}

public class EBreakable : MonoBehaviour, IBreakable {

	[System.Serializable] public class RaycastHitEvent : UnityEvent<RaycastHit> { }

	public RaycastHitEvent onBreak = new RaycastHitEvent();

	public void Break(RaycastHit hitInfo) {

		onBreak.Invoke(hitInfo);

	}

}
