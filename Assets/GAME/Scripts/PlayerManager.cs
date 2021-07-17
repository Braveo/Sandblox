using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour {

	public static PlayerManager single;

	public PlayerMovement pm;
	public SBRigAnim anim;
	public CamInteraction camI;

	void Awake() {
		single = this;
	}

}
