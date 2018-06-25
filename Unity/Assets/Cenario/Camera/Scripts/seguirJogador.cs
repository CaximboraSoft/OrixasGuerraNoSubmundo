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
	private float temporizadorOlhar = 0f;

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
		Transform meuAlvo = abejide.GetComponent <Abejide> ().LugarParaCameraOlhar ();

		if (meuAlvo != meuUltimoAlvo) {
			temporizadorOlhar = tempoOlhar;
		}

		meuUltimoAlvo = meuAlvo;

		//O tempo de toração vai decaindo até chegar a zero
		if (temporizadorOlhar > 0f) {
			temporizadorOlhar -= Time.deltaTime;

			if (temporizadorOlhar < 0f) {
				temporizadorOlhar = 0f;
			}
		}

		if (temporizadorOlhar == 0f) {
			transform.LookAt (meuAlvo.position + rotacao);
		} else {
			seta.LookAt (meuAlvo.position + rotacao);
			transform.rotation = Quaternion.Lerp (transform.rotation, seta.rotation, temporizadorOlhar * Time.deltaTime);
		}

		novaPosicao.Set (abejide.position.x, ((meuAlvo.position.y + abejide.position.y) / 2) + distanciaY, ((meuAlvo.position.z + abejide.position.z) / 2) - distanciaZ);

		transform.position = Vector3.Lerp (transform.position, novaPosicao, tempoDeResposta * Time.deltaTime);
	}

	public void AtivarComLula () {
		temporizadorOlhar = tempoOlhar;
		GetComponent<seguirJogador> ().enabled = true;
	}
}
