using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Conversas : MonoBehaviour {
	
	public bool limparTexto;

	public float tempoVidaTexto;
	public float maxTempoVidaTexto;

	// Use this for initialization
	void Start () {
		GetComponent<Text> ().text = "";

		limparTexto = false;

		tempoVidaTexto = 0;
		maxTempoVidaTexto = 0;
	}
	
	// Update is called once per frame
	void Update () {
		//Tira a mensagem após seu tempo de vida acabar.
		if (limparTexto) {
			tempoVidaTexto += Time.deltaTime;
			if (tempoVidaTexto >= maxTempoVidaTexto) {
				GetComponent<Text> ().text = "";
				limparTexto = false;
			}
		}
	}

	/// <summary>
	/// Este método serve para passar o tempo que o texto vai demorar para ser tirado da tela.
	/// </summary>
	/// <param name="menssagem">Menssagem.</param>
	/// <param name="maxTempoMudarTexto">Max tempo mudar texto.</param>
	/// <param name="maxTempoVidaTexto">Max tempo vida texto.</param>
	/// <param name="esperarAcabar">If set to <c>true</c> esperar acabar.</param>
	public void MudarTempoLimparTexto (float maxTempoVidaTexto) {
		this.maxTempoVidaTexto = maxTempoVidaTexto;

		tempoVidaTexto = 0;
		limparTexto = true;
	}
}
