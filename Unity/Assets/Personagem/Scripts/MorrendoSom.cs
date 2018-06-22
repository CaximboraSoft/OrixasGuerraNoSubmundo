using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorrendoSom : MonoBehaviour {

	private AudioSource meuAudioSource;
	public AudioClip[] sons;

	// Use this for initialization
	void Start () {
		meuAudioSource = GetComponent<AudioSource> ();
		meuAudioSource.clip = sons[Random.Range (0, sons.Length)];
		meuAudioSource.Play ();

		StartCoroutine ("Destruir");
	}

	IEnumerator Destruir () {
		yield return new WaitForSeconds (2);
		Destroy (gameObject);
	}
}
