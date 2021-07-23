using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName="Data",menuName="ScriptableObjs/ItemData",order=1)] 
public class ItemData : ScriptableObject {

	[System.Serializable]
	public class Display {

		public Texture2D itemThumbnail;
		public Color color = Color.white;
		public Material itemMaterial;
		public GameObject itemHeldPrefab;

	}
	public Display display = new Display();

	[System.Serializable]
	public class Animation {

		public string animPrimary = "Place";
		public string animSecondary = "Place";

	}
	public Animation animation = new Animation();

	

	public virtual void PrimaryInteract(MonoBehaviour m, InputAction.CallbackContext ctx, RaycastHit hit) {

		if (hit.collider.TryGetComponent(out IBreakable b)) {

			b.Break(hit);

		}

	}

	public virtual void SecondaryInteract(MonoBehaviour m, InputAction.CallbackContext ctx, RaycastHit hit) {

	}

}