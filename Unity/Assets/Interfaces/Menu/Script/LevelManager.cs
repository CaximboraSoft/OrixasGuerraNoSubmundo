using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public Canvas telaFinal;
	public Text telaFinalText;

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
		telaFinalText.text = "Pontuação:\n" + FindObjectOfType<DadosDaFase> ().pontuacao.ToString ();

		telaFinal.enabled = true;
	}
}
