using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour {

	public static int portugues = 1;
	public static int telaCheia = 1;
	public static int resolucao;
	public static int qualidade;
	private GameObject[] datas;

	void Awake () {
		datas = GameObject.FindGameObjectsWithTag ("Data");

		if (datas.Length >= 2) {
			Destroy (datas [0]);
		}

		DontDestroyOnLoad (transform.gameObject);
	}

	void Start () {
		//Banco de dados do indioma
		if (PlayerPrefs.HasKey ("Indioma")) {
			portugues = PlayerPrefs.GetInt ("Indioma");
		} else {
			PlayerPrefs.SetInt ("Indioma", portugues);
		}

		//Banco de dados da tela cheia
		if (PlayerPrefs.HasKey ("TelaCheia")) {
			telaCheia = PlayerPrefs.GetInt ("TelaCheia");
		} else {
			PlayerPrefs.SetInt ("TelaCheia", telaCheia);
		}

		//Banco de dados da resolucao
		if (PlayerPrefs.HasKey ("Resolucao")) {
			resolucao = PlayerPrefs.GetInt ("Resolucao");
		} else {
			//Pega o indice da resolução que estiver ativa
			for (int i = 0; i < Screen.resolutions.Length; i++) {
				if (Screen.currentResolution.height == Screen.resolutions[i].height &&
					Screen.currentResolution.width == Screen.resolutions[i].width) {
					PlayerPrefs.SetInt ("Resolucao", i);
					break;
				}
			}
		}

		//Banco de dados da qualidade
		if (PlayerPrefs.HasKey ("Qualidade")) {
			qualidade = PlayerPrefs.GetInt ("Qualidade");
		} else {
			//Pega o indice da resolução que estiver ativa
			for (int i = 0; i < QualitySettings.names.Length; i++) {
				if (QualitySettings.GetQualityLevel () == i) {
					PlayerPrefs.SetInt ("Qualidade", i);
					break;
				}
			}
		}
	}
}
