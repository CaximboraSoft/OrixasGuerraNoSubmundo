using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeteEncruzilhadasAto02 : MonoBehaviour {

	public GameObject terreno;
	private Animator atorAnimator;
	public RuntimeAnimatorController controllerCutscene;
	private MetodosDaCutscene meuMetodosDaCutscene;

	private float tempoEspera;
	private float maxTempoEspera;

	private string fala;

	void Start () {
		if (GetComponent<SeteEncruzilhadasAto02> ().enabled) {
			AtivarCodigo ();
		}
	}

	// Use this for initialization
	public void AtivarCodigo () {
		atorAnimator = GetComponentInChildren<Animator> ();
		atorAnimator.runtimeAnimatorController = controllerCutscene as RuntimeAnimatorController;

		tempoEspera = 10;
		maxTempoEspera = 0;

		meuMetodosDaCutscene = GetComponent<MetodosDaCutscene> ();
		meuMetodosDaCutscene.MudarNome("Exu sete encruzilhadas:");
		meuMetodosDaCutscene.posicoesNome = "SeteEncruzilhadasPosicoes1";
		meuMetodosDaCutscene.MudarIndicePosicao (0);
		meuMetodosDaCutscene.MudarAtor (0);
		meuMetodosDaCutscene.PosicionarInicial ();

		GetComponent<SeteEncruzilhadasAto02> ().enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (!meuMetodosDaCutscene.PegarEstaAtuando () && tempoEspera > maxTempoEspera) {

			switch (meuMetodosDaCutscene.PegarAto ()) {
			case 0:
				fala = "Esta será a sua nova casa.";
				meuMetodosDaCutscene.Falar (fala, meuMetodosDaCutscene.PegarNome (), 5, 5, true);
				tempoEspera = 0;
				maxTempoEspera = 5 + 5;
				break;
			case 1:
				meuMetodosDaCutscene.IncrementarAto ();
				break;
			case 2:
				if (meuMetodosDaCutscene.AtorJaPassouDoAto(0, 1)) {
					fala = "Vamos nos divertir.";
					meuMetodosDaCutscene.Falar (fala, meuMetodosDaCutscene.PegarNome (), 0, 4, true);
					tempoEspera = 0;
					maxTempoEspera = 4;
				}
				break;
			case 3:
				meuMetodosDaCutscene.IncrementarAto ();
				break;
			case 4:
				if (meuMetodosDaCutscene.AtorJaPassouDoAto(0, 8)) {
					fala = "Mas o que é isso?";
					meuMetodosDaCutscene.Falar (fala, meuMetodosDaCutscene.PegarNome (), 0, 3, true);
					tempoEspera = 0;
					maxTempoEspera = 3;
				}
				break;
			case 5:
				fala = "Sou um velho cansado, mas não vou deixar que sai assim, meu caro.";
				meuMetodosDaCutscene.Falar (fala, meuMetodosDaCutscene.PegarNome (), 0, 3, true);
				tempoEspera = 0;
				maxTempoEspera = 3;
				break;
			case 6:
				fala = "Carcerreiros! cuidem dele. Eu voltarei em breve.";
				meuMetodosDaCutscene.Falar (fala, meuMetodosDaCutscene.PegarNome (), 0, 3, true);
				tempoEspera = 0;
				maxTempoEspera = 3;
				break;
			case 7:
				meuMetodosDaCutscene.ComecarAtuacaoPosicao (1f, true, false, 0);
				break;
			case 8:
				meuMetodosDaCutscene.ComecarAtuacaoPosicao (1f, true, false, 0);
				break;
			}
		} else {
			tempoEspera += Time.deltaTime;
		}
	}
}
