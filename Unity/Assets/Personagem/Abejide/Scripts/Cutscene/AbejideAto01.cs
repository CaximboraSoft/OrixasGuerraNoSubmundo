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

	private string nome;

	// Use this for initialization
	void Start () {
		hud.enabled = false;
		corpoAnimator.runtimeAnimatorController = corpoController as RuntimeAnimatorController;
		peAnimator.runtimeAnimatorController = peController as RuntimeAnimatorController;
		GetComponent<Collider> ().enabled = false;
		GetComponent<Rigidbody> ().useGravity = false;

		tempoEspera = 10;
		maxTempoEspera = 0;

		nome = "Abejide:";
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Space)) {
			GetComponent<MetodosDaCutscene> ().MudarEstaAtuando (false);
			GetComponent<MetodosDaCutscene> ().MudarAtor (10);
		}

		corpoAnimator.SetFloat ("AceleracaoAndando", GetComponent<DadosForcaResultante> ().PegarNewtonAndando ());
		peAnimator.SetFloat ("AceleracaoAndando", GetComponent<DadosForcaResultante> ().PegarNewtonAndando ());

		if (!GetComponent<MetodosDaCutscene> ().PegarEstaAtuando () && tempoEspera > maxTempoEspera) {
			
			switch (GetComponent<MetodosDaCutscene> ().PegarAto ()) {
			case 0:
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoMoverNoEixoY (1.9f, true, false, false, 1, 2);
				GetComponent<MetodosDaCutscene> ().Falar ("Eae veio tarado, tô chegando e quero a minha maconha.", nome, 2, 9, true);
				break;
			case 1:
				GetComponent<Collider> ().enabled = true;
				GetComponent<Rigidbody> ().useGravity = true;
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoOlharParaObjeto (terreno.GetComponent<DadosDaFase> ().atores [1], true, false);
				break;
			case 2:
				//Comeca a tentar fugir.
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoPosicao (true, true, false, false, 0);
				GetComponent<MetodosDaCutscene> ().Falar ("Esquece cara, vou sair dessa merda.", nome, 0, 6, true);
				break;
			case 3:
				if (terreno.GetComponent<DadosDaFase> ().atores [1].GetComponent<MetodosDaCutscene> ().PegarAto () > 8) {
					corpoAnimator.SetTrigger ("Paralizar");
					peAnimator.SetTrigger ("Paralizar");
					tempoEspera = 0;
					maxTempoEspera = 2.5f;
					GetComponent<MetodosDaCutscene> ().IncrementarAto ();
				}
				break;
			case 4:
				GetComponent<Collider> ().enabled = false;
				GetComponent<Rigidbody> ().useGravity = false;
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoMoverNoEixoY (0.7f, true, false, false, 1, 0);
				break;
			case 5:
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoRotacao (true, true, 0);
				break;
			case 6:
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoPosicao (true, true, false, false, 0);
				GetComponent<MetodosDaCutscene> ().Falar ("Que diabo de magia é essa meu, você é filho do diabo satanas.", nome, 0, 4, true);
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
				GetComponent<MetodosDaCutscene> ().MudarIndicePosicao (2);
				GetComponent<DadosForcaResultante> ().MudarNewtonAndando (0);
				GetComponent<DadosForcaResultante> ().MudarNewtonRodando (0);
				GetComponent<AbejideAndando> ().AtivarCodigo ();
				GetComponent<AbejideAtaque> ().AtivarCodigo ();
				GetComponent<Collider> ().enabled = true;
				GetComponent<Rigidbody> ().useGravity = true;
				hud.enabled = true;
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoTeleporte ();
				GetComponent<MetodosDaCutscene> ().boca.GetComponent<Conversas> ().conversas.enabled = false;
				GetComponent<MetodosDaCutscene> ().boca.GetComponent<Conversas> ().enabled = false;
				GetComponent<AbejideAto01> ().enabled = false;
				GetComponent<MetodosDaCutscene> ().enabled = false;
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

	public string PegarNome () {
		return nome;
	}
}
