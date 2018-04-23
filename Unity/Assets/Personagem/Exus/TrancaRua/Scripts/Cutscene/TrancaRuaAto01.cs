using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrancaRuaAto01 : MonoBehaviour {

	public GameObject terreno;
	public Animator atorAnimator;
	public RuntimeAnimatorController controllerInicial;
	public RuntimeAnimatorController controllerCutscene;

	private float tempoEspera;
	private float maxTempoEspera;

	// Use this for initialization
	void Start () {
		atorAnimator.runtimeAnimatorController = controllerCutscene as RuntimeAnimatorController;

		tempoEspera = 10;
		maxTempoEspera = 0;
	}
	
	// Update is called once per frame
	void Update () {
		atorAnimator.SetFloat ("NewtonAndando", GetComponent<DadosForcaResultante> ().PegarNewtonAndando ());

		if (!GetComponent<MetodosDaCutscene> ().PegarEstaAtuando () && tempoEspera > maxTempoEspera) {

			switch (GetComponent<MetodosDaCutscene> ().PegarAto ()) {
			case 0:
				if (terreno.GetComponent<DadosDaFase>().atores[0].GetComponent<MetodosDaCutscene>().PegarAto () > 1) {
					GetComponent<MetodosDaCutscene> ().IncrementarAto ();
					GetComponent<MetodosDaCutscene> ().Falar ("Oh fã do felixo feto, a maconha aumentou de preço, vai custar 500 conto.", 2, 8);
				}
				break;
			case 1:
				GetComponent<MetodosDaCutscene> ().IncrementarAto ();
				break;
			case 2:
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoPosicao (true, false, false, false, 0);
				break;
			case 3:
				tempoEspera = 0;
				maxTempoEspera = 2;
				GetComponent<MetodosDaCutscene> ().IncrementarAto ();
				GetComponent<MetodosDaCutscene> ().Falar ("...", 0, 2);
				break;
			case 4:
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoRotacao (false, true, 0);
				GetComponent<MetodosDaCutscene> ().Falar ("Pera ai, esqueci de pegar a maconha ali no paranaue.", 2, 8);
				break;
			case 5:
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoPosicao (false, false, true, false, 0);
				break;
			case 6:
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoPosicao (false, false, false, false, 0);
				break;
			case 7:
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoRotacao (false, false, 0);
				break;
			case 8:
				tempoEspera = 0;
				maxTempoEspera = 3;
				atorAnimator.SetInteger ("Ato", GetComponent<MetodosDaCutscene> ().PegarAto ());
				atorAnimator.SetTrigger ("MudarAnimacao");
				GetComponent<MetodosDaCutscene> ().IncrementarAto ();
				GetComponent<MetodosDaCutscene> ().Falar ("Aaaahhh, mais você não vai fugir não seu resto de aborto.", 0, 6);

				break;
			case 9:
				if (terreno.GetComponent<DadosDaFase>().atores[2].GetComponent<MetodosDaCutscene>().PegarAto () > 2) {
					GetComponent<MetodosDaCutscene> ().IncrementarAto ();
				}
				break;
			case 10:
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoRotacao (true, true, 0);
				break;
			case 11:
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoPosicao (true, false, false, false, 0);
				break;
			case 12:
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoRotacao (true, false, 0);
				break;
			default:
				//ato--;
				/*
				GetComponent<Ato01> ().enabled = false;
				GetComponent<MetodosDaCutscene> ().enabled = false;
				GetComponent<AbejideAtaque> ().enabled = true;
				GetComponent<AbejideAndando> ().enabled = true;*/
				break;
			}
		} else {
			tempoEspera += Time.deltaTime;
		}
	}
}
