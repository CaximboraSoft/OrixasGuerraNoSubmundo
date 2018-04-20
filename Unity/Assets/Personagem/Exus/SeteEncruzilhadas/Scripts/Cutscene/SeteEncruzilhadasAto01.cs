using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeteEncruzilhadasAto01 : MonoBehaviour {

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
		atorAnimator.SetFloat ("NewtonAndando", GetComponent<MetodosDaCutscene> ().PegarNewtonAndando ());

		if (!GetComponent<MetodosDaCutscene> ().PegarEstaAtuando () && tempoEspera > maxTempoEspera) {

			switch (GetComponent<MetodosDaCutscene> ().PegarAto ()) {
			case 0:
				if (terreno.GetComponent<DadosDaFase>().atores[0].GetComponent<MetodosDaCutscene>().PegarAto () > 7) {
					tempoEspera = 0;
					maxTempoEspera = 2;
					GetComponent<MetodosDaCutscene> ().IncrementarAto ();
				}
				break;
			case 1:
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoPosicao (false, false, true, false, 0);
				break;
			case 2:
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoPosicao (true, false, true, false, 0);
				break;
			case 3:
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoPosicao (false, false, false, false, 0);
				break;
			case 4:
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoRotacao (false, false, 3); //Indice 1
				break;
			case 5:
				atorAnimator.GetComponent<Rigidbody> ().useGravity = false;
				atorAnimator.GetComponent<Collider> ().enabled = false;
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoMoverNoEixoY (2.7f, false, true, false, 1, 0);
				break;
			case 6:
				GetComponent<MetodosDaCutscene> ().IncrementarAto ();
				break;
			default:
				break;
			}
		} else {
			tempoEspera += Time.deltaTime;
		}
	}
}
