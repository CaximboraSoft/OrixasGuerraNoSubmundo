using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lixo : MonoBehaviour {

	public Animator corpo;
	public Animator pe;

	private AnimatorStateInfo corpoState;

	private float timerState;

	// Use this for initialization
	void Start () {
		corpoState = corpo.GetCurrentAnimatorStateInfo (0);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Space) && corpo.GetBool("Parado")) {
			timerState = 0;

			corpo.SetBool ("Parado", false);
			corpo.SetBool ("In", false);
			corpo.SetBool ("Out", false);

			pe.SetBool ("Parado", false);
			pe.SetBool ("In", false);
			pe.SetBool ("Out", false);
		} else if (!Input.GetKey (KeyCode.Space) && !corpo.GetBool("Parado")) {
			corpo.SetBool ("Parado", true);
			pe.SetBool ("Parado", true);
		}

		if (!corpo.GetBool("Parado")) {
			SincronizarAndando ();
		}
	}

	private void SincronizarAndando () {
		timerState += Time.deltaTime;

		if (timerState > corpoState.length / 3) {
			timerState = 0;

			if (corpo.GetBool("In")) {
				corpo.SetBool ("In", false);
				corpo.SetBool ("Out", true);

				pe.SetBool ("In", false);
				pe.SetBool ("Out", true);
			} else {
				corpo.SetBool ("In", true);
				corpo.SetBool ("Out", false);

				pe.SetBool ("In", true);
				pe.SetBool ("Out", false);
			}
		}
	}
}
