using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seguirJogador : MonoBehaviour {

	private Transform abejide;
	public Vector3 rotacao;
	private Vector3 novaPosicao = Vector3.zero;

	public float distanciaY;
	public float distanciaZ;

	// Use this for initialization
	void Awake () {
		abejide = GameObject.Find ("Abejide").GetComponent<Transform> ();

		AtivarCodigo ();
	}

	public void AtivarCodigo() {
		GetComponent<Camera> ().fieldOfView = 60;
		transform.eulerAngles = Vector3.zero;
		transform.rotation = Quaternion.Euler (rotacao);

		GetComponent<seguirJogador> ().enabled = true;
	}

	// Update is called once per frame
	void Update () {
		novaPosicao.Set (abejide.position.x, abejide.position.y + distanciaY, abejide.position.z - distanciaZ);

		transform.position = novaPosicao;
	}
}
