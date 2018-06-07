using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EsqueletoAto01 : MonoBehaviour {

	public GameObject terreno;
	public Animator atorAnimator;
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
		//atorAnimator.SetFloat ("NewtonAndando", GetComponent<DadosForcaResultante> ().PegarNewtonAndando ());

		if (terreno.GetComponent<DadosDaFase> ().atores [0].GetComponent<AbejideAto01> ().enabled == false) {
			Destroy (gameObject);
		}

		if (!GetComponent<MetodosDaCutscene> ().PegarEstaAtuando () && tempoEspera > maxTempoEspera) {
			
			switch (GetComponent<MetodosDaCutscene> ().PegarAto ()) {
			case 0:
				if (terreno.GetComponent<DadosDaFase> ().atores [0].GetComponent<MetodosDaCutscene> ().PegarAto () > 3) {
					tempoEspera = 0;
					maxTempoEspera = 0.3f;
					GetComponent<MetodosDaCutscene> ().IncrementarAto ();
				}
				break;
			case 1:
				atorAnimator.SetBool ("Andando", true);

				if (direita) {
					GetComponent<MetodosDaCutscene> ().ComecarAtuacaoPosicaoDeLado (1, 0);
				} else {
					GetComponent<MetodosDaCutscene> ().ComecarAtuacaoPosicaoDeLado (-1, 0);
				}
				break;
			case 2:
				atorAnimator.SetBool ("Andando", false);
				atorAnimator.SetTrigger ("Impedir");
				GetComponent<MetodosDaCutscene> ().IncrementarAto ();
				break;
			}
		} else {
			tempoEspera += Time.deltaTime;
		}
	}
}
