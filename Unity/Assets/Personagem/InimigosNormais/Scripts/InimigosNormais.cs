using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InimigosNormais : MonoBehaviour {

	public Transform posicaoDeredorT;
	public Vector3 posicaoDeredor;
	public Transform seta;
	private Transform abejide;
	private NavMeshAgent meuNavMeshAgent;
	private DadosMovimentacao meuDadosMovimentacao;
	private RaycastHit meuRaycastHit;
	private Animator meuAnimator;
	private Vector3 meuRotacao;

	public bool sorteouPosicaoDeredor = false;
	private bool chegouNaPosicao = false;
	public bool rodandoCutscene = false;
	private bool viuAbejide = false;

	public float distancia;
	public float distanciaAtacar = 20f;
	public float velocidade;
	public float distanciaDeAtaque = 1.8f;

	public float tempoParaAtacar = 0f;
	public float tempoParaAtacarRandom;
	public float minTempoParaAtacar = 2f;
	public float maxTempoParaAtacar = 4f;

	public float minDistanciaDeredor = 2f;
	public float maxDistanciaDeredor = 9f;

	public int nivelDaIa = 0;
	/*
	 * 0: Atacando.
	 * 1: Andando por perto do abejide.
	 */
	public int estato = 0;

	public void AtivarCodigo () {
		abejide = GameObject.FindGameObjectWithTag ("Abejide").transform;
		meuNavMeshAgent = GetComponent<NavMeshAgent> ();
		meuDadosMovimentacao = GetComponentInChildren<DadosMovimentacao> ();
		meuAnimator = GetComponentInChildren <Animator> ();

		tempoParaAtacarRandom = Random.Range (2f, maxTempoParaAtacar);
		meuNavMeshAgent.stoppingDistance = distanciaDeAtaque;
		GetComponent<InimigosNormais> ().enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (!rodandoCutscene) { 
			distancia = Vector3.Distance (transform.position, abejide.position);

			if (!viuAbejide && distancia < distanciaAtacar) {
				seta.LookAt (abejide.transform.position);
				Physics.Raycast (seta.position, seta.forward, out meuRaycastHit);

				Debug.DrawLine (seta.position, meuRaycastHit.point, Color.red);
				if (meuRaycastHit.transform != null && meuRaycastHit.transform == abejide) {
					viuAbejide = true;
				}
			}

			if (viuAbejide) {
				switch (estato) {
				case 0:
					SeguirAbejide ();
					break;
				case 1:
					if (sorteouPosicaoDeredor) {
						if (!chegouNaPosicao) {
							meuNavMeshAgent.SetDestination (posicaoDeredor);
							OlharParaVector3 (posicaoDeredor, meuDadosMovimentacao.velocidadeRotacao * 2f);
							velocidade = 3;
						} else {
							OlharParaVector3 (abejide.position, meuDadosMovimentacao.velocidadeRotacao);
							velocidade = 0;
						}

						if (!chegouNaPosicao && Vector3.Distance (transform.position, posicaoDeredor) < distanciaAtacar + 0.5f) {
							chegouNaPosicao = true;
						}
					} else {
						SeguirAbejide ();
					}
					break;
				}
			}
		}


		if (distancia < distanciaDeAtaque + 0.5f && estato == 0) {
			Atacar ();
		} else if (tempoParaAtacar != 0f) {
			switch (nivelDaIa) {
			case 0:
				tempoParaAtacar = 0f;
				break;
			case 1:
				tempoParaAtacar = 0f;
				tempoParaAtacarRandom = Random.Range (minTempoParaAtacar, maxTempoParaAtacar);
				break;
			}
		}

		if (!sorteouPosicaoDeredor && estato == 1) {
			SortearPosicaoDeredor ();
		}

		meuAnimator.SetFloat ("Velocidade", velocidade);
	}

	private void SeguirAbejide () {
		meuNavMeshAgent.SetDestination (abejide.position);
		OlharParaVector3 (abejide.position, meuDadosMovimentacao.velocidadeRotacao);
		velocidade = meuNavMeshAgent.velocity.magnitude;
	}

	private void OlharParaVector3 (Vector3 foco, float velocidadeRotacao) {
		seta.LookAt (foco);
		meuRotacao.Set (transform.eulerAngles.x, seta.eulerAngles.y, seta.eulerAngles.z);
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (meuRotacao), velocidadeRotacao * Time.deltaTime);
	}

	private void Atacar () {
		tempoParaAtacar += Time.deltaTime;

		if (tempoParaAtacar > tempoParaAtacarRandom) {
			tempoParaAtacar = 0;
			tempoParaAtacarRandom = Random.Range (minTempoParaAtacar, maxTempoParaAtacar);

			meuAnimator.SetInteger ("IndiceAtaque", Random.Range (0, 3));
			meuAnimator.SetTrigger ("Atacar");
		}
	}

	private void SortearPosicaoDeredor () {
		Vector3 setaposicao = seta.position;

		seta.position = abejide.position;
		seta.eulerAngles = new Vector3 (0f, Random.Range (0f, 361f), 0f);

		float distanciaSorteada = Random.Range (minDistanciaDeredor, maxDistanciaDeredor);
		Physics.Raycast (seta.position, seta.TransformDirection (Vector3.forward), out meuRaycastHit, distanciaSorteada);

		if (meuRaycastHit.transform != null) {
			if (meuRaycastHit.distance > minDistanciaDeredor) {
				posicaoDeredorT.position = meuRaycastHit.point;
				posicaoDeredor = meuRaycastHit.point;
				sorteouPosicaoDeredor = true;
				chegouNaPosicao = false;
			}
			Debug.DrawLine (seta.position, meuRaycastHit.point, Color.blue);
		} else {
			posicaoDeredorT.position = seta.position + seta.forward * distanciaSorteada;
			posicaoDeredor = seta.position + seta.forward * distanciaSorteada;
			sorteouPosicaoDeredor = true;
			chegouNaPosicao = false;
			Debug.DrawLine (seta.position, seta.position + seta.forward * distanciaSorteada, Color.blue);
		}

		seta.position = setaposicao;
	}

	public void PerdeuVida () {
		switch (nivelDaIa) {
		case 0:
			tempoParaAtacar = 0f;
			break;
		case 1:
			tempoParaAtacar = 0f;
			tempoParaAtacarRandom = Random.Range (minTempoParaAtacar, maxTempoParaAtacar);
			break;
		}
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
