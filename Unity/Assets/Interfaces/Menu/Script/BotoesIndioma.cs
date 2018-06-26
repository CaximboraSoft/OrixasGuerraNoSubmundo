using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class BotoesIndioma : MonoBehaviour {

	private Text meuText;
	public string portugues;
	public string ingles;

	// Use this for initialization
	void Start () {
		meuText = GetComponentInChildren<Text> ();

		if (meuText == null) {
			meuText = GetComponent<Text> ();
		}

		MudarIndioma ();
	}

	public void MudarIndioma () {
		if (PlayerPrefs.GetInt ("Indioma") == 1) {
			meuText.text = portugues;
		} else {
			meuText.text = ingles;
		}
	}
}
