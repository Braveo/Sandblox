using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour {

	[System.Serializable]
	public class Events {
		public UnityEvent onJump = new UnityEvent();
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

		
		
		SimpleMove(currentXYInput * currentSpeedState);
	}


	public void SimpleMove(Vector2 m) {
		if (!rb) return;
		currentDirection = relativeForward.TransformDirection(new Vector3(m.x, 0, m.y).normalized);
		currentDirection = new Vector3(currentDirection.x, 0, currentDirection.z).normalized * currentSpeedState;
		rb.velocity = new Vector3(currentDirection.x, currentVerticalSpeed, currentDirection.z);
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

	internal void GroundDetection() {

		if (Physics.SphereCast(rb.centerOfMass + transform.position, 0.325f, Vector3.down, out isGroundedHit)) {
			trueGrounded = isGroundedHit.distance < 0.6f;
			Debug.DrawLine(rb.centerOfMass + transform.position, 
				rb.centerOfMass + transform.position + Vector3.down*0.6f, Color.red);
		} else {
			trueGrounded = false;
		}

		if (trueGrounded) {
			currentVerticalSpeed = Physics.gravity.y/10f;
			isGroundedTimer = 0;
		} else {
			currentVerticalSpeed += Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;
			isGroundedTimer += Time.fixedDeltaTime;
		}

		isGrounded = isGroundedTimer < 0.1f;

	}

}
