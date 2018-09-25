using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pausar : MonoBehaviour {

	public Canvas pausa;
	public Canvas trapaca;
	public Canvas opcoes;
	public Canvas fimDeJogo;
	private bool jogoPausado = false;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape) && !trapaca.enabled && !fimDeJogo.enabled) {
			if (opcoes.enabled) {
				opcoes.GetComponentInChildren <CanvasGroup> ().interactable = false;
				opcoes.enabled = false;
				pausa.enabled = true;
			} else {
				jogoPausado = !jogoPausado;
				pausa.enabled = jogoPausado;

				if (jogoPausado) {
					pausa.gameObject.SetActive (true);
					Time.timeScale = 0;
				} else {
					DespausarJogo ();
				}
			}

			pausa.GetComponent <CanvasGroup> ().interactable = jogoPausado;
		}
	}

	public void DespausarJogo () {
		jogoPausado = false;
		pausa.gameObject.SetActive (false);
		Time.timeScale = 1;

		pausa.GetComponent <CanvasGroup> ().interactable = jogoPausado;
	}
}
