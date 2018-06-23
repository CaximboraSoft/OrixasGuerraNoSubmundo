using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balas : MonoBehaviour {

	public GameObject somPrefab;
	public AudioClip som;
	private Rigidbody meuRigidbody;

	public float dano = 25f;
	public float velocidade = 50f;
	public float volume = 1f;

	private float temporizador = 0f;

	// Use this for initialization
	void Start () {
		meuRigidbody = GetComponent<Rigidbody> ();

		GameObject somTemp = Instantiate (somPrefab, transform.position, transform.rotation);
		somTemp.GetComponent<AudioSource> ().clip = som;
		somTemp.GetComponent<AudioSource> ().volume = volume;
		somTemp.GetComponent<AudioSource> ().Play ();
	}

	// Update is called once per frame
	void Update () {
		temporizador += Time.deltaTime;

		if (temporizador > 10f) {
			Destroy (gameObject);
		}

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
