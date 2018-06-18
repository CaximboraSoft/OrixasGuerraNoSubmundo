using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arma : MonoBehaviour {
	
	public float dano = 0f;
	//0 nenhum elemento | 1 água | 2 fogo | 3 terra
	public int elemento = 0;
	public int indiceDoLayer = 0;
	public Animator donoAnimator;
	private AnimatorStateInfo donoStateInfo;

	private int nomeDoAtaque;

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
		if (donoStateInfo.fullPathHash != nomeDoAtaque && donoStateInfo.normalizedTime > donoStateInfo.length / 1.5f &&
			donoStateInfo.IsTag ("Atacando") && other.transform.tag == "Inimigo") {

			nomeDoAtaque = donoStateInfo.fullPathHash;
			other.GetComponent<DadosMovimentacao> ().PerderVida (dano);
		}
	}
}
