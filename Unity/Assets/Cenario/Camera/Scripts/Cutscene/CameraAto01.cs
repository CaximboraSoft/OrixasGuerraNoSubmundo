using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAto01 : MonoBehaviour {

	public GameObject terreno;
	public Animator atorAnimator;
	public RuntimeAnimatorController controllerInicial;
	public RuntimeAnimatorController controllerCutscene;

	private float tempoEspera;
	private float maxTempoEspera;

	private int ato;

	// Use this for initialization
	void Start () {
		atorAnimator.runtimeAnimatorController = controllerCutscene as RuntimeAnimatorController;

		ato = 0;
		tempoEspera = 10;
		maxTempoEspera = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (tempoEspera > maxTempoEspera) {
			
			switch (ato) {
			case 0:
				if (terreno.GetComponent<DadosDaFase>().atores[0].GetComponent<MetodosDaCutscene>().PegarAto () > 2) {
					ato++;
					atorAnimator.SetInteger ("Ato", ato);
				}
				break;
			case 1:
				if (terreno.GetComponent<DadosDaFase>().atores[0].GetComponent<MetodosDaCutscene>().PegarAto () > 5) {
					ato++;
					atorAnimator.SetInteger ("Ato", ato);
				}
				break;
			case 2:
				if (terreno.GetComponent<DadosDaFase>().atores[2].GetComponent<MetodosDaCutscene>().PegarAto () > 2) {
					ato++;
					atorAnimator.SetInteger ("Ato", ato);
				}
				break;
			case 3:
				if (terreno.GetComponent<DadosDaFase>().atores[0].GetComponent<Abejide>().enabled == true) {
					atorAnimator.enabled = false;

					GetComponent<CameraAto01> ().enabled = false;
				}
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
				}
			}

			tempoEspera += Time.deltaTime;
		}
	}
}
