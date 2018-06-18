﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadosDaFase : MonoBehaviour {
	
	public Transform[] atores;
	private Transform abejide;

	public Light[] luz;
	public ParticleSystem[] fogo;

	public float distanciaAcenderTocha = 10f;
	public float inimigosNormais = 0;
	public float inimigosNormaisAtacando = 0;

	public int sat;

	// Use this for initialization
	void Start () {
		abejide = GameObject.FindGameObjectWithTag ("Abejide").transform;

		PegarTochas ();
	}
	
	// Update is called once per frame
	void Update () {
		LigarConformeDistancia ();
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
}
