using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrancaRuaAto01 : MonoBehaviour {

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

		GetComponent<MetodosDaCutscene> ().MudarNome("Tranca Rua:");

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
				fala = "Sua magia incoveniente não tem efeito por essas terras.";
				GetComponent<MetodosDaCutscene> ().Falar (fala, GetComponent<MetodosDaCutscene> ().PegarNome (), 1, 4 * sat, true);
				tempoEspera = 0;
				maxTempoEspera = 2f * sat;
				break;
			case 1:
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoPosicao(true, false, true, false, 0);
				break;
			case 2:
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoPosicao(true, false, false, false, 1);
				break;
			case 3:
				fala = "Há que bobagem, Se não fosse por nós ele estaria morto assim como todas as coisa vivas.";
				GetComponent<MetodosDaCutscene> ().Falar (fala, GetComponent<MetodosDaCutscene> ().PegarNome (), 0, 7 * sat, true);
				tempoEspera = 0;
				maxTempoEspera = 7 * sat;
				break;
			case 4:
				fala = "E agora, como tu desejaste seu bem própio iremos balancear o karma a nosso favor. Que comece a punição.";
				GetComponent<MetodosDaCutscene> ().Falar (fala, GetComponent<MetodosDaCutscene> ().PegarNome (), 0, 8 * sat, true);
				tempoEspera = 0;
				maxTempoEspera = 8.1f * sat;
				break;
			case 5: //Para o Abejide falar os tres pontinhos.
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoPosicao (false, false, true, false, 0);
				break;
			case 6:
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoPosicao (false, false, true, false, 0);
				break;
			case 7:
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoPosicao (false, false, false, false, 0);
				break;
			case 8:
				GetComponent<MetodosDaCutscene> ().ComecarAtuacaoRotacao (true, false, 3);
				break;
			case 9:
				fala = "Que ousado! De fato não é nada fraco.";
				GetComponent<MetodosDaCutscene> ().Falar (fala, GetComponent<MetodosDaCutscene> ().PegarNome (), 0, 5 * sat, true);
				tempoEspera = 0;
				maxTempoEspera = 1f * sat;
				break;
			case 10:
				atorAnimator.SetInteger ("Ato", 9);
				atorAnimator.SetTrigger ("MudarAnimacao");
				GetComponent<MetodosDaCutscene> ().IncrementarAto ();
				break;
			case 11:
				break;
			}
		} else {
			tempoEspera += Time.deltaTime;
		}
	}
}
