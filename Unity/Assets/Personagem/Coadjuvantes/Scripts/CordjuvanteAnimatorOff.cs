using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CordjuvanteAnimatorOff : MonoBehaviour {

	private Animator[] meuAnimator;
	private Transform abejide;

	private float distancia;

	// Use this for initialization
	void Start () {
		meuAnimator = GetComponentsInChildren<Animator> ();

		abejide = GameObject.FindGameObjectWithTag ("Abejide").transform;
	}
	
	// Update is called once per frame
	void Update () {
		distancia = Vector3.Distance (transform.position, abejide.position);

		if (!meuAnimator[0].enabled) {
			if (distancia < 20f) {
				for (int i = 0; i < meuAnimator.Length; i++) {
					meuAnimator[i].enabled = true;
				}
			}
		} else if (meuAnimator[0].enabled && distancia > 20f) {
			for (int i = 0; i < meuAnimator.Length; i++) {
				meuAnimator[i].enabled = false;
			}
		}
	}
}
