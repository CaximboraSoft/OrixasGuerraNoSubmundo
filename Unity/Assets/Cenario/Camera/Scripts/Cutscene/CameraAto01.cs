using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAto01 : MonoBehaviour {

	private DadosDaFase dadosDaFase;
	private Animator atorAnimator;
	public RuntimeAnimatorController controllerCutscene;

	private float tempoEspera;
	private float maxTempoEspera;

	private int ato;

	// Use this for initialization
	void Start () {
		dadosDaFase = FindObjectOfType<DadosDaFase> ();

		atorAnimator = GetComponent<Animator> ();
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
				//Abejide
				if (dadosDaFase.atores[0].GetComponent<MetodosDaCutscene>().PegarAto () > 3) {
					ato++;
					atorAnimator.SetInteger ("Ato", ato);
				}
				break;
			case 1:
				if (dadosDaFase.atores[0].GetComponent<MetodosDaCutscene>().PegarAto () > 5) {
					ato++;
					atorAnimator.SetInteger ("Ato", ato);
				}
				break;
			case 2:
				//Exu Velho
				if (dadosDaFase.atores[2].GetComponent<MetodosDaCutscene>().PegarAto () > 7) {
					ato++;
					atorAnimator.SetInteger ("Ato", ato);
				}
				break;
			case 3:
				if (dadosDaFase.atores[0].GetComponent<Abejide>().enabled == true) {
					atorAnimator.enabled = false;

					GetComponent<CameraAto01> ().enabled = false;
				}
				break;
			default:
				break;
			}
		} else {
			tempoEspera += Time.deltaTime;
		}
	}
}
