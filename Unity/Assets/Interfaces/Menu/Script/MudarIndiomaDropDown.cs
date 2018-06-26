using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class MudarIndiomaDropDown : MonoBehaviour {

	Dropdown meuDropdown;
	public Text label;

	private bool adicionouConfiguracoes;

	public string[] pt;
	public string[] en;

	// Use this for initialization
	void Start () {
		adicionouConfiguracoes = false;

		meuDropdown = GetComponent<Dropdown> ();

		for (int i = 0; i < pt.Length - 1; i++) {
			meuDropdown.options.Add (new Dropdown.OptionData (""));

			if (QualitySettings.GetQualityLevel () == i) {
				GetComponent<Dropdown> ().value = i;
			}
		}

		MudarIndioma ();

		adicionouConfiguracoes = true;
	}

	public void MudarIndioma () {
		if (PlayerPrefs.GetInt ("Indioma") == 1) {
			for (int i = 0; i < en.Length; i++) {
				meuDropdown.options [i].text = pt [i];

				if (QualitySettings.GetQualityLevel () == i) {
					label.text = pt [i];
				}
			}
		} else {
			for (int i = 0; i < en.Length; i++) {
				meuDropdown.options [i].text = en [i];

				if (QualitySettings.GetQualityLevel () == i) {
					label.text = en [i];
				}
			}
		}
	}

	public void MudarQualidade () {
		if (adicionouConfiguracoes && PlayerPrefs.GetInt ("Qualidade") != meuDropdown.value) {
			QualitySettings.SetQualityLevel(meuDropdown.value);

			Data.qualidade = meuDropdown.value;

			PlayerPrefs.SetInt ("Qualidade", Data.qualidade);
		}
	}
}
