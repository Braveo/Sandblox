using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FrameManager : MonoBehaviour {

	void Awake() {
		Application.targetFrameRate = 60;
	}

}
