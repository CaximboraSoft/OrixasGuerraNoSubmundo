using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FazerPedrasVoar : MonoBehaviour {

	private Rigidbody meuRigidbody;
	public Vector3 vetorDaForca;

	public float forca;

	// Use this for initialization
	void Start () {
		meuRigidbody = GetComponent<Rigidbody> ();
		meuRigidbody.useGravity = false;
		GetComponent<Collider> ().enabled = false;
	}

	public void AplicarForcaNasPedrsa () {
		meuRigidbody.useGravity = true;
		meuRigidbody.constraints = RigidbodyConstraints.None;
		meuRigidbody.AddForce (vetorDaForca * forca);
		GetComponent<Collider> ().enabled = true;
	}
}
