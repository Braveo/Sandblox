using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CamRaycast : MonoBehaviour {

	public Transform centerPoint;
	public Transform initialTransform;
	public LayerMask hitLayers;
	
	Vector3 rayDir; float rayDirDist;
	RaycastHit rayHit;

	void Awake() {
		Initiate();
	}

	void LateUpdate() {

		if (Physics.Raycast (centerPoint.position, initialTransform.parent.TransformDirection(rayDir), 
			out rayHit, rayDirDist, hitLayers.value)) {

			initialTransform.position = rayHit.point+rayHit.normal*0.05f;
		} else {
			initialTransform.localPosition = rayDir;
		}
	}

	void Initiate() {
		rayDir = initialTransform.localPosition;
		rayDirDist = rayDir.magnitude;
	}

	public void SwitchTransform(Transform t) {
		if (t == null) return;
		Debug.Log($"Switched to transform {t.gameObject.name}");
		rayDir = t.localPosition;
		rayDirDist = rayDir.magnitude;
	}

	/*public void SmoothTransform(Transform t, float time, AnimationCurve easing) {
		QOL.PlayCoroutine(this, currentSmooth, EnSmoothTransform(t, time, easing));
	}

	IEnumerator currentSmooth;
	IEnumerator EnSmoothTransform(Transform t, float time, AnimationCurve easing) {
		Vector3 initial = rayDir;
		float initialMag = rayDirDist;

		Vector3 final = t.localPosition;
		float finalMag = final.magnitude;

		for (float i = 0; i < time; i += Time.deltaTime) {
			rayDir = Vector3.Lerp(initial, final, easing.Evaluate(i/time));
			rayDirDist = Mathf.Lerp(initialMag, finalMag, easing.Evaluate(i/time));
			yield return QOL.WaitForEndOfFrame;
		}

		SwitchTransform(t);
	}*/

	QOL.TweenAnim currentTween = new QOL.TweenAnim();
	public void SmoothTransform(Transform t, float time, AnimationCurve ease) {

		Vector3 initial = rayDir;
		float initialMag = rayDirDist;

		Vector3 final = t.localPosition;
		float finalMag = final.magnitude;

		currentTween.Tween(this, time, ease, ctx => {
			rayDir = Vector3.Lerp(initial, final, ctx);
			rayDirDist = Mathf.Lerp(initialMag, finalMag, ctx);
		});
	}

}
