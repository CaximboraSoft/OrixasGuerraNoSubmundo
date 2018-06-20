using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadosMovimentacao : MonoBehaviour {

	private Animator meuAnimator;
	private Rigidbody meuRigidbody;
	private InimigosNormais meuInimigosNormais;

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
	}
	
	// Update is called once per frame
	void Update () {
		if (vida <= 1) {
			AnimatorStateInfo meuStateInfo = meuAnimator.GetCurrentAnimatorStateInfo (0);

			if (meuStateInfo.IsTag ("MORTO")) {
				meuAnimator.enabled = false;
				Destroy (meuAnimator);
				Destroy (GetComponent<DadosMovimentacao> ());
			}
		}
	}

	public Rigidbody PegarMeuRigidbody () {
		return meuRigidbody;
	}

	public void PerderVida (float dano) {
		if (meuInimigosNormais.estato != 2) {
			if (vida > 0) {
				vida -= dano;
				if (dano == 10f) {
					meuInimigosNormais.PerdeuVida (false);
				} else {
					meuInimigosNormais.PerdeuVida (true);
				}


				if (vida <= 1) {
					meuAnimator.SetLayerWeight (1, 0f);

					if (meuAnimator.layerCount > 2) {
						meuAnimator.SetLayerWeight (2, 0f);
						meuAnimator.SetLayerWeight (3, 0f);
					}

					meuAnimator.SetBool ("Morto", true);
					Destroy (meuRigidbody);
					Destroy (GetComponent<Collider> ());
					meuInimigosNormais.DesativarCodigo ();
				}

				meuAnimator.SetTrigger ("PerderVida");
			}
		}
	}
}
