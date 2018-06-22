using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class SplashScreen : MonoBehaviour {

	public RawImage splashScreen;
	public Image menu;

	// Use this for initialization
	void Start () {
		splashScreen.enabled = true;
		menu.enabled = false;

		StartCoroutine ("MostrarMenu");
	}

	IEnumerator MostrarMenu () {
		yield return new WaitForSeconds (11);
		splashScreen.enabled = false;
		menu.GetComponent<Animator> ().SetTrigger ("Ativar");
		menu.enabled = true;

		yield return new WaitForSeconds (2);
		GetComponent<LevelManager> ().CarregarCena ("Menu");
	}
}
