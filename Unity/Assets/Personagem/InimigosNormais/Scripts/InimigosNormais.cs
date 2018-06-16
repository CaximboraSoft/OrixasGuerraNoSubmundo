using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InimigosNormais : MonoBehaviour {

	public Transform seta;
	private Transform abejide;
	private NavMeshAgent meuNavMeshAgent;
	private DadosMovimentacao meuDadosMovimentacao;
	private RaycastHit meuRaycastHit;
	private Animator meuAnimator;

	private bool viuAbejide = false;

	public float distancia;
	public float distanciaAtacar = 20f;
	public float velocidade;

	public void AtivarCodigo () {
		abejide = GameObject.FindGameObjectWithTag ("Abejide").transform;
		meuNavMeshAgent = GetComponent<NavMeshAgent> ();
		meuDadosMovimentacao = GetComponentInChildren<DadosMovimentacao> ();
		meuAnimator = GetComponentInChildren <Animator> ();

		GetComponent<InimigosNormais> ().enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		distancia = Vector3.Distance (transform.position, abejide.position);

		if (!viuAbejide && distancia < distanciaAtacar) {
			if (Physics.Linecast (transform.position, abejide.position, out meuRaycastHit)) {
				viuAbejide = true;
			}
		}

		if (viuAbejide) {
			meuNavMeshAgent.SetDestination (abejide.position);
			seta.LookAt (new Vector3(abejide.transform.position.x, seta.position.y, abejide.transform.position.z));
			transform.rotation = Quaternion.Slerp (transform.rotation, seta.rotation, meuDadosMovimentacao.velocidadeRotacao * Time.deltaTime);
			velocidade = meuNavMeshAgent.velocity.magnitude;

			meuAnimator.SetFloat ("Velocidade", velocidade);
		}
	}

	/// <summary>
	/// Desativa todas as coisas deste que o objeto pode esta ativado, tipo o <NavMeshAgent>;
	/// </summary>
	public void DesativarCodigo () {
		meuNavMeshAgent.enabled = false;
		GetComponent<InimigosNormais> ().enabled = false;
	}
}
