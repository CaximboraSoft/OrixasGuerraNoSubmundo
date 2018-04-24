using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Conversas : MonoBehaviour {

	public Canvas conversas;
	private Transform rostoMostrar;
	public Camera cameraRosto;
	public Text nome;

	public bool limparTexto;

	public float tempoVidaTexto;
	public float maxTempoVidaTexto;

	// Use this for initialization
	void Start () {
		limparTexto = false;

		tempoVidaTexto = 0;
		maxTempoVidaTexto = 0;

		cameraRosto.enabled = true;
		conversas.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (conversas.enabled == true) {
			cameraRosto.transform.position = rostoMostrar.position;
			cameraRosto.transform.eulerAngles = rostoMostrar.eulerAngles;
		}
		//Tira a mensagem após seu tempo de vida acabar.
		if (limparTexto) {
			tempoVidaTexto += Time.deltaTime;
			if (tempoVidaTexto >= maxTempoVidaTexto) {
				conversas.enabled = false;
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

	public void MudarRostoMostrar (Transform rostoMostrar) {
		this.rostoMostrar = rostoMostrar;
	}
}
