using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CamRaycastSwitch : MonoBehaviour {

	[System.Serializable]
	public class Events {
		public QOL.BoolEvent onHoldSwitch = new QOL.BoolEvent();
		public QOL.StringEvent onConfirm = new QOL.StringEvent();
	}
	public Events events = new Events();

	internal CamRaycast camRaycast;
	internal CamInteraction camInteraction;
	public PlayerInput pi;

	public Transform pointL, pointC, pointR; //0, 1, 2

	void Start() {

		camRaycast = GetComponent<CamRaycast>();
		camInteraction = GetComponent<CamInteraction>();

		pi.actions["CamSwitch"].started += ctx => {
			HoldSwitch(true);
			if (camInteraction) camInteraction.enabled = false;
		};

		pi.actions["CamSwitch"].canceled += ctx => {
			HoldSwitch(false);
			if (camInteraction) camInteraction.enabled = true;
		};

	}
	bool heldSwitch;
	void HoldSwitch(bool s) {

		heldSwitch = s;
		events.onHoldSwitch.Invoke(s);

		if (heldSwitch) {
			pi.actions["SwitchL"].started += SetSwitchWithContext;
			pi.actions["SwitchC"].started += SetSwitchWithContext;
			pi.actions["SwitchR"].started += SetSwitchWithContext;
		} else {
			pi.actions["SwitchL"].started -= SetSwitchWithContext;
			pi.actions["SwitchC"].started -= SetSwitchWithContext;
			pi.actions["SwitchR"].started -= SetSwitchWithContext;
		}
	}

	void SetSwitchWithContext(InputAction.CallbackContext ctx) {

		Transform t = null;

		if (ctx.action.name == "SwitchL") t = pointL;
		if (ctx.action.name == "SwitchC") t = pointC;
		if (ctx.action.name == "SwitchR") t = pointR;
		SetSwitch(t);

		events.onConfirm.Invoke(ctx.action.name);
	}

	void SetSwitch(Transform t) {
		if (t == null) return;
		//camRaycast.SwitchTransform(t);
		camRaycast.SmoothTransform(t, 0.3f, QOL.easeOutCurve);
	}

}
