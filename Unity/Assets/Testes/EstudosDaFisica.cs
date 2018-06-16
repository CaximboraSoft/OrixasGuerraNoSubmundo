using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class EstudosDaFisica : MonoBehaviour {

	public Rigidbody meuRigidbody;
	public Text rigidbotyText;

	public Text estudoText;
	public float massa;
	public float atrito;
	public float graviade;
	public float velocidade;

	private float forcaAplicada = 100f;
	public bool souEstudo = false;

	// Update is called once per frame
	void Update () {
		if (Time.time > 1f) {
			if (souEstudo) {
				EstudoForca ();
			} else {
				RigidbodyForca ();
			}
		}
	}

	private void RigidbodyForca () {
		meuRigidbody.AddForce (transform.forward * forcaAplicada, ForceMode.Force);

		rigidbotyText.text = "Massa: " + meuRigidbody.mass + "\n" +
			"Atrito: " + meuRigidbody.drag + "\n" +
			"Forca aplicada: " + forcaAplicada + "\n" +
			"Velocidade: " + meuRigidbody.velocity.magnitude;
	}

	private void EstudoForca () {
		float forcaTemp;
		forcaTemp = forcaAplicada / massa;
		//forcaTemp -= graviade * massa;

		velocidade = Mathf.Lerp (velocidade, forcaTemp, 1 * Time.deltaTime);

		transform.Translate (transform.forward * velocidade * Time.deltaTime);

		estudoText.text = "Massa: " + massa + "\n" +
			"Atrito: " + atrito + "\n" +
			"Forca aplicada: " + forcaAplicada + "\n" +
			"Velocidade: " + velocidade;
	}
}
