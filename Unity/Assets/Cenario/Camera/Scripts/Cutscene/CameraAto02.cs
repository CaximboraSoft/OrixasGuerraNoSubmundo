using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAto02 : MonoBehaviour {

	private Transform abejide;
	private DadosDaFase dadosDaFase;
	private Animator atorAnimator;
	public RuntimeAnimatorController controllerCutscene;
	private seguirJogador meuSeguirJogador;

	private float tempoEspera = 0f;
	private float maxTempoEspera = 0f;

	private int ato;

	void Awake () {
		abejide = GameObject.Find ("Abejide").GetComponent<Transform> ();
		dadosDaFase = FindObjectOfType<DadosDaFase> ();
		atorAnimator = GetComponent<Animator> ();
		meuSeguirJogador = GetComponent<seguirJogador> ();
	}

	// Use this for initialization
	public void AtivarCodigo () {
		atorAnimator.runtimeAnimatorController = controllerCutscene as RuntimeAnimatorController;

		GetComponent<CameraAto02> ().enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		switch (ato) {
		case 0:
			//Abejide
			if (dadosDaFase.atores [0].GetComponent<MetodosDaCutscene> ().PegarAto () > 7) {
				ato++;
				atorAnimator.SetInteger ("Ato", ato);
				maxTempoEspera = 2f;
			}
			break;
		case 1:
			tempoEspera += Time.deltaTime;
			if (tempoEspera > maxTempoEspera) {
				tempoEspera = 0f;
				atorAnimator.enabled = false;
				ato++;
			}
			break;
		case 2:
			float tempoTrancicao = 0.2f;

			Vector3 novaPosicao = new Vector3 (abejide.position.x, abejide.position.y + meuSeguirJogador.distanciaY, abejide.position.z - meuSeguirJogador.distanciaZ);
			transform.position = Vector3.Lerp (transform.position, novaPosicao, tempoTrancicao * Time.deltaTime);
			meuSeguirJogador.seta.LookAt (abejide.position);
			transform.rotation = Quaternion.Lerp (transform.rotation, meuSeguirJogador.seta.rotation, tempoTrancicao * Time.deltaTime);
			break;
		}
	}
}
