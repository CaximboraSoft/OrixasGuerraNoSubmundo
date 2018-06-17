﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Lula : MonoBehaviour {

	public Transform rostoMostrar;
	public Transform lugarDoAbejide;
	public GameObject espada;
	public InimigosNormais[] carcereiros;
	public Transform abejideSeta;
	private Abejide abejide;
	public Renderer abejideEspada;
	private Animator abejideAnimator;

	public Conversas boca;

	private bool carcereirosMorreram = false;
	private bool morto = false;

	private int ato = 0;

	private string fala;

	private float temporizador;

	// Use this for initialization
	void Awake () {
		abejide = FindObjectOfType<Abejide> ();
		abejideAnimator = abejide.GetComponentInChildren<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		carcereirosMorreram = CarcereirosMorreram ();

		if (morto) {
			temporizador += Time.deltaTime;
			if (temporizador > 3.5f) {
				GetComponent<Animator> ().enabled = false;
				GetComponent<Lula> ().enabled = false;
			}
		}
		// && ato > 2 && !morto
		if (Input.GetKeyDown (KeyCode.Return)) {
			if (espada != null) {
				Destroy (espada);
			}
				
			if (!abejideEspada.enabled) {
				Destroy (espada);
				temporizador = 0f;
				GetComponent<Animator> ().SetTrigger ("TirarArma");

				abejideAnimator.SetInteger ("IndiceGatilho", 0);
				abejideAnimator.SetTrigger ("Gatilho");
				abejideEspada.enabled = true;
			}

			AcabouCutscene ();
		}

		switch  (ato) {
		case 0:
			if (carcereirosMorreram) {
				DesativarAbejide ();

				abejide.enabled = false;
				boca.enabled = true;
				boca.GetComponent<Conversas> ().MudarRostoMostrar (rostoMostrar);
				fala = "Psiu! Psiu...";
				boca.GetComponent<Conversas> ().nome.text = "Akin:";
				boca.GetComponent<Text> ().text = fala;
				boca.GetComponent<Conversas> ().MudarTempoLimparTexto (1.5f);
				boca.GetComponent<Conversas> ().conversas.enabled = true;
				temporizador = 0;
				ato++;
			}
			break;
		case 1:
			temporizador += Time.deltaTime;
			if (temporizador > 1.5f) {
				abejide.AtivarCodigoLula ();
				ato++;
			}
			break;
		case 2:
			if (Vector3.Distance(abejide.transform.position, transform.position) < 5f) {
				DesativarAbejide ();
				ato++;
			}
			break;
		case 3:
			abejideAnimator.SetFloat ("Velocidade", 2f);
			abejide.transform.position = Vector3.Lerp (abejide.transform.position, lugarDoAbejide.position, 1 * Time.deltaTime);
			abejideSeta.LookAt (new Vector3 (lugarDoAbejide.position.x, abejideSeta.position.y, lugarDoAbejide.position.z));
			abejide.transform.rotation = Quaternion.Slerp (abejide.transform.rotation, abejideSeta.rotation, 3f * Time.deltaTime);

			if (Vector3.Distance(abejide.transform.position, lugarDoAbejide.position) < 1f) {
				abejideAnimator.SetFloat ("Velocidade", 0);
				ato ++;
			}
			break;
		case 4:
			abejideSeta.LookAt (new Vector3 (transform.position.x, abejideSeta.position.y, transform.position.z));
			abejide.transform.rotation = Quaternion.Slerp (abejide.transform.rotation, abejideSeta.rotation, 3f * Time.deltaTime);

			if (Mathf.Abs(abejide.transform.eulerAngles.y - abejideSeta.eulerAngles.y) < 5) {
				ato++;

			}
			break;

		case 5:
			fala = "Se me ajudar.. Te ajudo a sair daqui. Vi seu olhar... conheço-o muito bem.";
			boca.GetComponent<Text> ().text = fala;
			boca.GetComponent<Conversas> ().MudarTempoLimparTexto (4f);
			boca.GetComponent<Conversas> ().conversas.enabled = true;
			temporizador = 0f;
			ato++;
			break;
		case 6:
			temporizador += Time.deltaTime;
			if (temporizador > 4f) {
				fala = "De quebra você ainda consegue uma arma boa, hein que tal?";
				boca.GetComponent<Text> ().text = fala;
				boca.GetComponent<Conversas> ().MudarTempoLimparTexto (4f);
				boca.GetComponent<Conversas> ().conversas.enabled = true;
				temporizador = 0f;
				ato++;
			}
			break;
		case 7:
			temporizador += Time.deltaTime;
			if (temporizador > 5f) {
				Destroy (espada);
				temporizador = 0f;
				GetComponent<Animator> ().SetTrigger ("TirarArma");

				abejideAnimator.SetInteger ("IndiceGatilho", 0);
				abejideAnimator.SetTrigger ("Gatilho");
				abejideEspada.enabled = true;
				ato++;
			}
			break;
		case 8:
			temporizador += Time.deltaTime;
			if (temporizador > 1f) {
				fala = "Brigado, filho...";
				boca.GetComponent<Text> ().text = fala;
				boca.GetComponent<Conversas> ().MudarTempoLimparTexto (2f);
				boca.GetComponent<Conversas> ().conversas.enabled = true;
				temporizador = 0f;
				ato++;
			}
			break;
		case 9:
			temporizador += Time.deltaTime;
			if (temporizador > 2f) {
				AcabouCutscene ();
			}
			break;
		}
	}

	private void AcabouCutscene () {
		Destroy (abejide.GetComponentInChildren<AbejideMao> ().gameObject);

		abejideAnimator.SetLayerWeight (abejide.armaAtual + 2, 0f);
		abejide.armaAtual = 0;
		abejideAnimator.SetLayerWeight (abejide.armaAtual + 2, 1f);
		abejide.AtivarCodigoLula ();
		boca.enabled = false;
		boca.GetComponent<Conversas> ().conversas.enabled = false;
		temporizador = 0;
		morto = true;
		abejideEspada.GetComponent<Arma> ().enabled = true;
		ato = -999;
	}

	private void DesativarAbejide () {
		abejide.enabled = false;
		abejideAnimator.SetBool ("Atacando", false);
		abejideAnimator.SetBool ("Correndo", false);
		abejideAnimator.SetBool ("FocandoInimigo", false);
		abejideAnimator.SetFloat ("Velocidade", 0f);
	}

	private bool CarcereirosMorreram () {
		for (int i = 0; i < carcereiros.Length; i++) {
			if (carcereiros[i].enabled) {
				return false;
			}
		}

		return true;
	}
}