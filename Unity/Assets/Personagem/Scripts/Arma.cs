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
	private Abejide abejide;

	public AudioClip[] danoSoco;

	private int nomeDoAtaque;

	// Use this for initialization
	void Awake () {
		abejide = FindObjectOfType<Abejide> ();
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

			if (donoAnimator.name == "AbejideBlender") {
				if (other.transform.tag == "Inimigo") {
					nomeDoAtaque = donoStateInfo.fullPathHash;

					abejide.mana.value += abejide.addMana;

					bool danoDaEspada = true;
					if (dano == 10f) {
						GetComponent<AudioSource> ().clip = danoSoco [Random.Range (0, danoSoco.Length)];
						GetComponent<AudioSource> ().Play ();
						danoDaEspada = false;
					}

					other.GetComponent<DadosMovimentacao> ().PerderVida (dano * abejide.estamina.value, danoDaEspada);
				} else if (other.transform.tag == "MiniBoss" && other.GetComponent<MiniBoss> ().enabled) {
					nomeDoAtaque = donoStateInfo.fullPathHash;
					other.GetComponent<DadosMovimentacao> ().PerderVida (dano * abejide.estamina.value, true);

					abejide.mana.value += abejide.addMana;

				} else if (other.transform.tag == "Coadjuvantes") {
					FindObjectOfType <DadosDaFase> ().pontuacao += 50;

					other.GetComponent<Animator> ().SetInteger ("Ato", -1);
					other.GetComponent<Animator> ().SetTrigger ("MudarAto");
					other.tag = "Untagged";

					abejide.mana.value += abejide.addMana * 8f;
				}
			} else if (other.transform.tag == "Abejide") {
				nomeDoAtaque = donoStateInfo.fullPathHash;
				other.GetComponent<Abejide> ().PerderVida ();
			}
		}
	}
}
