using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName="Data",menuName="ScriptableObjs/ItemData-Block",order=1)] 
public class ItemDataTypeBlock : ItemData {

	public GameObject blockPrefab;

	public override void SecondaryInteract(MonoBehaviour m, InputAction.CallbackContext ctx, RaycastHit hit) {

		Vector3 rawPos = hit.point + hit.normal * 0.5f;

		Vector3 roundedPosition = RoundVector3(rawPos + Vector3.one * 0.5f) - Vector3.one * 0.5f;

		if (hit.rigidbody)
			//roundedPosition = rawPos;
			roundedPosition = 
				RoundVector3(rawPos - (hit.rigidbody.position)) + 
				(hit.rigidbody.position);

			GameObject block = Instantiate(blockPrefab,
				roundedPosition, 
				Quaternion.identity, null);


		Rigidbodyable bodyable = block.GetComponent<Rigidbodyable>();
		/*if (bodyable) {
			bodyable.TurnIntoRigidbody();
			if (!hit.rigidbody) bodyable.Kinematic();
		}*/
		if (hit.rigidbody) {

			Vector3 locScale = new Vector3(1 / hit.transform.localScale.x, 1 / hit.transform.localScale.y, 1 / hit.transform.localScale.z);
			Vector3 rawPosLocal = Vector3.Scale(hit.transform.InverseTransformPoint(rawPos), hit.transform.localScale);
			Vector3 newRoundedPosition = Vector3.Scale(RoundVector3(rawPosLocal), locScale);

			Transform initialParent = block.transform.parent;
			block.transform.parent = hit.transform;
			block.transform.localPosition = newRoundedPosition;
			block.transform.localEulerAngles = Vector3.zero;
			block.transform.parent = initialParent;

			//Rigidbodyable bodyable = block.GetComponent<Rigidbodyable>();
			//if (bodyable) {
			//	bodyable.TurnIntoRigidbody();
				
			//}

			FixedJoint joint = block.AddComponent<FixedJoint>();
			joint.connectedBody = hit.rigidbody;
			//joint.breakForce = 25000;
			//joint.massScale = 15;
			block.GetComponent<Rigidbody>().interpolation = hit.rigidbody.interpolation;
			block.GetComponent<Rigidbody>().mass = 15;
			//block.GetComponent<Rigidbody>().freezeRotation = true;


		}

	}

	public static Vector3 RoundVector3(Vector3 val) {

		return new Vector3(Mathf.Round(val.x), Mathf.Round(val.y), Mathf.Round(val.z));

	}

}