using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coadjuvantes : MonoBehaviour {

	private Animator meuAnimator;
	private Transform abejide;

	public bool codigoIniciaAtivo = true;

	private float distancia;

	public int ato;

	// Use this for initialization
	void Start () {
		meuAnimator = GetComponent<Animator> ();
		meuAnimator.SetInteger ("Ato", ato);
		meuAnimator.SetTrigger ("MudarAto");

		abejide = GameObject.FindGameObjectWithTag ("Abejide").transform;

		GetComponent<Coadjuvantes> ().enabled = codigoIniciaAtivo;
		if (ato == 3 || ato == 4) {
			GetComponent<Coadjuvantes> ().enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		distancia = Vector3.Distance (transform.position, abejide.position);

		if (ato == 5 && distancia < 20f) {
			transform.Translate (0f, 0f, 2f * Time.deltaTime);
			transform.Rotate (0f, -60f * Time.deltaTime, 0f);
		}
		if (!meuAnimator.enabled) {
			if (distancia < 20f) {
				meuAnimator.enabled = true;
			}
		} else if (meuAnimator.enabled && distancia > 20f) {
			meuAnimator.enabled = false;
		}
	}
}
