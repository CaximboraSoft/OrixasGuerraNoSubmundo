using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadosMovimentacao : MonoBehaviour {

	private Animator meuAnimator;
	private Rigidbody meuRigidbody;
	private InimigosNormais meuInimigosNormais;
	private MiniBoss meuMiniBoss;
	private Boss meuBoss;

	public float vida = 100f;
	public float velocidadeAndando;
	public float velocidadeCorrendo;
	public float velocidadePulando;
	public float velocidadeRotacao;

	// Use this for initialization
	void Awake () {
		meuAnimator = GetComponentInChildren<Animator> ();
		meuRigidbody = GetComponent<Rigidbody> ();
		meuInimigosNormais = GetComponent<InimigosNormais> ();
		meuMiniBoss = GetComponent<MiniBoss> ();
		meuBoss = GetComponent<Boss> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (vida <= 1) {
			AnimatorStateInfo meuStateInfo = meuAnimator.GetCurrentAnimatorStateInfo (0);

			if (meuStateInfo.IsTag ("MORTO")) {
				if (meuBoss != null) {
					GetComponent <SeteEncruzilhadasAto04> ().enabled = true;
				}
				meuAnimator.enabled = false;
				Destroy (meuAnimator);
				Destroy (GetComponent<DadosMovimentacao> ());
			}
		}
	}

	public Rigidbody PegarMeuRigidbody () {
		return meuRigidbody;
	}

	public void PerderVida (float dano, bool danoEspada) {
		if (vida > 0) {
			if (meuInimigosNormais != null && meuInimigosNormais.estato != 2) { //Estato 2 é bloqueando
				meuInimigosNormais.PerdeuVida (danoEspada);
				vida -= dano;
			} else if (meuMiniBoss != null) {
				meuMiniBoss.PerdeuVida (danoEspada);
				vida -= dano;
			} else if (meuBoss != null) {
				meuBoss.PerdeuVida (danoEspada);
				vida -= dano;
			}

			if (vida <= 1) {
				meuAnimator.SetLayerWeight (1, 0f);

				if (meuAnimator.layerCount > 2) {
					meuAnimator.SetLayerWeight (2, 0f);

					if (meuAnimator.layerCount > 3) {
						meuAnimator.SetLayerWeight (3, 0f);
					}
				}

				meuAnimator.SetBool ("Morto", true);
				Destroy (meuRigidbody);
				Destroy (GetComponent<Collider> ());
				if (meuInimigosNormais != null) {
					meuInimigosNormais.DesativarCodigo ();
				} else if (meuMiniBoss != null) {
					meuMiniBoss.DesativarCodigo ();
				} else if (meuBoss != null) {
					meuBoss.DesativarCodigo ();
				}
			}

			meuAnimator.SetTrigger ("PerderVida");
		}
	}
}
