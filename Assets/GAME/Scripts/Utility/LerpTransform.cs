using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Braveo.QOL;

public class LerpTransform : MonoBehaviour {

	public Transform pA;
	public Transform pB;
	public AnimationCurve tweenCurve = QOL.linearCurve;
	public float time = 1;
	public UnityEvent onEnd = new UnityEvent();
	char currentlySelected = 'A';

	QOL.TweenAnim tween = new QOL.TweenAnim();

	public void LerpToA() => LerpTo(pA, time, tweenCurve);
	public void LerpToB() => LerpTo(pB, time, tweenCurve);
	public void LerpSwap() => LerpTo(currentlySelected == 'A' ? pB : pA, time, tweenCurve);

	public void LerpTo(Transform t, float time, AnimationCurve aCurve) {

		currentlySelected = (t == pA) ? ('A') : (t == pB ? 'B' : currentlySelected);

		Vector3 initPos = transform.position;
		Quaternion initRot = transform.rotation;

		tween.Tween(this, time, aCurve, ctx => {
			transform.position = Vector3.Lerp(initPos, t.position, ctx);
			transform.rotation = Quaternion.Lerp(initRot, t.rotation, ctx);
		}, () => onEnd.Invoke());

	}

}
