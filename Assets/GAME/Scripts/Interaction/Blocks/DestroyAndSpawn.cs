using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestroyAndSpawn : MonoBehaviour {

	public GameObject toSpawn;

	public void Destroy() {

		GameObject spawned = Instantiate(toSpawn, transform.position, Quaternion.identity, null);
		Destroy(gameObject);

	}

}
