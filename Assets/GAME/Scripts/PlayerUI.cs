using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {
	
	public CamInteraction ci;
	public PlayerInteraction pi;
	public AudioSource sfx;

	public GameObject crosshair;
	public GameObject crosshairInteraction;

	public ItemData emptyItem;

	[Header("HOTBAR")]
	public GameObject[] hotbarSlots;
	internal RawImage[] imageSlots;

	public Transform hotbarSelect;
	public ItemData[] hotbarItems;
	public int currentSlot = 0;

	void Awake() {
		if (ci) {
			ci.crosshairEntering.AddListener(SetCrosshairInteracting);
			ci.pi.actions["MoveSlot"].performed += ctx => SelectHotbarDir(ctx.ReadValue<float>());
			ci.pi.actions["NumKeys"].performed += ctx => SelectHotbarSlot(Mathf.RoundToInt(ctx.ReadValue<float>())-1);
		}

		imageSlots = new RawImage[hotbarSlots.Length];
		for (int i = 0; i < hotbarSlots.Length; i++) {

			RawImage[] total = hotbarSlots[i].transform.GetComponentsInChildren<RawImage>();
			foreach (RawImage e in total) {

				if (e.gameObject != hotbarSlots[i]) imageSlots[i] = e;

			}

		}

		StartCoroutine(EnStartSlot());
	}

	public IEnumerator EnStartSlot() {

		yield return new WaitForEndOfFrame();
		UpdateItemSlot();

	}

	public void SetCrosshairInteracting(IInteractEnterExit iee, RaycastHit hit) {

		crosshair.SetActive(iee == null);
		crosshairInteraction.SetActive(iee != null);

	}

	public void SelectHotbarDir(float dir) {

		if (dir > 0) SelectHotbarSlot(currentSlot - 1);
		if (dir < 0) SelectHotbarSlot(currentSlot + 1);

	}

	public void SelectHotbarSlot(int slot) {

		bool playSFX = slot != currentSlot;

		SelectHotbarSlotRAW(slot);

		if (sfx && playSFX) {

			sfx.pitch = 1f + ((1-currentSlot/(float)hotbarSlots.Length)*0.5f);
			sfx.Play();

		}

	}
	public void SelectHotbarSlotRAW(int slot) {

		if (slot >= hotbarSlots.Length) {

			currentSlot = 0;

		} else if (slot < 0) {

			currentSlot = hotbarSlots.Length-1;

		} else {

			currentSlot = slot;

		}

		UpdateItemSlot();

	}

	//When you switch items
	internal void UpdateItemSlot() {

		hotbarSelect.position = hotbarSlots[currentSlot].transform.position;

		if (!pi) return;

		pi.currentItem = hotbarItems[currentSlot] ? hotbarItems[currentSlot] : emptyItem;

		for (int i = 0; i < imageSlots.Length; i++) {

			if (hotbarItems[i]) {

				imageSlots[i].material = hotbarItems[i].display.itemMaterial;
				imageSlots[i].color = hotbarItems[i].display.color;
				imageSlots[i].texture = hotbarItems[i].display.itemThumbnail;

			} else {

				imageSlots[i].material = null;
				imageSlots[i].color = Color.clear;
				imageSlots[i].texture = null;

			}

		}

	}

	//Preparing holding and item thumbnails
	internal void DisplayItems() {

		for (int i = 0; i < imageSlots.Length; i++) {

			if (hotbarItems[i] != null) {

				imageSlots[i].texture = hotbarItems[i].display.itemThumbnail;
				imageSlots[i].color = hotbarItems[i].display.color;
				imageSlots[i].material = hotbarItems[i].display.itemMaterial;

			} else {

				imageSlots[i].texture = null;
				imageSlots[i].color = Color.clear;
				imageSlots[i].material = null;

			}

		}

	}
}
