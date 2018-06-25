using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrancaRuaAto01 : MonoBehaviour {

	private MetodosDaCutscene meuMetodosDaCutscene;
	private Animator atorAnimator;
	public RuntimeAnimatorController controllerCutscene;

	public float[] rotacoes;
	private int indiceRotacoes;
	private float tempoEspera;
	private float maxTempoEspera;
	public float velocidade = 1f;

	private int sat;

	private string fala;

	// Use this for initialization
	void Start () {
		atorAnimator = GetComponentInChildren<Animator> ();
		atorAnimator.SetInteger ("IndiceGatilho", 0);
		atorAnimator.SetTrigger ("Gatilho");

		meuMetodosDaCutscene = GetComponent<MetodosDaCutscene> ();
		meuMetodosDaCutscene.PosicionarInicial ();

		tempoEspera = 10;
		maxTempoEspera = 0;

		sat = meuMetodosDaCutscene.PegarSat ();
	}
	
	// Update is called once per frame
	void Update () {
		if (meuMetodosDaCutscene.PegarOutroAtor(0).GetComponent<AbejideAto01> ().enabled == false) {
			Destroy (gameObject);
		}

		if (!meuMetodosDaCutscene.PegarEstaAtuando () && tempoEspera > maxTempoEspera) {

			switch (meuMetodosDaCutscene.PegarAto ()) {
			case 0:
				if (meuMetodosDaCutscene.PegarIndioma ()) {
					fala = "Sua magia inconveniente não tem efeito por essas terras.";
				} else {
					fala = "Ingles";
				}
				meuMetodosDaCutscene.Falar (fala, meuMetodosDaCutscene.PegarNome (), 1, 5 * sat, true);
				tempoEspera = 0;
				maxTempoEspera = 3f * sat;
				break;
			case 1:
				meuMetodosDaCutscene.ComecarAtuacaoPosicao(velocidade, true, false, 0);
				break;
			case 2:
				meuMetodosDaCutscene.ComecarAtuacaoPosicao(velocidade , true, false, 0);
				break;
			case 3:
				meuMetodosDaCutscene.ComecarAtuacaoPosicao(velocidade, false, false, 0);
				break;
			case 4:
				if (meuMetodosDaCutscene.PegarIndioma ()) {
					fala = "Há que bobagem. Se não fosse por nós ele estaria morto assim como todas as coisas vivas.";
				} else {
					fala = "Ingles";
				}
				meuMetodosDaCutscene.Falar (fala, meuMetodosDaCutscene.PegarNome (), 0, 7 * sat, true);
				tempoEspera = 0;
				maxTempoEspera = 7 * sat;
				break;
			case 5:
				if (meuMetodosDaCutscene.PegarIndioma ()) {
					fala = "E agora, como tu desejaste seu bem própio iremos balancear o karma a nosso favor. Que comece a punição.";
				} else {
					fala = "Ingles";
				}
				meuMetodosDaCutscene.Falar (fala, meuMetodosDaCutscene.PegarNome (), 0, 8 * sat, true);
				tempoEspera = 0;
				maxTempoEspera = 8.1f * sat;
				break;
			case 6: //Para o Abejide falar os tres pontinhos.
				meuMetodosDaCutscene.ComecarAtuacaoPosicao (velocidade, true, false, 0);
				break;
			case 7:
				meuMetodosDaCutscene.ComecarAtuacaoPosicao (velocidade, true, false, 0);
				break;
			case 8:
				meuMetodosDaCutscene.ComecarAtuacaoPosicao (velocidade, false, false, 0);
				break;
			case 9:
				meuMetodosDaCutscene.ComecarAtuacaoRotacao (rotacoes [indiceRotacoes], velocidade, 0);
				indiceRotacoes++;
				break;
			case 10:
				if (meuMetodosDaCutscene.PegarIndioma ()) {
					fala = "Que ousado! De fato não é nada fraco.";
				} else {
					fala = "Ingles";
				}
				meuMetodosDaCutscene.Falar (fala, meuMetodosDaCutscene.PegarNome (), 0, 5 * sat, true);
				tempoEspera = 0;
				maxTempoEspera = 1f * sat;
				break;
			case 11:
				atorAnimator.SetInteger ("IndiceGatilho", 1);
				atorAnimator.SetTrigger ("Gatilho");
				meuMetodosDaCutscene.IncrementarAto ();
				break;
			case 12:
				break;
			}
		} else {
			tempoEspera += Time.deltaTime;
		}
	}
}
