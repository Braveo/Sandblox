using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IInteractEnterExit {

	public void Enter(RaycastHit hitInfo);
	public void Exit(RaycastHit hitInfo);

}

public class EInteractEnterExit : MonoBehaviour, IInteractEnterExit {

	[System.Serializable] public class RaycastHitEvent : UnityEvent<RaycastHit> { }

	public RaycastHitEvent onEnter = new RaycastHitEvent();
	public RaycastHitEvent onExit = new RaycastHitEvent();

	public void Enter(RaycastHit hitInfo) {

		onEnter.Invoke(hitInfo);

	}

	public void Exit(RaycastHit hitInfo) {

		onExit.Invoke(hitInfo);

	}
}