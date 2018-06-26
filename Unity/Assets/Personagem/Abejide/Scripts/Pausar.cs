using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pausar : MonoBehaviour {

	public Canvas pausa;
	public Canvas opcoes;

	private bool jogoPausado = false;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (opcoes.enabled) {
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
		}
	}

	public void DespausarJogo () {
		jogoPausado = false;
		pausa.gameObject.SetActive (false);
		Time.timeScale = 1;
	}
}
