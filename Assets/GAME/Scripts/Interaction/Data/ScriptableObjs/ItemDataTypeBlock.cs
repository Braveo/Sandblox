using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName="Data",menuName="ScriptableObjs/ItemData-Block",order=1)] 
public class ItemDataTypeBlock : ItemData {

	public GameObject blockPrefab;

	public override void SecondaryInteract(MonoBehaviour m, InputAction.CallbackContext ctx, RaycastHit hit) {

		Vector3 rawPos = hit.point + hit.normal * 0.5f;

			GameObject block = Instantiate(blockPrefab, 
				RoundVector3(rawPos + Vector3.one * 0.5f) - Vector3.one * 0.5f, 
				Quaternion.identity, null);

	}

	public static Vector3 RoundVector3(Vector3 val) {

		return new Vector3(Mathf.Round(val.x), Mathf.Round(val.y), Mathf.Round(val.z));

	}

}