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
	private Vector3 meuRotacao;

	public bool rodandoCutscene = false;
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
		if (!rodandoCutscene) { 
			distancia = Vector3.Distance (transform.position, abejide.position);

			seta.LookAt (abejide.transform.position);
			if (!viuAbejide && distancia < distanciaAtacar) {
				Physics.Raycast (seta.position, seta.forward, out meuRaycastHit);

				if (meuRaycastHit.transform != null && meuRaycastHit.transform == abejide) {
					viuAbejide = true;
				}
			}

			if (viuAbejide) {
				meuNavMeshAgent.SetDestination (abejide.position);
				meuRotacao.Set (transform.eulerAngles.x, seta.eulerAngles.y, seta.eulerAngles.z);
				transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (meuRotacao), meuDadosMovimentacao.velocidadeRotacao * Time.deltaTime);
				velocidade = meuNavMeshAgent.velocity.magnitude;
				Debug.DrawLine (seta.position, meuRaycastHit.point, Color.red);
			}
		}

		meuAnimator.SetFloat ("Velocidade", velocidade);
	}

	/// <summary>
	/// Para o inimigo caso o jogador entre em uma cutscene.
	/// </summary>
	public void PausarCutscene () {
		rodandoCutscene = true;
		meuNavMeshAgent.SetDestination (transform.position);
		velocidade = 0f;
	}

	/// <summary>
	/// Desativa todas as coisas deste que o objeto pode esta ativado, tipo o <NavMeshAgent>;
	/// </summary>
	public void DesativarCodigo () {
		transform.tag = "Untagged";
		Destroy (meuNavMeshAgent);
		Destroy (GetComponent<InimigosNormais> ());
		//meuNavMeshAgent.enabled = false;
		//GetComponent<InimigosNormais> ().enabled = false;
	}
}
