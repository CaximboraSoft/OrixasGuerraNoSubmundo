using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbejideAto01 : MonoBehaviour {

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
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoMoverNoEixoY (1.85f, true, false, false, 1, 2);
				break;
			case 1:
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoOlharParaObjeto (terreno.GetComponent<DadosDaFase> ().atores [1], true, false);
				break;
			case 2:
				//Comeca a tentar fugir.
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoPosicao (true, true, false, false, 0);
				break;
			case 3:
				if (terreno.GetComponent<DadosDaFase> ().atores [1].GetComponent<MetodosDaCutscene> ().PegarAto () > 8) {
					atorAnimator.SetTrigger ("Paralizar");
					tempoEspera = 0;
					maxTempoEspera = 1;
					GetComponent<MetodosDaCutscene> ().IncrementarAto ();
				}
				break;
			case 4:
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoMoverNoEixoY (0.7f, true, false, false, 1, 0);
				break;
			case 5:
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoRotacao (true, true, 0);
				break;
			case 6:
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoPosicao (true, true, false, false, 0);
				break;
			case 7:
				//Nesse ponto é que o velho vai se apriximar do Abejide, incrementa para fazer o velho começar sua rotina.
				GetComponent<MetodosDaCutscene> ().IncrementarAto ();
				break;
			case 8:
				if (terreno.GetComponent<DadosDaFase> ().atores [2].GetComponent<MetodosDaCutscene> ().PegarAto () > 5) {
					GetComponent<MetodosDaCutscene> ().IncrementarAto ();
				}
				break;
			case 9:
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoSeguirOutroAtor (terreno.GetComponent<DadosDaFase> ().atores [2], true, true, true);
				break;
			case 10:
				atorAnimator.runtimeAnimatorController = controllerInicial as RuntimeAnimatorController;
				transform.position = new Vector3 (287.86f, 1.47f, 266.84f);
				transform.eulerAngles = new Vector3 (0, 0, 0);
				GetComponent<AbejideAtaque> ().AtivarCodigo ();
				GetComponent<AbejideAtaque> ().enabled = true;
				GetComponent<AbejideAndando> ().enabled = true;

				GetComponent<AbejideAto01> ().enabled = false;
				GetComponent<MetodosDaCutscene> ().enabled = false;
				break;
			default:
				
				break;
			}
		} else {
			if (GetComponent<MetodosDaCutscene> ().PegarEstaAtuando ()) {
				switch (GetComponent<MetodosDaCutscene> ().PegarAto ()) {
				case 2: //Faz o abejide parar de olhar para o exu tranca tudo.
					if (terreno.GetComponent<DadosDaFase> ().atores [1].GetComponent<MetodosDaCutscene> ().PegarAto () > 6) {
						GetComponent<MetodosDaCutscene> ().MudarEstaAtuando (false);

					}
					break;
				case 10: //Faz o abejide parar de olhar para o exu tranca tudo.
					if (terreno.GetComponent<DadosDaFase> ().atores [2].GetComponent<MetodosDaCutscene> ().PegarAto () > 6) {
						GetComponent<MetodosDaCutscene> ().MudarEstaAtuando (false);

					}
					break;
				}
			}

			tempoEspera += Time.deltaTime;
		}
	}
}
