using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class SeteEncruzilhadasAto01 : MonoBehaviour {

	private MetodosDaCutscene meuMetodosDaCutscene;
	public GameObject terreno;
	private Animator atorAnimator;
	public RuntimeAnimatorController controllerInicial;
	public RuntimeAnimatorController controllerCutscene;
	public Canvas canvasTelaPreta;
	private Image telaPreta;
	private Text texto;

	public float[] rotacoes;
	private float tempoEspera;
	private float maxTempoEspera;
	private float velocidade;

	private int indiceRotacoes;
	private int sat;

	private string fala;

	// Use this for initialization
	void Start () {
		CarcereiroAto01[] objTemp = FindObjectsOfType<CarcereiroAto01> ();
		for (int i = 0; i < objTemp.Length; i++) {
			objTemp [i].enabled = false;
		}

		atorAnimator = GetComponentInChildren <Animator> ();
		atorAnimator.runtimeAnimatorController = controllerCutscene as RuntimeAnimatorController;
		atorAnimator.SetInteger ("IndiceGatilho", 0);
		atorAnimator.SetTrigger ("Gatilho");

		indiceRotacoes = 0;
		tempoEspera = 10;
		maxTempoEspera = 0;
		velocidade = 1;

		meuMetodosDaCutscene = GetComponent<MetodosDaCutscene> ();
		meuMetodosDaCutscene.PosicionarInicial ();

		canvasTelaPreta.enabled = true;
		telaPreta = canvasTelaPreta.GetComponentInChildren <Image> ();
		texto = canvasTelaPreta.GetComponentInChildren <Text> ();
		texto.text = "";
		telaPreta.color = new Color (0, 0, 0, 1);

		sat = meuMetodosDaCutscene.PegarSat ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!meuMetodosDaCutscene.PegarEstaAtuando () && tempoEspera > maxTempoEspera) {

			switch (meuMetodosDaCutscene.PegarAto ()) {
			case 0:
				if (meuMetodosDaCutscene.AtorJaPassouDoAto(1, 2)) {
					fala = "Não subestime o garoto, não esqueça que este jovem levou Olodumare ao limite, meu caro.";
					GetComponent<MetodosDaCutscene> ().Falar (fala, GetComponent<MetodosDaCutscene> ().PegarNome (), 0, 6 * sat, true);
				}
				break;
			case 1:
				if (meuMetodosDaCutscene.AtorJaPassouDoAto(1, 6)) {
					fala = "Eu vou começar pois tenho preferência no quesito idade.";
					meuMetodosDaCutscene.Falar (fala, GetComponent<MetodosDaCutscene> ().PegarNome (), 2, 6 * sat, true);
					tempoEspera = 0;
					maxTempoEspera = 4.5f * sat;
				}
				break;
			case 2:
				meuMetodosDaCutscene.ComecarAtuacaoPosicao (velocidade, true, false, 0);
				break;
			case 3:
				meuMetodosDaCutscene.ComecarAtuacaoPosicao (velocidade, false, false, 0);
				break;
			case 4:
				if (meuMetodosDaCutscene.AtorJaPassouDoAto(0, 6)) {
					fala = "É melhor que tu fiques por bem meu jovem. Tu causou problemas de mais para nós. Não brimques com nossa paciência.";
					meuMetodosDaCutscene.Falar (fala, meuMetodosDaCutscene.PegarNome (), 3, 9.5f * sat, true);
				}
				break;
			case 5:
				meuMetodosDaCutscene.ComecarAtuacaoPosicao (velocidade, false, false, 2.3f);
				break;
			case 6:
				meuMetodosDaCutscene.ComecarAtuacaoRotacao (rotacoes [indiceRotacoes], velocidade, 0);
				indiceRotacoes++;
				break;
			case 7:
				fala = "Faça isso novamente e você pagará um preço ainda maior do que aquele que temos em mente.";
				meuMetodosDaCutscene.Falar (fala, meuMetodosDaCutscene.PegarNome (), 0, 7 * sat, true);
				tempoEspera = 0;
				maxTempoEspera = 7.1f * sat;
				break;
			case 8:
				fala = "Eu sei para onde iremos meu jovem. Mas acho que você não achará meu reino tão agradável quanto eu...";
				meuMetodosDaCutscene.Falar (fala, meuMetodosDaCutscene.PegarNome (), 0, 7 * sat, true);
				tempoEspera = 0;
				maxTempoEspera = 8 * sat;
				break;
			case 9:
				canvasTelaPreta.enabled = true;
				meuMetodosDaCutscene.IncrementarAto ();
				break;
			case 10:
				meuMetodosDaCutscene.ComecarAtuacaoMoverNoEixoY (4.2f, false, false, 1.8f, 0);
				break;
			case 11:
				meuMetodosDaCutscene.IncrementarAto ();
				break;
			}
		} else {
			tempoEspera += Time.deltaTime;
		}

		//Faz a fade do preto para o transparente.
		if (meuMetodosDaCutscene.PegarAto () < 3) {
			telaPreta.color = new Color (0, 0, 0, Mathf.Lerp(telaPreta.color.a, 0, 0.6f * Time.deltaTime));

			if (telaPreta.color.a < 0.02f) {
				canvasTelaPreta.enabled = false;
			}
		} else if (meuMetodosDaCutscene.PegarAto () > 9) {
			telaPreta.color = new Color (0, 0, 0, Mathf.Lerp(telaPreta.color.a, 1, 1.2f * Time.deltaTime));
		}
	}
}
