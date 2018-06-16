using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbejideMao : MonoBehaviour {

	public float dano = 0f;
	public int indiceDoLayer = 0;
	public Animator donoAnimator;
	private AnimatorStateInfo donoStateInfo;

	public int nomeDoAtaque = 0;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		donoStateInfo = donoAnimator.GetCurrentAnimatorStateInfo (indiceDoLayer);

		if (nomeDoAtaque != 0 && !donoStateInfo.IsTag("Atacando")) {
			nomeDoAtaque = 0;
		}
	}

	void OnTriggerStay (Collider other) {
		if (donoStateInfo.fullPathHash != nomeDoAtaque && donoStateInfo.normalizedTime > donoStateInfo.length / 2f &&
			donoStateInfo.IsTag ("Atacando") && other.transform.tag == "Inimigo") {

			nomeDoAtaque = donoStateInfo.fullPathHash;
			other.GetComponent<DadosMovimentacao> ().PerderVida (dano);
		}
	}
}
