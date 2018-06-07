using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadosMovimentacao : MonoBehaviour {

	public Vector3 velocidade;
	private Animator meuAnimator;
	private Rigidbody meuRigidbody;
	public RuntimeAnimatorController animacaoOriginal;

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
		/*
		 if (EstouNoChao) {
			
		} else {
			meuAnimator.SetFloat ("Velocidade", 0);
		}*/
	}

	public AnimatorStateInfo PegarLayerDoMeuAnimator (int indice) {
		return meuAnimator.GetCurrentAnimatorStateInfo (indice);
	}

	public void MudarFloatDoMeuAnimator (string nome, float valor) {
		meuAnimator.SetFloat (nome, valor);
	}

	public void MudarBoolDoMeuAnimator (string nome, bool valor) {
		meuAnimator.SetBool (nome, valor);
	}

	public bool PegarBoolDoMeuAnimator (string nome) {
		return meuAnimator.GetBool (nome);
	}

	public void ChamarTriggerDoMeuAnimator (string nome) {
		meuAnimator.SetTrigger (nome);
	}

	public Rigidbody PegarMeuRigidbody () {
		return meuRigidbody;
	}

	public void ColocarAnimacaoOriginal () {
		meuAnimator.runtimeAnimatorController = animacaoOriginal as RuntimeAnimatorController;
	}
}
