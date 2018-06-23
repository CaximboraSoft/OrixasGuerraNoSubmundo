using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbejideMagia : MonoBehaviour {

	private Rigidbody meuRigidbody;

	public float dano = 25;
	public float velocidade;

	// Use this for initialization
	void Start () {
		meuRigidbody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		meuRigidbody.AddForce (transform.forward * velocidade);
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "ArmaColisor") {
			return;
		}

		if (other.tag == "Inimigo") {
			other.GetComponent<DadosMovimentacao> ().PerderVida (dano, false);
			Destroy (gameObject);
		} if (other.tag == "MiniBoss") {
			other.GetComponent<DadosMovimentacao> ().PerderVida (dano / 2.5f, false);
			Destroy (gameObject);
		} else if (other.tag != "Abejide" && other.tag != "Player") {
			Destroy (gameObject);
		}
	}
}
