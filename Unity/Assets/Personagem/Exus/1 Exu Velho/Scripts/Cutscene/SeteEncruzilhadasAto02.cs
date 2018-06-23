using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeteEncruzilhadasAto02 : MonoBehaviour {

	public GameObject terreno;
	private MetodosDaCutscene meuMetodosDaCutscene;
	public Transform batalhaFinal;

	private float tempoEspera;
	private float maxTempoEspera;

	private int sat;

	private string fala;

	void Start () {
		if (GetComponent<SeteEncruzilhadasAto02> ().enabled) {
			AtivarCodigo ();
		}

		sat = meuMetodosDaCutscene.PegarSat ();
	}

	// Use this for initialization
	public void AtivarCodigo () {
		tempoEspera = 10;
		maxTempoEspera = 0;

		meuMetodosDaCutscene = GetComponent<MetodosDaCutscene> ();
		meuMetodosDaCutscene.MudarNome("Exu sete encruzilhadas:");
		meuMetodosDaCutscene.posicoesNome = "SeteEncruzilhadasPosicoes1";
		meuMetodosDaCutscene.MudarIndicePosicao (0);
		meuMetodosDaCutscene.MudarAtor (0);
		meuMetodosDaCutscene.PosicionarInicial ();

		GetComponent<Collider> ().enabled = true;
		GetComponent<Rigidbody> ().useGravity = true;

		GetComponent<SeteEncruzilhadasAto02> ().enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (!meuMetodosDaCutscene.PegarEstaAtuando () && tempoEspera > maxTempoEspera) {

			switch (meuMetodosDaCutscene.PegarAto ()) {
			case 0:
				CarcereiroAto01[] objTemp = FindObjectsOfType<CarcereiroAto01> ();
				for (int i = 0; i < objTemp.Length; i++) {
					objTemp [i].AtivarCodigo ();
				}

				fala = "Esta será sua nova casa.";
				meuMetodosDaCutscene.Falar (fala, meuMetodosDaCutscene.PegarNome (), 5, 5 * sat, true);
				tempoEspera = 0;
				maxTempoEspera = (5 + 5) * sat;
				break;
			case 1:
				meuMetodosDaCutscene.IncrementarAto ();
				break;
			case 2:
				if (meuMetodosDaCutscene.AtorJaPassouDoAto(0, 1)) {
					fala = "Vamos nos divertir.";
					meuMetodosDaCutscene.Falar (fala, meuMetodosDaCutscene.PegarNome (), 0, 4 * sat, true);
					tempoEspera = 0;
					maxTempoEspera = 4 * sat;
				}
				break;
			case 3:
				meuMetodosDaCutscene.IncrementarAto ();
				break;
			case 4:
				if (meuMetodosDaCutscene.AtorJaPassouDoAto(0, 8)) {
					fala = "Mas o que é isso?";
					meuMetodosDaCutscene.Falar (fala, meuMetodosDaCutscene.PegarNome (), 0, 3 * sat, true);
					tempoEspera = 0;
					maxTempoEspera = 3 * sat;
				}
				break;
			case 5:
				fala = "Sou um velho cansado, mas não vou deixar que saia assim, meu caro.";
				meuMetodosDaCutscene.Falar (fala, meuMetodosDaCutscene.PegarNome (), 0, 4 * sat, true);
				tempoEspera = 0;
				maxTempoEspera = 4 * sat;
				break;
			case 6:
				fala = "Carcereiros! Cuidem dele. Eu voltarei em breve.";
				meuMetodosDaCutscene.Falar (fala, meuMetodosDaCutscene.PegarNome (), 0, 4 * sat, true);
				tempoEspera = 0;
				maxTempoEspera = 4 * sat;
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

	public void ColocarNaBatalhaFinal () {
		transform.position = batalhaFinal.position;
		transform.rotation = batalhaFinal.rotation;

		Destroy (batalhaFinal.gameObject);

		GetComponent<SeteEncruzilhadasAto03> ().AtivarCodigo ();
		GetComponentInChildren<Arma> ().enabled = true;

		Destroy (GetComponent<MetodosDaCutscene> ());
		Destroy (GetComponent<SeteEncruzilhadasAto01> ());
		Destroy (GetComponent<SeteEncruzilhadasAto02> ());
	}
}
