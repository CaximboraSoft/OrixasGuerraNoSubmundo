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

	private float temporizador;

	// Use this for initialization
	void Start () {
		meuAnimator = GetComponentInChildren<Animator> ();
		meuRigidbody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (vida < 1) {
			AnimatorStateInfo meuStateInfo = meuAnimator.GetCurrentAnimatorStateInfo (0);

			if (meuStateInfo.IsTag ("MORTO")) {
				meuAnimator.enabled = false;
				GetComponent<DadosMovimentacao> ().enabled = false;
			}
		}
	}

	public Rigidbody PegarMeuRigidbody () {
		return meuRigidbody;
	}

	public void PerderVida (float dano) {
		if (vida > 0) {
			vida -= dano;

			if (vida < 1) {
				meuAnimator.SetBool ("Morto", true);
				GetComponent<Rigidbody> ().useGravity = false;
				GetComponent<Collider> ().enabled = false;
				GetComponent<InimigosNormais> ().DesativarCodigo ();
				temporizador = 0f;
			}
			meuAnimator.SetTrigger ("PerderVida");
		}
	}
}
