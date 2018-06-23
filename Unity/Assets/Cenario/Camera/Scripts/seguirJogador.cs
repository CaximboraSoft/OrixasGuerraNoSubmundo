using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seguirJogador : MonoBehaviour {

	private Transform abejide;
	public Transform seta;
	public Vector3 rotacao;
	private Vector3 novaPosicao = Vector3.zero;
	private Transform meuUltimoAlvo;

	public float distanciaY = 10f;
	public float distanciaZ = 7f;
	public float tempoDeResposta = 2f;
	public float tempoOlhar = 2f;
	public float temporizador = 0f;

	// Use this for initialization
	void Awake () {
		abejide = GameObject.Find ("Abejide").GetComponent<Transform> ();
	}

	public void AtivarCodigo() {
		GetComponent<Camera> ().fieldOfView = 60;

		novaPosicao.Set (abejide.position.x, abejide.position.y + distanciaY, abejide.position.z - distanciaZ);
		transform.position = novaPosicao;

		Transform meuAlvo = abejide.GetComponent <Abejide> ().LugarParaCameraOlhar ();
		transform.LookAt (meuAlvo.position + rotacao);

		GetComponent<seguirJogador> ().enabled = true;
	}

	// Update is called once per frame
	void Update () {
		novaPosicao.Set (abejide.position.x, abejide.position.y + distanciaY, abejide.position.z - distanciaZ);

		Transform meuAlvo = abejide.GetComponent <Abejide> ().LugarParaCameraOlhar ();

		if (meuAlvo != meuUltimoAlvo) {
			temporizador = tempoOlhar;
		}

		meuUltimoAlvo = meuAlvo;

		//O tempo de toração vai decaindo até chegar a zero
		if (temporizador > 0.1f) {
			temporizador -= Time.deltaTime;

			if (temporizador < 0.1f) {
				temporizador = 0.1f;
			}
		}
		seta.LookAt (meuAlvo.position + rotacao);
		transform.rotation = Quaternion.Lerp (transform.rotation, seta.rotation, temporizador * Time.deltaTime);
		transform.position = Vector3.Lerp (transform.position, novaPosicao, tempoDeResposta * Time.deltaTime);
	}

	public void AtivarComLula () {
		temporizador = tempoOlhar;
		GetComponent<seguirJogador> ().enabled = true;
	}
}
