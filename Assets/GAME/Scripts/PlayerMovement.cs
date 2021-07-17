using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour {

	[System.Serializable]
	public class Events {
		public UnityEvent onJump = new UnityEvent();
		public QOL.FloatEvent onLand = new QOL.FloatEvent();
	}
	public Events events = new Events();

	internal Rigidbody rb;
	internal PlayerInput pi;

	public Transform relativeForward;

	public float walkSpeed = 6;
	public float runSpeed = 10;
	[Space(10)]
	public float jumpSpeed = 5;
	public float gravityMultiplier = 3;

	internal float currentSpeedState;
	internal Vector2 currentXYInput;
	internal Vector3 currentDirection;
	internal float currentVerticalSpeed;

	internal bool trueGrounded, isGrounded;
	internal float isGroundedTimer;
	internal RaycastHit isGroundedHit;

	internal bool holdingJump;
	internal float holdingJumpTimer;


	void Awake() {
		rb = GetComponent<Rigidbody>();
		pi = GetComponent<PlayerInput>();

		currentSpeedState = walkSpeed;
		currentXYInput = Vector2.zero;

		pi.actions["Shift"].performed += ctx => currentSpeedState = ctx.ReadValueAsButton() ? runSpeed : walkSpeed;
		pi.actions["Move"].performed += ctx => currentXYInput = ctx.ReadValue<Vector2>();
		pi.actions["Space"].started += ctx => holdingJump = true;
		pi.actions["Space"].canceled += ctx => holdingJump = false;
	}

	void FixedUpdate() {

		GroundDetection();
		JumpOperations();

		
		
		SmoothMove(currentXYInput * currentSpeedState);

		if (pi.currentActionMap.name != "Player") currentXYInput = Vector2.zero;
		//rb.velocity = new Vector3(smoothDirection.x, currentVerticalSpeed, smoothDirection.z);
		rb.velocity = planeNormalX * smoothDirection.x + planeNormal * currentVerticalSpeed + planeNormalZ * smoothDirection.z;
	}

	internal Vector3 smoothDirection, smoothDirectionVel;
	internal bool skidding;
	public void SmoothMove(Vector2 m) {
		if (!rb) return;
		currentDirection = relativeForward.TransformDirection(new Vector3(m.x, 0, m.y).normalized);
		currentDirection = new Vector3(currentDirection.x, 0, currentDirection.z).normalized * currentSpeedState;

		skidding = Vector3.Dot(currentDirection, smoothDirection) < -0.25f;
		float currentSmoothDamp = skidding ? 0.15f : 0.075f;
		smoothDirection = Vector3.SmoothDamp
				(smoothDirection, currentDirection, ref smoothDirectionVel, currentSmoothDamp);
	}
	public void SimpleMove(Vector2 m) {
		if (!rb) return;
		currentDirection = relativeForward.TransformDirection(new Vector3(m.x, 0, m.y).normalized);
		currentDirection = new Vector3(currentDirection.x, 0, currentDirection.z).normalized * currentSpeedState;
	}
	
	public void JumpOperations() {
		if (holdingJump) {
			holdingJumpTimer += Time.fixedDeltaTime;
		} else {
			holdingJumpTimer = 0;
		}

		if (holdingJumpTimer != 0 && holdingJumpTimer < 0.25f) {
			Jump();
		}
	}
	public void Jump() {
		if (!isGrounded) return;
		currentVerticalSpeed = jumpSpeed;
		holdingJumpTimer = 100;
		trueGrounded = false;
		isGrounded = false;
		events.onJump.Invoke();
	}

	internal Vector3 planeNormal;
	internal Vector3 planeNormalX;
	internal Vector3 planeNormalZ;
	internal void GroundDetection() {

		bool prevGrounded = isGrounded;
		float prevVertical = currentVerticalSpeed;

		if (Physics.SphereCast(rb.centerOfMass + transform.position, 0.325f, Vector3.down, out isGroundedHit)) {
			trueGrounded = isGroundedHit.distance < 0.6f;
			Debug.DrawLine(rb.centerOfMass + transform.position, 
				rb.centerOfMass + transform.position + Vector3.down*0.6f, Color.red);
			
		} else {
			trueGrounded = false;
		}

		//Thing

		if (trueGrounded) {

			currentVerticalSpeed = Physics.gravity.y/10f;
			isGroundedTimer = 0;

			planeNormal = isGroundedHit.normal;
			planeNormalX = new Vector3(planeNormal.y, -planeNormal.x, 0);
			planeNormalZ = new Vector3(0, -planeNormal.z, planeNormal.y);

		} else {

			currentVerticalSpeed += Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;
			isGroundedTimer += Time.fixedDeltaTime;

			planeNormal = Vector3.up;
			planeNormalX = new Vector3(planeNormal.y, -planeNormal.x, 0);
			planeNormalZ = new Vector3(0, -planeNormal.z, planeNormal.y);

		}

		isGrounded = isGroundedTimer < 0.1f;

		if (prevGrounded != isGrounded && isGrounded && prevVertical < 0) 
			events.onLand.Invoke(Mathf.Abs(prevVertical) / Mathf.Abs(Physics.gravity.y));

	}

}
