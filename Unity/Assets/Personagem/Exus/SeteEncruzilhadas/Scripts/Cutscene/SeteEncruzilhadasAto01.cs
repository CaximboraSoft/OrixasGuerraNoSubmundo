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

	private string fala;

	private int sat;

	// Use this for initialization
	void Start () {
		atorAnimator.runtimeAnimatorController = controllerCutscene as RuntimeAnimatorController;

		tempoEspera = 10;
		maxTempoEspera = 0;

		GetComponent<MetodosDaCutscene> ().MudarNome("Exu sete encruzilhadas:");

		sat = terreno.GetComponent<DadosDaFase> ().sat;
	}
	
	// Update is called once per frame
	void Update () {
		atorAnimator.SetFloat ("NewtonAndando", GetComponent<DadosForcaResultante> ().PegarNewtonAndando ());

		if (terreno.GetComponent<DadosDaFase> ().atores[0].GetComponent<MetodosDaCutscene> ().enabled == false) {
			Destroy (gameObject);
		}

		if (!GetComponent<MetodosDaCutscene> ().PegarEstaAtuando () && tempoEspera > maxTempoEspera) {

			switch (GetComponent<MetodosDaCutscene> ().PegarAto ()) {
			case 0:
				if (terreno.GetComponent<DadosDaFase> ().atores[1].GetComponent<MetodosDaCutscene> ().PegarAto () > 2) {
					fala = "Não substime o garoto, não esqueça que este jovem levou Olodumare ao limite, meu caro.";
					GetComponent<MetodosDaCutscene> ().Falar (fala, GetComponent<MetodosDaCutscene> ().PegarNome (), 0, 5 * sat, true);
				}
				break;
			case 1:
				if (terreno.GetComponent<DadosDaFase> ().atores [1].GetComponent<MetodosDaCutscene> ().PegarAto () > 6) {
					fala = "Eu vou começar, pois tenho preferência em idade.";
					GetComponent<MetodosDaCutscene> ().Falar (fala, GetComponent<MetodosDaCutscene> ().PegarNome (), 2, 5 * sat, true);
					tempoEspera = 0;
					maxTempoEspera = 4.5f * sat;
				}
				break;
			case 2:
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoPosicao (false, false, true, false, 0);
				break;
			case 3:
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoPosicao (true, false, false, false, 0);
				break;
			case 4:
				if (terreno.GetComponent<DadosDaFase> ().atores [0].GetComponent<MetodosDaCutscene> ().PegarAto () > 6) {
					fala = "É melhor que tu fiques por bem, meu jovem. ja causou problemas de mais para nós, não brimques com a nossa paciência.";
					GetComponent<MetodosDaCutscene> ().Falar (fala, GetComponent<MetodosDaCutscene> ().PegarNome (), 3, 9.5f, true);
				}
				break;
			case 5:
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoPosicao (true, false, false, false, 2.3f);
				break;
			case 6:
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoRotacao (false, false, 0);
				break;
			case 7:
				fala = "Faça isso de novo, e você pagará um preço ainda maior do que aquele que temos em mente.";
				GetComponent<MetodosDaCutscene> ().Falar (fala, GetComponent<MetodosDaCutscene> ().PegarNome (), 0, 6 * sat, true);
				tempoEspera = 0;
				maxTempoEspera = 6.1f * sat;
				break;
			case 8:
				fala = "Ja sei para onde vamos ir meu jovem, mas acho qe você não achará meu reino tão agradável quanto eu...";
				GetComponent<MetodosDaCutscene> ().Falar (fala, GetComponent<MetodosDaCutscene> ().PegarNome (), 0, 7 * sat, true);
				tempoEspera = 0;
				maxTempoEspera = 8 * sat;
				break;
			case 9:
				GetComponent<MetodosDaCutscene> ().IncrementarAto ();
				break;
			case 10:
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoMoverNoEixoY (4.2f, false, false, false, 3, 0);
				GetComponent<Collider> ().enabled = false;
				GetComponent<Rigidbody> ().useGravity = false;
				break;
			case 11:
				GetComponent<MetodosDaCutscene> ().IncrementarAto ();
				break;
			}
		} else {
			tempoEspera += Time.deltaTime;
		}
	}
}
