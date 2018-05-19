using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbejideAto01 : MonoBehaviour {

	public Canvas hud;
	public GameObject terreno;
	public Animator corpoAnimator;
	public Animator peAnimator;
	public RuntimeAnimatorController corpoController;
	public RuntimeAnimatorController peController;

	private float tempoEspera;
	private float maxTempoEspera;

	private string fala;

	private int sat;

	// Use this for initialization
	void Start () {
		hud.enabled = false;
		corpoAnimator.runtimeAnimatorController = corpoController as RuntimeAnimatorController;
		peAnimator.runtimeAnimatorController = peController as RuntimeAnimatorController;
		GetComponent<Collider> ().enabled = false;
		GetComponent<Rigidbody> ().useGravity = false;

		tempoEspera = 10;
		maxTempoEspera = 0;

		GetComponent<MetodosDaCutscene> ().MudarNome("Abejide:");

		sat = terreno.GetComponent<DadosDaFase> ().sat;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Space)) {
			GetComponent<MetodosDaCutscene> ().MudarEstaAtuando (false);
			GetComponent<MetodosDaCutscene> ().MudarAtor (9);
		}

		corpoAnimator.SetFloat ("AceleracaoAndando", GetComponent<DadosForcaResultante> ().PegarAceleracaoAndando ());
		peAnimator.SetFloat ("AceleracaoAndando", GetComponent<DadosForcaResultante> ().PegarAceleracaoAndando ());

		if (!GetComponent<MetodosDaCutscene> ().PegarEstaAtuando () && tempoEspera > maxTempoEspera) {
			
			switch (GetComponent<MetodosDaCutscene> ().PegarAto ()) {
			case 0:
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoMoverNoEixoY (1.95f, true, true, false, 1, 0);
				break;
			case 1:
				GetComponent<Collider> ().enabled = true;
				GetComponent<Rigidbody> ().useGravity = true;
				GetComponent<MetodosDaCutscene> ().IncrementarAto ();
				break;
			case 2:
				if (terreno.GetComponent<DadosDaFase> ().atores[1].GetComponent<MetodosDaCutscene> ().PegarAto () > 5) {
					fala = "...";
					GetComponent<MetodosDaCutscene> ().Falar(fala, GetComponent<MetodosDaCutscene> ().PegarNome (), 0, 3 * sat, true);
				}
				break;
			case 3:
				if (terreno.GetComponent<DadosDaFase> ().atores [2].GetComponent<MetodosDaCutscene> ().PegarAto () > 3) {
					fala = "Seu velho tolo!!!";
					GetComponent<MetodosDaCutscene> ().Falar (fala, GetComponent<MetodosDaCutscene> ().PegarNome (), 0, 4 * sat, true);
					tempoEspera = 0;
					maxTempoEspera = 3.5f * sat;
				}
				break;
			case 4: //Abejide começa a tentar fugir.
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoPosicao (true, true, false, false, 0);
				break;
			case 5:
				if (terreno.GetComponent<DadosDaFase> ().atores [1].GetComponent<MetodosDaCutscene> ().PegarAto () > 10) {
					corpoAnimator.SetTrigger ("Paralizar");
					peAnimator.SetTrigger ("Paralizar");
					tempoEspera = 0;
					maxTempoEspera = 1f * sat;
					GetComponent<MetodosDaCutscene> ().IncrementarAto ();
				}
				break;
			case 6:
				GetComponent<Collider> ().enabled = false;
				GetComponent<Rigidbody> ().useGravity = false;
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoMoverNoEixoY (2f, true, true, false, 2, 0);
				break;
			case 7:
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoPosicao (true, true, false, false, 0);
				break;
			case 8:
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoSeguirOutroAtor (terreno.GetComponent<DadosDaFase> ().atores[2], false, true, false);
				break;
			case 9:
				Camera.main.GetComponent<CameraAto01> ().enabled = false;
				Camera.main.GetComponent<Animator> ().enabled = false;
				GetComponent<MetodosDaCutscene> ().boca.GetComponent<Conversas> ().conversas.enabled = false;
				GetComponent<MetodosDaCutscene> ().boca.GetComponent<Conversas> ().enabled = false;
				GetComponent<MetodosDaCutscene> ().MudarIndicePosicao (2);
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoTeleporte ();
				GetComponent<MetodosDaCutscene> ().enabled = false;
				GetComponent<Collider> ().enabled = true;
				GetComponent<Rigidbody> ().useGravity = true;

				hud.enabled = true;
				GetComponent<AbejideAtaque> ().enabled = true;
				GetComponent<AbejideAndando> ().enabled = true;
				GetComponent<AbejideAtaque> ().AtivarCodigo ();
				GetComponent<AbejideAndando> ().AtivarCodigo ();
				break;
			}

		} else {
			if (GetComponent<MetodosDaCutscene> ().PegarAto () == 9) {
				if (terreno.GetComponent<DadosDaFase> ().atores [2].GetComponent<MetodosDaCutscene> ().PegarAto () > 11) {
					GetComponent<MetodosDaCutscene> ().MudarEstaAtuando (false);
				}
			}

			tempoEspera += Time.deltaTime;
		}
	}
}
