using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroirAposTempo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine ("Destroir");
	}

	IEnumerator Destroir () {
		yield return new WaitForSeconds (3);
		Destroy (gameObject);
	}
}
