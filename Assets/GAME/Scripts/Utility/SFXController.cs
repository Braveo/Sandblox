using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXController : MonoBehaviour {

	internal AudioSource source;

	public bool playOnAwake = false;
	[Range(0,1)]
	public float volumeBase = 1;
	public Vector2 volumeOffset = Vector2.zero;
	[Range(-3,3)]
	public float pitchBase = 1;
	public Vector2 pitchOffset = Vector2.zero;

	[System.Serializable]
	public struct Sounds {
		public AudioClip[] clips;
	}
	public Optional<Sounds> sounds = new Optional<Sounds>(new Sounds(), false);

	void Awake() {
		source = GetComponent<AudioSource>();
	}
	void Start() {
		if (playOnAwake) {

			if (sounds.Enabled) {

				PlayRandom();

			} else {

				Play();

			}

		}
	}
	void OnValidate() {
		if (TryGetComponent<AudioSource>(out source)) {

			source.playOnAwake = playOnAwake;
			source.volume = volumeBase;
			source.pitch = pitchBase;

		}
	}

	public void Play() {

		Play(source.clip);

	}

	public void PlayWithVolume(float dynamic) => PlayRaw(source.clip, volBase: dynamic * volumeBase);
	public void PlayWithPitch(float pitch) => PlayRaw(source.clip, pitBase: pitch * pitchBase);

	public void Play(AudioClip clip) => PlayRaw(clip, volumeBase, pitchBase);
	
	public void PlayRaw(AudioClip clip, float volBase = 1, float pitBase = 1) {

		if (clip) source.clip = clip;

		source.volume = volBase + Random.Range(volumeOffset.x, volumeOffset.y);
		source.pitch = pitBase + Random.Range(pitchOffset.x, pitchOffset.y);

		source.Play();

	}



	int prevIndex = -1;

	public void PlayRandomAnimEvent(AnimationEvent @event) {

		PlayRandomRaw(@event.animatorClipInfo.weight * @event.floatParameter);

	}

	public void PlayRandom() {

		PlayRandomRaw(volumeBase);

	}

	public void PlayRandomRaw(float volBase = 1) {

		if (sounds && sounds.val.clips.Length > 1) {

			int len = sounds.val.clips.Length;
			int random = Random.Range(0,len);

			if (random == prevIndex) {

				if (random == len-1) random -= 1;
				if (random == 0) random += 1;

			}

			prevIndex = random;

			PlayRaw(sounds.val.clips[random], volBase, pitchBase);

		}

	}

}