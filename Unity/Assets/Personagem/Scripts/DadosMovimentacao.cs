using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadosMovimentacao : MonoBehaviour {

	private Animator meuAnimator;
	private Rigidbody meuRigidbody;

	public float vida = 100f;
	public float velocidadeAndando;
	public float velocidadeCorrendo;
	public float velocidadePulando;
	public float velocidadeRotacao;

	// Use this for initialization
	void Start () {
		meuAnimator = GetComponentInChildren<Animator> ();
		meuRigidbody = GetComponent<Rigidbody> ();
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
		if (vida > 0) {
			vida -= dano;
			GetComponent<InimigosNormais> ().PerdeuVida ();

			if (vida <= 1) {
				meuAnimator.SetLayerWeight (1, 0f);
				meuAnimator.SetBool ("Morto", true);
				Destroy (meuRigidbody);
				Destroy (GetComponent<Collider> ());
				GetComponent<InimigosNormais> ().DesativarCodigo ();
			}

			meuAnimator.SetTrigger ("PerderVida");
		}
	}
}
