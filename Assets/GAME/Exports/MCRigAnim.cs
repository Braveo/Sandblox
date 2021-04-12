using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCRigAnim : MonoBehaviour {

	public CamInteraction cam;
	public PlayerMovement pm;
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
		//anim.SetLookAtWeight(0.5f, 0.5f, 0.5f, 0.5f, 0.5f);
		
	}


	void Update() {
		UpdateRotation(pm.currentDirection);
		

		anim.SetFloat("SpeedState", 
			baseRunCurve.Evaluate(pm.currentSpeedState * pm.currentXYInput.sqrMagnitude),
			0.1f,Time.deltaTime);
		anim.SetBool("Grounded", pm.isGroundedTimer < 0.01f);
		anim.SetFloat("SpeedVertical", pm.currentVerticalSpeed);
	}

	void OnAnimatorIK(int layerIndex) {
		//UpdateFacingDir();
		anim.SetLookAtPosition(cam.hasHit ? cam.hitInfo.point : (cam.transform.position + cam.rayPoint.forward * 100));
		anim.SetLookAtWeight(1, 0.25f, 1, 0, 0.5f);
	}

	float smoothAngle, smoothAngleVel;
	internal void UpdateRotation(Vector3 dir) {
		if (dir.sqrMagnitude != 0) {

			float targetAngle = -Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg + 90;
			smoothAngle = Mathf.SmoothDampAngle(smoothAngle, targetAngle, ref smoothAngleVel, 0.1f);

			anim.transform.eulerAngles = new Vector3(0, smoothAngle, 0);

		}
	}
	internal void UpdateFacingDir() {
		//if (cam == null) return;
		
	}

}
