using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawBolasDeFogo : MonoBehaviour {

	private Transform abejide;
	public GameObject bolaDeFogo;

	public float distanciaY = 5f;
	public float precisaoX = 3f;
	public float precisaoY = 3f;
	public float maxTempoEntreSpaws = 2f;
	private float temporizador = 0f;

	public int maxBoloDeFogo = 2;
	private int contador = 0;
	public int tipoDeRotinaBolaDeFogo = 0;

	// Use this for initialization
	void Start () {
		abejide = GameObject.FindGameObjectWithTag ("Abejide").transform;
	}
	
	// Update is called once per frame
	void Update () {
		temporizador += Time.deltaTime;

		//Cria a bola de fogo quando o tempo de spaw za tiver passado
		if (temporizador > maxTempoEntreSpaws) {
			contador++;

			transform.position = new Vector3 (abejide.position.x, abejide.position.y + distanciaY, abejide.position.z);
			transform.LookAt (abejide.position);

			float randomX = Random.Range (-precisaoX, precisaoX);
			float randomY = Random.Range (-precisaoY, precisaoY);

			transform.eulerAngles = new Vector3 (transform.eulerAngles.x + randomX, transform.eulerAngles.y + randomY, transform.eulerAngles.z);
			Instantiate (bolaDeFogo, transform.position, transform.rotation);

			if (contador > maxBoloDeFogo) {
				Destroy (gameObject);
			}

			temporizador = 0f;
		}
	}
}
