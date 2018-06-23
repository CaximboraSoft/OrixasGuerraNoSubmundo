using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class DadosDaFase : MonoBehaviour {
	
	public Transform[] atores;
	private Transform abejide;

	public Light[] luz;
	public ParticleSystem[] fogo;

	public bool portugues;

	public int inimigosIa0Atacando; //Usado para os carcereiros
	public int inimigosIa1Atacando; //Usados para as succubus
	public int inimigosIa2Atacando; //Usados para as esqueletos

	public Text pontuacaoText;
	public int pontuacao;
	public int sat;

	// Use this for initialization
	void Start () {
		abejide = GameObject.FindGameObjectWithTag ("Abejide").transform;

		PegarTochas ();

		if (PlayerPrefs.GetInt ("Indioma") == 1) {
			portugues = true;
		} else {
			portugues = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		pontuacaoText.text = pontuacao.ToString ();

		ChecarInimigosAtacando ();

		LigarConformeDistancia ();
	}

	void ChecarInimigosAtacando () {
		int inimigosIa0AtacandoTemp = 0;
		int inimigosIa1AtacandoTemp = 0;
		int inimigosIa2AtacandoTemp = 0;
		InimigosNormais[] inimigosNormais = FindObjectsOfType <InimigosNormais> ();

		for (int i = 0; i < inimigosNormais.Length; i++) {
			if (inimigosNormais[i] != null && inimigosNormais[i].viuAbejide) {
				if (inimigosNormais[i].estato == 0) {
					inimigosIa0AtacandoTemp++;

					if (inimigosNormais[i].nivelDaIa == 1) {
						inimigosIa1AtacandoTemp++;
					} if (inimigosNormais[i].nivelDaIa == 2) {
						inimigosIa2AtacandoTemp++;
					}
				}
			}
		}

		if (inimigosIa0Atacando != inimigosIa0AtacandoTemp) {
			inimigosIa0Atacando = inimigosIa0AtacandoTemp;
		} if (inimigosIa1Atacando != inimigosIa1AtacandoTemp) {
			inimigosIa1Atacando = inimigosIa1AtacandoTemp;
		} if (inimigosIa2Atacando != inimigosIa2AtacandoTemp) {
			inimigosIa2Atacando = inimigosIa2AtacandoTemp;
		}
	}

	void LigarConformeDistancia () {
		for (int i = 0; i < luz.Length; i++) {
			if (luz [i] != null) {
				float distancia = Vector3.Distance (luz [i].transform.position, abejide.position);

				if (!luz [i].enabled) {
					if (distancia < 35f) {
						luz [i].enabled = true;
						fogo [i].Play ();
					}
				} else if (luz [i].enabled && distancia > 35f) {
					luz [i].enabled = false;
					fogo [i].Stop ();
				}
			} else {
				PegarTochas ();
			}
		}
	}

	void PegarTochas () {
		GameObject[] objTemp = GameObject.FindGameObjectsWithTag ("Tocha");
		luz = new Light[objTemp.Length];
		fogo = new ParticleSystem[objTemp.Length];

		for (int i = 0; i < objTemp.Length; i++) {
			luz [i] = objTemp [i].GetComponentInChildren<Light> ();
			fogo [i] = objTemp [i].GetComponentInChildren<ParticleSystem> ();

			fogo[i].Play ();
			luz[i].enabled = true;
		}
	}

	public bool PegarIndioma () {
		return portugues;
	}
}
