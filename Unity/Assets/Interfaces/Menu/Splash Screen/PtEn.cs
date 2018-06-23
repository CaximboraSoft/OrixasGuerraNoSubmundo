using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class PtEn : MonoBehaviour {

	public Text Pt;
	public Text En;

	// Use this for initialization
	void Start () {
		MudarIndioma ();
	}
	
	public void MudarIndioma () {
		if (PlayerPrefs.GetInt ("Indioma") == 1) {
			Pt.enabled = true;
			En.enabled = false;
		} else {
			Pt.enabled = false;
			En.enabled = true;
		}
	}
}
