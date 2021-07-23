using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestroyOn : MonoBehaviour {

	public UnityEvent onDestroy;

	void OnDestroy() {

		onDestroy.Invoke();

	}

}
