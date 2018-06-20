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

	public AudioClip[] danoSoco;

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
			donoStateInfo.IsTag ("Atacando")) {

			if (other.transform.tag == "Inimigo" && donoAnimator.name == "AbejideBlender") {
				nomeDoAtaque = donoStateInfo.fullPathHash;
				other.GetComponent<DadosMovimentacao> ().PerderVida (dano);
				if (dano == 10f) {
					GetComponent<AudioSource> ().clip = danoSoco [Random.Range (0, danoSoco.Length)];
					GetComponent<AudioSource> ().Play ();
				}
			} else if (other.transform.tag == "Abejide") {
				nomeDoAtaque = donoStateInfo.fullPathHash;
				other.GetComponent<Abejide> ().PerderVida ();
			} else if (other.transform.tag == "Coadjuvantes" && donoAnimator.name == "AbejideBlender") {
				FindObjectOfType <DadosDaFase> ().pontuacao += 50;

				other.GetComponent<Animator> ().SetInteger ("Ato", -1);
				other.GetComponent<Animator> ().SetTrigger ("MudarAto");
				other.tag = "Untagged";
			}
		}
	}
}
