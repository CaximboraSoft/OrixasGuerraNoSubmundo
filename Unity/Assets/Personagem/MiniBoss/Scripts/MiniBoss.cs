using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class MiniBoss : MonoBehaviour {


	private Vector3 posicaoDeredor;
	public Transform seta;
	private Transform abejide;
	private NavMeshAgent meuNavMeshAgent;
	private DadosMovimentacao meuDadosMovimentacao;
	private RaycastHit meuRaycastHit;
	private Animator meuAnimator;
	private AnimatorStateInfo meuStateInfo;
	private Vector3 meuRotacao;
	private DadosDaFase dadosDaFase;
	private AudioSource meuAudioSource;

	public AudioClip[] danoSons;

	private bool sorteouPosicaoDeredor = false;
	public bool rodandoCutscene = false;
	public bool viuAbejide = false;

	private float distancia;
	public float distanciaEnxergar = 20f;
	public float velocidade;
	public float distanciaDeAtaque = 1.8f;

	private float tempoAtacar = 0f; //Temporizador usada para medior o tempo entre os ataque e para medir o tempo da investida
	private float tempoAtacarRandom = 0f;
	public float minTempoAtacar = 2f;
	public float maxTempoAtacar = 4f;

	public float minDistanciaDeredor = 2f;
	public float maxDistanciaDeredor = 9f;

	//Conrola a IA
	public float changeEstatoAtacando = 50f; //Chance de quando for mudar de Estato esta vai cair
	public float changeEstatoParado = 50f;
	public float changeEstatoInvestir = 50f;
	public float tempoMudarEstato = 0f; //Contador que vai de zero até o número sorteado para mudar de estato
	public float tempoMudarEstatoRandom; //Guarda o número sorteado para mudar de Estato
	//Variáveis que guarda o tempo que vai mudar de Estato
	public float minMudarEstatoParado = 10f;
	public float maxMudarEstatoParado = 20f;
	public float minMudarEstatoAtacando = 10f;
	public float maxMudarEstatoAtacando = 25f;
	public float muitoProximo = 0f;
	public float maxMuitoProximo = 0f;
	public float muitoDistante = 0f;
	public float maxMuitoDistante = 4f;
	public float minInvestindo = 4f;
	public float maxInvestindo = 7f;
	public float distanciaInvestida = 4f;

	public int estato = 0;

	public void Awake () {
		abejide = GameObject.FindGameObjectWithTag ("Abejide").transform;
		meuNavMeshAgent = GetComponent<NavMeshAgent> ();
		meuDadosMovimentacao = GetComponentInChildren<DadosMovimentacao> ();
		meuAnimator = GetComponentInChildren <Animator> ();

		tempoAtacarRandom = Random.Range (2f, maxTempoAtacar);
		meuNavMeshAgent.stoppingDistance = distanciaDeAtaque;
		meuAudioSource = GetComponent<AudioSource> ();

		dadosDaFase = FindObjectOfType <DadosDaFase> ();

		estato = 0;
		SortearEstato ();
	}

	// Update is called once per frame
	void Update () {
		if (!rodandoCutscene) { 
			distancia = Vector3.Distance (transform.position, abejide.position);

			if (!viuAbejide && distancia < distanciaEnxergar) {
				seta.LookAt (abejide.transform.position);
				Physics.Raycast (seta.position, seta.forward, out meuRaycastHit);

				Debug.DrawLine (seta.position, meuRaycastHit.point, Color.red);
				if (meuRaycastHit.transform != null && meuRaycastHit.transform == abejide) {
					viuAbejide = true;
				}
			}

			if (viuAbejide) {
				//Se o miniBoss estiver invextindo, ele não vai poder mudar de estato
				//if (estato != 2) {
					tempoMudarEstato += Time.deltaTime;
					if (tempoMudarEstato > tempoMudarEstatoRandom) {
						if (estato == 2) {
							meuAnimator.SetBool ("Investindo", false);
						}

						SortearEstato ();
					}
				//}

				//Verifica qual é o estato que o inimig esta
				switch (estato) {
				//Estato 0 o inimigo segue o Abejide
				case 0:
					SeguirAbejide ();
					break;

				//Estato 1 o inimigo vai para a posição que foi sorteada perto do Abejide
				case 1:
					if (sorteouPosicaoDeredor) {
						Debug.DrawLine (seta.position, posicaoDeredor, Color.black);

						meuNavMeshAgent.SetDestination (posicaoDeredor);
						if (velocidade != 0) {
							OlharParaVector3 (posicaoDeredor, meuDadosMovimentacao.velocidadeRotacao * 2f);
						} else {
							OlharParaVector3 (abejide.position, meuDadosMovimentacao.velocidadeRotacao);
						}

						if (Vector3.Distance (abejide.position, posicaoDeredor) > maxDistanciaDeredor) {
							sorteouPosicaoDeredor = false;
						}
						//Mas caso ainda não tenha sido sorteado uma posição valida, o inimigo segue o Abejide
					} else {
						SeguirAbejide ();
					}
					break;	

				case 2:
					OlharParaVector3 (abejide.position, 1.5f);
					GetComponent<Rigidbody> ().AddForce (transform.forward * 9000f);
					break;
				}

				velocidade = meuNavMeshAgent.velocity.magnitude;
			}

			if (distancia < distanciaDeAtaque - 0.5f && estato != 2) {
				muitoDistante = 0f;
				muitoProximo += Time.deltaTime;
				GetComponent<Rigidbody> ().AddForce (transform.forward * -2000f);

				if (estato != 0) {
					estato = 0;
				}

				meuAnimator.SetBool ("AndandoParaTras", true);
			} else {
				muitoProximo = 0f;

				if (meuAnimator.GetBool ("AndandoParaTras")) {
					meuAnimator.SetBool ("AndandoParaTras", false);
				} else if (distancia > distanciaDeAtaque + 0.5f && estato == 0) {
					if (!meuAnimator.GetBool ("Investindo")) {
						muitoDistante += Time.deltaTime;

						//Caso o jogador fique muito distance, o inimigo pode dar uma investida
						if (muitoDistante > maxMuitoDistante) {
							muitoDistante = 0f;

							if (distancia > distanciaInvestida && estato != 2 && Random.Range (0f, 100f) < changeEstatoInvestir) {
								estato = 2;
								tempoMudarEstato = 0;
								meuAnimator.SetBool ("Investindo", true);
								meuAnimator.SetTrigger ("Investir");
								tempoMudarEstatoRandom = Random.Range (minInvestindo, maxInvestindo);
							} else {
								if (Random.Range (0f, 100f) < 35f) {
									tempoAtacar = 999;
									Atacar ();
								} else {
									muitoDistante = 0f;
									SortearEstato ();
								}
							}
						}
					}
				}
			}

			if (muitoProximo > maxMuitoProximo && estato != 0) {
				tempoAtacar = 999;
				estato = 0;
				meuAnimator.SetBool ("Defendendo", false);
			}

			//Só chama a função de ataque caso o inimigo esteja dentro do alcance de sua arma
			if (distancia < distanciaDeAtaque + 0.5f && estato == 0) {
				Atacar ();
			}

			if (!sorteouPosicaoDeredor && estato != 0) {
				SortearPosicaoDeredor ();
			}

			meuAnimator.SetFloat ("Velocidade", velocidade);
		}
	}

	/// <summary>
	/// Faz o inimigo atacar após um determinado tempo de estera, depois desse tempo é sorteado outro
	/// </summary>
	private void Atacar () {
		tempoAtacar += Time.deltaTime;

		if (tempoAtacar > tempoAtacarRandom && !meuStateInfo.IsTag ("Atacando")) {
			tempoAtacar = 0;
			tempoAtacarRandom = Random.Range (minTempoAtacar, maxTempoAtacar);

			meuAnimator.SetInteger ("IndiceAtaque", Random.Range (0, 4));

			meuAnimator.SetTrigger ("Atacar");
		}
	}

	/// <summary>
	/// Sorteia um novo estato de acrodo com a porcentagem desse inimigo
	/// </summary>
	private void SortearEstato () {
		tempoMudarEstato = 0f;

		if (estato == 1) {
			float chanceRandomInvestir = Random.Range (0f, 100f);

			if (chanceRandomInvestir < changeEstatoInvestir && distancia > distanciaInvestida) {
				estato = 2;
				tempoMudarEstatoRandom = Random.Range (minInvestindo, maxInvestindo);
				return;
			}
		}

		float chanceRandomAtaque = Random.Range (0f, changeEstatoAtacando);
		float chanceRandomParado = Random.Range (0f, changeEstatoParado);

		//Estato atacando
		if (chanceRandomAtaque >= chanceRandomParado) {
			estato = 0;
			tempoAtacar = 0f;
			tempoMudarEstatoRandom = Random.Range (minMudarEstatoAtacando, maxMudarEstatoAtacando);

			//Estato parado perdo do Abejide
		} else if (chanceRandomParado >= chanceRandomAtaque) {
			estato = 1;
			sorteouPosicaoDeredor = false;
			tempoMudarEstatoRandom = Random.Range (minMudarEstatoParado, maxMudarEstatoParado);
		}
	}

	/// <summary>
	/// Faz o inimigo seguir o Abejide
	/// </summary>
	private void SeguirAbejide () {
		meuNavMeshAgent.SetDestination (abejide.position);

		OlharParaVector3 (abejide.position, meuDadosMovimentacao.velocidadeRotacao);
	}

	/// <summary>
	/// Faz o inimigo olhar para o vetor que foi passado, neste caso é uma seta que olha para a posição e o inimigo rotaciona até a rotação da seta
	/// com um determinado tempo
	/// </summary>
	/// <param name="foco">Foco.</param>
	/// <param name="velocidadeRotacao">Velocidade rotacao.</param>
	private void OlharParaVector3 (Vector3 foco, float velocidadeRotacao) {
		seta.LookAt (foco);
		meuRotacao.Set (transform.eulerAngles.x, seta.eulerAngles.y, seta.eulerAngles.z);
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (meuRotacao), velocidadeRotacao * Time.deltaTime);
	}

	/// <summary>
	/// Sorteia uma posição perto do Abejide para o inimigo andar até ela, essa posição tem uma determinada distancia minima e máxima, pois primeiro a seta
	/// é colocada na posião do abejide com um angulo em <y> aleatorio de 0 até 360, depois disso é sorteado uma distancia e a seta é transladada até essa
	/// distancia, se tiver não tiver nada neste local sorteado o inimigo vai até lá, mas caso teja tem que ver se a distancia desse algo é maior que
	/// a distancia minima aceitavel, de for o inimigo se move até esse algo, caso não seja é sorteado uma outra na próxima vez que do loop do <Update>.
	/// </summary>
	private void SortearPosicaoDeredor () {
		//Salva a posição que a seta está, pois ela vai ser colocada na posição do Abejide para fazer o sorteio
		Vector3 setaposicao = seta.position;

		seta.position = abejide.position;
		//Sorteia um angulo de 360 ao redor do abejide e uma doistancia para ver se tem algo por perto
		seta.eulerAngles = new Vector3 (0f, Random.Range (0f, 360f), 0f);
		float distanciaSorteada = Random.Range (minDistanciaDeredor, maxDistanciaDeredor);
		//Vê se tem algo na posição sorteada
		Physics.Raycast (seta.position, seta.TransformDirection (Vector3.forward), out meuRaycastHit, distanciaSorteada);

		//Se tiver algo e esse algo estiver com a distancia maior que a distancia minima que pode ser sorteada, o inimigo vai até ela
		if (meuRaycastHit.transform != null) {
			if (meuRaycastHit.distance > minDistanciaDeredor) {
				posicaoDeredor = meuRaycastHit.point;
				sorteouPosicaoDeredor = true;
			}
			Debug.DrawLine (seta.position, meuRaycastHit.point, Color.black);
			//Se não tiver nada o inimigo vai até a posição sorteada
		} else {
			posicaoDeredor = seta.position + seta.forward * distanciaSorteada;
			sorteouPosicaoDeredor = true;
			Debug.DrawLine (seta.position, seta.position + seta.forward * distanciaSorteada, Color.black);
		}

		//Coloca a seta na sua posição original
		seta.position = setaposicao;
	}

	/// <summary>
	/// Caso o inimigo perca vida, dependendo do nivel de IA que ele estiver, vai ter uma ação diferente
	/// </summary>
	public void PerdeuVida (bool danoEspada) {
		if (danoEspada) {
			meuAudioSource.clip = danoSons [Random.Range (0, danoSons.Length)];
			meuAudioSource.Play ();
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
		dadosDaFase.pontuacao += 1000;

		for (int i = 0; i < 5; i++) {
			if (Random.Range (0f, 100f) < 70f) {
				Vector3 rotacao = new Vector3 (Random.Range (0f, 360f), Random.Range (0f, 360f), Random.Range (0f, 360f));
				Instantiate (Resources.Load ("Prefab/Vida", typeof(GameObject)), transform.position, Quaternion.Euler (rotacao));
			}
		}

		transform.tag = "Untagged";
		Destroy (meuNavMeshAgent);
		Destroy (seta.gameObject);
		Destroy (GetComponentInChildren<Arma> ().gameObject);
		Destroy (meuAudioSource);
		Destroy (GetComponent<InimigosNormais> ());
		//meuNavMeshAgent.enabled = false;
		//GetComponent<InimigosNormais> ().enabled = false;
	}
}
