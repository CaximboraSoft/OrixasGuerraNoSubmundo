using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public Canvas telaFinal;
	public Text telaFinalText;

	public void IndiomaPortugues () {
		Data.portugues = 1;
		MudarIndiomaBotao ();
	}

	public void IndiomaIngles () {
		Data.portugues = 0;
		MudarIndiomaBotao ();
	}

	private void MudarIndiomaBotao () {
		PlayerPrefs.SetInt ("Indioma", Data.portugues);

		BotoesIndioma[] botoes = FindObjectsOfType<BotoesIndioma> ();

		for (int i = 0; i < botoes.Length; i++) {
			botoes [i].MudarIndioma ();
		}

		PtEn[] pten = FindObjectsOfType<PtEn> ();
		if (pten.Length != 0) {
			for (int i = 0; i < pten.Length; i++) {
				pten [i].MudarIndioma ();
			}
		}

		MudarIndiomaDropDown[] mudarIndiomaDropDown = FindObjectsOfType<MudarIndiomaDropDown> ();
		if (mudarIndiomaDropDown.Length != 0) {
			for (int i = 0; i < mudarIndiomaDropDown.Length; i++) {
				mudarIndiomaDropDown [i].MudarIndioma ();
			}
		}
	}

	public void CarregarCena (string nomeDaCena) {
		SceneManager.LoadScene (nomeDaCena);
	}

	public void SairDoJogo () {
		Application.Quit ();
	}

	public void MostrarTelaFinal () {
		StartCoroutine ("MostrarTelaFinalTempo");
	}

	IEnumerator MostrarTelaFinalTempo () {
		yield return new WaitForSeconds (3);
		FindObjectOfType<Abejide> ().enabled = false;
		InimigosNormais[] inimigos = FindObjectsOfType<InimigosNormais> ();

		for (int i = 0; i < inimigos.Length; i++) {
			Destroy (inimigos [i].gameObject);
		}

		FindObjectOfType<Abejide> ().enabled = false;

		if (FindObjectOfType<DadosDaFase> ().PegarIndioma ()) {
			telaFinalText.text = "Pontuação:\n" + FindObjectOfType<DadosDaFase> ().pontuacao.ToString ();
		} else {
			telaFinalText.text = "Score:\n" + FindObjectOfType<DadosDaFase> ().pontuacao.ToString ();
		}

		telaFinal.enabled = true;
	}
}
