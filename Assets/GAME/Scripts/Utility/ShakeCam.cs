using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using EZCameraShake;
using Braveo.QOL;

//EZ CAMERA SHAKE: created by Anderson Addo
//Show some support:
//https://www.loadingdeveloper.com/
//https://github.com/andersonaddo

public class ShakeCam : MonoBehaviour {

	public float magnitude = 1f;
	public float roughness = 1f;
	public float fadeInTime = 0.01f;
	public float fadeOutTime = 1f;

	public Optional<float> effectRadius = new Optional<float>(30, true);

	void Start() {
		Shake();
	}

	public void Shake() {

		float effectNumber = effectRadius ? 1 : 
			(effectRadius.val / Vector3.Distance(transform.position, CameraShaker.Instance.transform.position));

		CameraShaker.Instance.ShakeOnce(
			magnitude * effectNumber, 
			roughness, fadeInTime, fadeOutTime);

	}

}
