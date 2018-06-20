using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarcereiroAto01 : MonoBehaviour {

	private MetodosDaCutscene meuMetodosDaCutscene;
	private Animator atorAnimator;
	public RuntimeAnimatorController controllerOriginal;

	public float[] rotacoes;
	private int indiceRotacoes;
	private float tempoEspera;
	private float maxTempoEspera;
	public float velocidade = 1f;

	void Awake () {
		atorAnimator = GetComponentInChildren<Animator> ();

		meuMetodosDaCutscene = GetComponent<MetodosDaCutscene> ();
	}

	// Use this for initialization
	public void AtivarCodigo () {
		meuMetodosDaCutscene.PosicionarInicial ();
		meuMetodosDaCutscene.enabled = true;

		tempoEspera = 10;
		maxTempoEspera = 0;

		indiceRotacoes = 0;

		GetComponent<CarcereiroAto01> ().enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (!meuMetodosDaCutscene.PegarEstaAtuando () && tempoEspera > maxTempoEspera) {

			switch (meuMetodosDaCutscene.PegarAto ()) {
			case 0:
				if (meuMetodosDaCutscene.AtorJaPassouDoAto (2, 7)) {
					meuMetodosDaCutscene.ComecarAtuacaoPosicao (velocidade, false, false, 0);
				}
				break;
			case 1:
				meuMetodosDaCutscene.ComecarAtuacaoRotacao (rotacoes [indiceRotacoes], 1, 0);
				indiceRotacoes++;
				break;
			}
		}
	}

	public void AcabouCutscene () {
		GetComponentInChildren<Arma> ().enabled = true;
		atorAnimator.runtimeAnimatorController = controllerOriginal as RuntimeAnimatorController;
		Destroy (GetComponent<MetodosDaCutscene> ());
		Destroy (GetComponent<CarcereiroAto01> ());
	}

	public void PularCutscene () {
		transform.rotation = Quaternion.Euler (new Vector3 (0, rotacoes[rotacoes.Length - 1], 0));
		
		meuMetodosDaCutscene.MudarIndicePosicao (0);
		meuMetodosDaCutscene.ComecarAtuacaoTeleporte ();

		AcabouCutscene ();
	}
}
