using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CamLook : MonoBehaviour {

	public Transform affectTransform;

	public float sensController = 150f;
	public float sensMouse = 0.0125f;
	internal float sensCurrent;

	public float maxAngle = 90;

	float yaw, pitch;
	Vector3 initAffectTransform;
	PlayerInput pi;

	void Awake() {
		pi = FindObjectOfType<PlayerInput>();
		ChangeControlScheme(pi);

		pi.onControlsChanged += ctx => ChangeControlScheme(ctx);
		initAffectTransform = affectTransform.localEulerAngles;
	}

	void Update() {
		Look(pi.actions.FindAction("Look").ReadValue<Vector2>());
	}

	void Look(Vector2 val) {

		yaw += val.x * sensCurrent * (usingController ? Time.deltaTime : 1);
		pitch += val.y * sensCurrent * (usingController ? Time.deltaTime : 1);
		pitch = Mathf.Clamp(pitch, -maxAngle, maxAngle);

		affectTransform.localEulerAngles = initAffectTransform + new Vector3(-pitch, yaw, 0);
	}

	bool usingController;
	void ChangeControlScheme(PlayerInput p) {
		usingController = p.currentControlScheme.Equals("Controller");
		sensCurrent = usingController ? sensController : sensMouse;
		Cursor.lockState = usingController ? CursorLockMode.None : CursorLockMode.Locked;
		Cursor.visible = false;
	}

}
