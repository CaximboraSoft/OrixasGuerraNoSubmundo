using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolaDeFogo : MonoBehaviour {

	private Rigidbody meuRigidbody;

	public float velocidade = 35f;


	// Use this for initialization
	void Start () {
		meuRigidbody = GetComponent<Rigidbody> ();

		StartCoroutine ("TempoDestroir");
	}
	
	// Update is called once per frame
	void Update () {
		meuRigidbody.AddForce (transform.forward * velocidade);
	}

	IEnumerator TempoDestroir () {
		yield return new WaitForSeconds (5);
		Destroy (gameObject);
	}
}
