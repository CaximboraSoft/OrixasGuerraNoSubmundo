using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarcereiroAto01 : MonoBehaviour {

	private MetodosDaCutscene meuMetodosDaCutscene;
	private Animator atorAnimator;
	public RuntimeAnimatorController controllerOriginal;
	public RuntimeAnimatorController controllerCutscene;

	public float[] rotacoes;
	private int indiceRotacoes;
	private float tempoEspera;
	private float maxTempoEspera;
	public float velocidade = 1f;

	// Use this for initialization
	void Start () {
		atorAnimator = GetComponentInChildren<Animator> ();
		atorAnimator.runtimeAnimatorController = controllerCutscene as RuntimeAnimatorController;

		meuMetodosDaCutscene = GetComponent<MetodosDaCutscene> ();
		meuMetodosDaCutscene.PosicionarInicial ();

		tempoEspera = 10;
		maxTempoEspera = 0;

		indiceRotacoes = 0;
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
}
