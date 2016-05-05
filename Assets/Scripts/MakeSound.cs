using UnityEngine;
using System.Collections;

public class MakeSound : MonoBehaviour {

	AudioSource myAudio;
	public AudioClip sound;

	// Use this for initialization
	void Start () {
		//TODO: change sound to audio fitting to the bullet which hit
		myAudio = GetComponent<AudioSource> ();
		myAudio.PlayOneShot (sound);
		Destroy (gameObject, sound.length);
	}
}
