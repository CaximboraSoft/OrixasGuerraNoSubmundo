using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public void CarregarCena (string nomeDaCena) {
		SceneManager.LoadScene (nomeDaCena);
	}

	public void SairDoJogo () {
		Application.Quit ();
	}
}
