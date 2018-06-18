using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbejideAto01 : MonoBehaviour {

	public GameObject cutsceneObjetos;
	private Animator corpoAnimator;
	public RuntimeAnimatorController animacaoDaCurscene;
	public PedraGigante pedraGigante;
	private MetodosDaCutscene meuMetodosDaCutscene;

	public float[] rotacoes;
	private int indiceRotacoes;
	private float tempoEspera;
	private float maxTempoEspera;

	private int sat;

	private string fala;

	// Use this for initialization
	void Start () {
		corpoAnimator = GetComponentInChildren<Animator> ();
		corpoAnimator.runtimeAnimatorController = animacaoDaCurscene as RuntimeAnimatorController;

		pedraGigante.transform.SetParent (transform);
		pedraGigante.transform.localPosition = new Vector3 (0f, -0.9f, 0f);

		tempoEspera = 10;
		maxTempoEspera = 0;

		meuMetodosDaCutscene = GetComponent<MetodosDaCutscene> ();
		GetComponent<MetodosDaCutscene> ().PosicionarInicial ();

		InimigosNormais[] objInimigosNormais = FindObjectsOfType<InimigosNormais> ();
		for (int i = 0; i < objInimigosNormais.Length; i++) {
			objInimigosNormais [i].enabled = false;
		}
	}

	// Update is called once per frame
	void Update () {
		sat = meuMetodosDaCutscene.PegarSat ();

		if (Input.GetKeyDown (KeyCode.Return)) {
			if (pedraGigante != null) {
				Destroy (pedraGigante.gameObject);
			}

			Destroy (cutsceneObjetos);
			GetComponent<AbejideAto02> ().PularCutscene ();
			GetComponent<AbejideAto01> ().enabled = false;
		}

		if (!meuMetodosDaCutscene.PegarEstaAtuando () && tempoEspera > maxTempoEspera) {

			switch (meuMetodosDaCutscene.PegarAto ()) {
			case 0:
				InimigosNormais[] inimigosNormaisTemp = FindObjectsOfType<InimigosNormais> ();
				for (int i = 0; i < inimigosNormaisTemp.Length; i++) {
					inimigosNormaisTemp [i].enabled = false;
				}

				corpoAnimator.SetInteger ("IndiceGatilho", 0); //Pedrificado.
				corpoAnimator.SetTrigger ("Gatilho");
				meuMetodosDaCutscene.ComecarAtuacaoMoverNoEixoY (2f, true, false, 1, 0);
				break;
			case 1:
				meuMetodosDaCutscene.IncrementarAto ();
				break;
			case 2:
				if (meuMetodosDaCutscene.AtorJaPassouDoAto (1, 6)) {
					fala = "...";
					meuMetodosDaCutscene.Falar(fala, meuMetodosDaCutscene.PegarNome (), 0, 3 * sat, true);
				}
				break;
			case 3:
				if (meuMetodosDaCutscene.AtorJaPassouDoAto (2, 3)) {
					corpoAnimator.SetInteger ("IndiceGatilho", 2); //Quebra a pedra.
					corpoAnimator.SetTrigger ("Gatilho");
					fala = "Seu velho tolo!!!";
					meuMetodosDaCutscene.Falar (fala, meuMetodosDaCutscene.PegarNome (), 1, 4 * sat, true);
					tempoEspera = 0;
					maxTempoEspera = 4.5f * sat;
					pedraGigante.ExplodirPedras ();
				}
				break;
			case 4: //Abejide começa a tentar fugir.
				corpoAnimator.SetTrigger ("VoltarHaAndar");
				meuMetodosDaCutscene.ComecarAtuacaoPosicao (2.5f, false, true, 0);
				break;
			case 5:
				if (meuMetodosDaCutscene.AtorJaPassouDoAto (1, 10)) {
					corpoAnimator.SetInteger ("IndiceGatilho", 1); //Paralizar andando no ar.
					corpoAnimator.SetTrigger ("Gatilho");
					tempoEspera = 0;
					maxTempoEspera = 1f * sat;
					meuMetodosDaCutscene.IncrementarAto ();
				}
				break;
			case 6:
				meuMetodosDaCutscene.ComecarAtuacaoMoverNoEixoY (2f, true, true, 2, 0);
				break;
			case 7:
				GetComponent<Collider> ().enabled = true;
				meuMetodosDaCutscene.ComecarAtuacaoPosicao (2f, false, false, 0);
				break;
			case 8:
				meuMetodosDaCutscene.ComecarAtuacaoSeguirOutroAtor (meuMetodosDaCutscene.PegarOutroAtor(2), false, true, false);
				break;
			case 9: //Só entra qui quando o sete encruzilhaja ja tiver passado do chão.
				Destroy (cutsceneObjetos);
				Destroy (pedraGigante.gameObject);

				GameObject.FindObjectOfType<SeteEncruzilhadasAto01> ().enabled = false;
				GameObject.FindObjectOfType<SeteEncruzilhadasAto02> ().AtivarCodigo ();

				GetComponent<AbejideAto01> ().enabled = false;
				GetComponent<AbejideAto02> ().AtivarCodigo ();
				break;
			}

		} else {
			if (meuMetodosDaCutscene.PegarAto () == 9) {
				if (meuMetodosDaCutscene.AtorJaPassouDoAto (2, 11)) {
					meuMetodosDaCutscene.MudarEstaAtuando (false);
				}
			}

			tempoEspera += Time.deltaTime;
		}
	}
}
