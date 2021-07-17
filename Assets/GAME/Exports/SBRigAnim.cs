using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SBRigAnim : MonoBehaviour {

	public CamInteraction cam;
	public PlayerMovement pm;
	public PlayerInteraction pi;
	internal Animator anim;
	public float movementVal = 0;
	
	internal AnimationCurve baseRunCurve;

	public void InitializeAnimCurves() {
		baseRunCurve = new AnimationCurve(new Keyframe[] {
			new Keyframe(0,0),
			new Keyframe(pm.walkSpeed/8f, 0.5f),
			new Keyframe(pm.walkSpeed, 0.5f),
			new Keyframe((pm.runSpeed+pm.walkSpeed)/2f, 1f)
		});
	}

	void Awake() {
		anim = GetComponent<Animator>();
		InitializeAnimCurves();
		pm.events.onJump.AddListener(() => anim.SetBool("MirrorJumping", !anim.GetBool("MirrorJumping")));

		cam.primary.AddListener(PrimaryAnimation);
		cam.secondary.AddListener(SecondaryAnimation);
		
	}


	QOL.TweenAnim layerTween = new QOL.TweenAnim();

	void PrimaryAnimation(InputAction.CallbackContext ctx, RaycastHit hit) {

		if (pi) {

			if (pi.item) {

				anim.Update(0);
				anim.CrossFade(pi.item.animPrimary, 0.075f, 1);

			}

		}

	}

	void SecondaryAnimation(InputAction.CallbackContext ctx, RaycastHit hit) {

		if (pi) {

			if (pi.item) {

				anim.Update(0);
				anim.CrossFade(pi.item.animSecondary, 0.075f, 1);

			}

		}

	}

	float dotTimer;
	void Update() {
		UpdateRotation(pm.currentDirection);
		
		/*anim.SetFloat("SpeedState", 
			baseRunCurve.Evaluate(pm.currentSpeedState * pm.currentXYInput.sqrMagnitude),
			0.1f,Time.deltaTime);*/

		anim.SetFloat("SpeedState", 
			baseRunCurve.Evaluate(new Vector3(pm.rb.velocity.x, 0, pm.rb.velocity.z).magnitude) * Mathf.Sign(pm.currentXYInput.sqrMagnitude),
			0.1f,Time.deltaTime);
		anim.SetBool("Grounded", pm.isGroundedTimer < 0.03f);
		anim.SetFloat("SpeedVertical", pm.currentVerticalSpeed);
		
		if (Vector3.Dot(pm.currentDirection, anim.transform.forward) < -0.25f) {
			anim.SetBool("Skidding", dotTimer < 0.05f && pm.currentSpeedState > pm.walkSpeed);
			dotTimer += Time.deltaTime;
		} else {
			anim.SetBool("Skidding", false);
			dotTimer = 0;
		}

		Debug.DrawLine(transform.position, transform.position + pm.currentDirection, Color.blue);
		Debug.DrawLine(transform.position, transform.position + pm.smoothDirection, Color.red);

	}

	float smoothAngle, smoothAngleVel;
	internal void UpdateRotation(Vector3 dir) {
		if (dir.sqrMagnitude != 0) {

			float targetAngle = -Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg + 90;
			float smoothDamping = pm.skidding ? 0.2f : 0.1f;
			smoothAngle = Mathf.SmoothDampAngle(smoothAngle, targetAngle, ref smoothAngleVel, smoothDamping);

			anim.transform.eulerAngles = new Vector3(0, smoothAngle, 0);

		}
	}


	void OnAnimatorIK(int layerIndex) {
		UpdateFacingDir();
		UpdateArms();
	}
	Vector3 lookPositionRaw;
	Vector3 lookPositionSmoothed, lookPositionVel;
	internal void UpdateFacingDir() {
		Vector3 upOffset = transform.up * 1.4f;

		lookPositionRaw = cam.GetIKPosition();
		lookPositionRaw = (lookPositionRaw - transform.position - upOffset).normalized;

		lookPositionSmoothed = Vector3.SmoothDamp(lookPositionSmoothed, lookPositionRaw, ref lookPositionVel, 0.15f).normalized;

		Debug.DrawLine(transform.position, transform.position + upOffset + lookPositionRaw, Color.red);
		Debug.DrawLine(transform.position, transform.position + upOffset + lookPositionSmoothed, Color.green);

		anim.SetLookAtPosition(lookPositionSmoothed + transform.position + upOffset);
		anim.SetLookAtWeight(1, 0.25f, 1, 0, 0.5f);
	}

	internal float lPointSmoothing, rPointSmoothing;
	internal void UpdateArms() {

		//TRY DOING SOME IN UPDATE AND SOME IN THIS FUNCTION

		/*RaycastHit lF, lH, rF, rH;
		Vector3 lPoint = Vector3.zero, rPoint = Vector3.zero;

		Vector3 lArmCenter = transform.position + transform.up - transform.right * 0.25f;
		Vector3 rArmCenter = transform.position + transform.up + transform.right * 0.25f;

		float pointSmoothingSpeed = 2f;

		float armLength = 0.5f;

		if (Physics.Raycast(lArmCenter, transform.forward-transform.right*0.2f, out lF, armLength)) {
			lPoint = lF.point + lF.normal*0.15f;
			Debug.DrawLine(lArmCenter, lF.point);
		}
		if (Physics.Raycast(lArmCenter, -transform.right, out lH, armLength)) {
			Vector3 determineVal = lH.point + lH.normal*0.15f;
			if (Vector3.SqrMagnitude(lPoint - lArmCenter) > Vector3.SqrMagnitude(determineVal - lArmCenter))
				lPoint = determineVal;
			Debug.DrawLine(lArmCenter, lH.point);
		}
		if (Physics.Raycast(rArmCenter, transform.forward+transform.right*0.2f, out rF, armLength)) {
			rPoint = rF.point + rF.normal*0.15f;
			Debug.DrawLine(rArmCenter, rF.point);
		}
		if (Physics.Raycast(rArmCenter, transform.right, out rH, armLength)) {
			Vector3 determineVal = rH.point + rH.normal*0.15f;
			if (Vector3.SqrMagnitude(rPoint - rArmCenter) > Vector3.SqrMagnitude(determineVal - rArmCenter))
				rPoint = determineVal;
			Debug.DrawLine(rArmCenter, rH.point);
		}

		if (lPoint != Vector3.zero) {
			anim.SetIKPosition(AvatarIKGoal.LeftHand, lPoint);
			lPointSmoothing += Time.deltaTime * pointSmoothingSpeed;
		} else {
			lPointSmoothing -= Time.deltaTime * pointSmoothingSpeed;
		}
		Mathf.Clamp01(lPointSmoothing);
		anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, lPointSmoothing);

		if (rPoint != Vector3.zero) {
			anim.SetIKPosition(AvatarIKGoal.RightHand, rPoint);
			rPointSmoothing += Time.deltaTime * pointSmoothingSpeed;
		} else {
			rPointSmoothing -= Time.deltaTime;
		}
		Mathf.Clamp01(rPointSmoothing);
		anim.SetIKPositionWeight(AvatarIKGoal.RightHand, rPointSmoothing);
		*/
	}

}
