using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolaDeFogo : MonoBehaviour {

	private Transform abejide;
	private Rigidbody meuRigidbody;

	public float velocidade = 35f;

	// Use this for initialization
	void Start () {
		abejide = GameObject.FindGameObjectWithTag ("Abejide").transform;

		meuRigidbody = GetComponent<Rigidbody> ();

		StartCoroutine ("TempoDestroir");

		velocidade += Random.Range (0f, 10f);
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (abejide.position);
		meuRigidbody.AddForce (transform.forward * velocidade);
	}

	IEnumerator TempoDestroir () {
		yield return new WaitForSeconds (5);
		Destroy (gameObject);
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag != "Inimigo") {
			if (other.tag == "Abejide") {
				other.GetComponent<Abejide> ().PerderVida ();
				Destroy (gameObject);
			} else if (other.tag != "BolaDeFogo" && other.tag != "ArmaColisor") {
				Destroy (gameObject);
			}
		}
	}
}
