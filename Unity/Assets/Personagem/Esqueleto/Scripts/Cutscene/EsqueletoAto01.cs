using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EsqueletoAto01 : MonoBehaviour {

	public GameObject terreno;
	public Animator atorAnimator;
	public RuntimeAnimatorController controllerInicial;
	public RuntimeAnimatorController controllerCutscene;

	public bool direita;

	private float tempoEspera;
	private float maxTempoEspera;

	// Use this for initialization
	void Start () {
		atorAnimator.runtimeAnimatorController = controllerCutscene as RuntimeAnimatorController;
		atorAnimator.SetBool ("Direita", direita);

		tempoEspera = 10;
		maxTempoEspera = 0;
	}
	
	// Update is called once per frame
	void Update () {
		atorAnimator.SetFloat ("NewtonAndando", GetComponent<MetodosDaCutscene> ().PegarNewtonAndando ());

		if (!GetComponent<MetodosDaCutscene> ().PegarEstaAtuando () && tempoEspera > maxTempoEspera) {
			
			switch (GetComponent<MetodosDaCutscene> ().PegarAto ()) {
			case 0:
				if (terreno.GetComponent<DadosDaFase> ().atores[0].GetComponent<MetodosDaCutscene> ().PegarAto () > 2) {
					tempoEspera = 0;
					maxTempoEspera = 1.3f;
					GetComponent<MetodosDaCutscene> ().IncrementarAto ();
				}
				break;
			case 1:
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoPosicaoDeLado (false, false, false, 0);
				break;
			case 2:
				atorAnimator.SetTrigger ("Impedir");
				GetComponent<MetodosDaCutscene> ().IncrementarAto ();
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
