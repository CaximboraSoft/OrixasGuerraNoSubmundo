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

	private string nome;

	// Use this for initialization
	void Start () {
		atorAnimator.runtimeAnimatorController = controllerCutscene as RuntimeAnimatorController;

		tempoEspera = 10;
		maxTempoEspera = 0;

		nome = "Exu sete encruzilhadas:";
	}
	
	// Update is called once per frame
	void Update () {
		atorAnimator.SetFloat ("NewtonAndando", GetComponent<DadosForcaResultante> ().PegarNewtonAndando ());

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
				GetComponent<MetodosDaCutscene> ().Falar ("Oh seu lixo filho do satanas, deixa esse neguinho comigo, vou matar ele e toda a sua familia.", nome, 0, 8, true);
				break;
			case 2:
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoPosicao (true, false, true, false, 0);
				break;
			case 3:
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoPosicao (false, false, false, false, 0);
				break;
			case 4:
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoRotacao (false, false, 3); //Indice 1
				GetComponent<MetodosDaCutscene> ().boca.GetComponent<Conversas> ().MudarRostoMostrar(terreno.GetComponent<DadosDaFase> ().atores[0].GetComponent<MetodosDaCutscene> ().rostoMostrar);
				GetComponent<MetodosDaCutscene> ().Falar ("Cara, a minha família já morreu de overdose de maconha a muito tempo atrás.", terreno.GetComponent<DadosDaFase> ().atores[0].GetComponent<AbejideAto01> ().PegarNome (), 0, 6, false);
				break;
			case 5:
				atorAnimator.GetComponent<Collider> ().enabled = false;
				GetComponent<Rigidbody> ().useGravity = false;
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoMoverNoEixoY (2.7f, false, true, false, 1, 0);
				GetComponent<MetodosDaCutscene> ().Falar ("isso é o que você pensa, por que eu sou seu pai...", nome, 1, 5, true);
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
