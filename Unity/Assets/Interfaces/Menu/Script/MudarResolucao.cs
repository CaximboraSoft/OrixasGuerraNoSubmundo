using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class MudarResolucao : MonoBehaviour {

	public Dropdown qualidadeDropDown;
	public Dropdown resolucoesDropdown;
	public Toggle telaCheiaToggle;
	private bool adicionouConfiguracoes;

	// Use this for initialization
	void Start () {
		adicionouConfiguracoes = false;

		//Adiciona as resoluções no botão
		Resolution[] resolucoes = Screen.resolutions;
		for (int i = 0; i < resolucoes.Length; i++) {
			resolucoesDropdown.options [i].text = resolucoes[i].width + "x" + resolucoes[i].height;
			resolucoesDropdown.options.Add (new Dropdown.OptionData (resolucoesDropdown.options [i].text));

			if (Screen.currentResolution.height == resolucoes[i].height &&
				Screen.currentResolution.width == resolucoes[i].width) {
				resolucoesDropdown.value = i;
			}
		}

		//Carreca para o Toggle se o jogo esta em tela cheia ou não
		if (Data.telaCheia == 1) {
			telaCheiaToggle.isOn = true;
		} else {
			telaCheiaToggle.isOn = false;
		}

		adicionouConfiguracoes = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void MudarResolucaoDaTela () {
		if (adicionouConfiguracoes && resolucoesDropdown.value != PlayerPrefs.GetInt ("Resolucao")) {
			Debug.Log (resolucoesDropdown.value);
			int indice = resolucoesDropdown.value;
			Screen.SetResolution (Screen.resolutions [indice].width, Screen.resolutions [indice].height, telaCheiaToggle.isOn);

			Data.resolucao = indice;
			PlayerPrefs.SetInt ("Resolucao", Data.resolucao);
		}
	}

	public void MudarTelaCheia() {
		if (adicionouConfiguracoes) {
			int indice = resolucoesDropdown.value;
			Screen.SetResolution (Screen.resolutions [indice].width, Screen.resolutions [indice].height, telaCheiaToggle.isOn);

			if (telaCheiaToggle.isOn) {
				Data.telaCheia = 1;
			} else {
				Data.telaCheia = 0;
			}

			PlayerPrefs.SetInt ("TelaCheia", Data.telaCheia);
		}
	}
}
