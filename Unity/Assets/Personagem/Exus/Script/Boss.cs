using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class Boss : MonoBehaviour {

	public GameObject[] inimigo;
	public GameObject spawsBolasDeFogo;
	public GameObject bolaDeFogo;
	public Transform mao;
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
	public bool viuAbejide = false;
	private bool ataqueBolaDeFogo = false;

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
	public float changeEstatoTeleportar = 50f;
	public float tempoMudarEstato = 0f; //Contador que vai de zero até o número sorteado para mudar de estato
	public float tempoMudarEstatoRandom; //Guarda o número sorteado para mudar de Estato
	//Variáveis que guarda o tempo que vai mudar de Estato
	public float minMudarEstatoParado = 10f;
	public float maxMudarEstatoParado = 20f;
	public float minMudarEstatoAtacando = 10f;
	public float maxMudarEstatoAtacando = 25f;
	public float minMudarEstatoTeleporte = 10f;
	public float maxMudarEstatoTeleporte = 25f;
	public float muitoProximo = 0f;
	public float maxMuitoProximo = 0f;
	public float muitoDistante = 0f;
	public float maxMuitoDistante = 4f;
	public float magiaTemporizador;
	private float temporizador;

	private float tempoSpawInimigo = 0f;
	private float tempoSpawInimigoRandom = 28f;

	private int contadorBola = 0;
	private int maxContadorBola = 60;
	public int estato = 0;

	public void AtivarCodigo () {
		abejide = GameObject.FindGameObjectWithTag ("Abejide").transform;
		meuNavMeshAgent = GetComponent<NavMeshAgent> ();
		meuNavMeshAgent.enabled = true;
		meuDadosMovimentacao = GetComponentInChildren<DadosMovimentacao> ();
		meuAnimator = GetComponentInChildren <Animator> ();

		tempoAtacarRandom = Random.Range (2f, maxTempoAtacar);
		meuNavMeshAgent.stoppingDistance = distanciaDeAtaque;
		meuAudioSource = GetComponent<AudioSource> ();

		dadosDaFase = FindObjectOfType <DadosDaFase> ();

		GetComponent<Boss> ().enabled = true;

		tempoSpawInimigoRandom = Random.Range (20f, 30f);
		SortearEstato ();
	}

	// Update is called once per frame
	void Update () {

		tempoSpawInimigo += Time.deltaTime;
		if (tempoSpawInimigo > tempoSpawInimigoRandom) {
			tempoSpawInimigoRandom = Random.Range (20f, 30f);
			tempoSpawInimigo = 0;
			sorteouPosicaoDeredor = false;
			estato = 4;
		}

		temporizador += Time.deltaTime;
		if (ataqueBolaDeFogo && temporizador > 0.2f) {
			meuAnimator.SetInteger ("IndiceAtaque", 4);
			meuAnimator.SetTrigger ("Atacar");

			contadorBola++;

			Instantiate (bolaDeFogo, mao.position, mao.rotation);
			if (contadorBola > maxContadorBola) {
				ataqueBolaDeFogo = false;
			}
		}

		if (!abejide.GetComponent<Abejide> ().GameOver ()) { 
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
				tempoMudarEstato += Time.deltaTime;
				if (tempoMudarEstato > tempoMudarEstatoRandom) {
					SortearEstato ();
				}

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
				//Teleportar
				case 2:
					if (sorteouPosicaoDeredor) {
						meuNavMeshAgent.SetDestination (posicaoDeredor);
						transform.position = posicaoDeredor;

						estato = 3;
					} else {
						SeguirAbejide ();
					}
					break;	
				case 3:
					OlharParaVector3 (abejide.position, meuDadosMovimentacao.velocidadeRotacao);
					break;
				case 4:
					if (sorteouPosicaoDeredor) {
						GameObject inimigoCriado = Instantiate (inimigo[inimigo.Length - 1], posicaoDeredor, Quaternion.identity);
						inimigoCriado.GetComponent<InimigosNormais> ().viuAbejide = true;
						inimigoCriado.GetComponent<InimigosNormais> ().chanceDroparVidaExu = 75f;
						inimigoCriado.GetComponent<InimigosNormais> ().AtivarCodigo ();
						SortearEstato ();
					}
					break;
				}
				
				velocidade = meuNavMeshAgent.velocity.magnitude;

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
						muitoDistante += Time.deltaTime;

						if (muitoDistante > maxMuitoDistante && estato == 0) {
							muitoDistante = 0f;

							if (Random.Range (0f, 100f) < 35f) {
								JogarBolaDeFogo ();
							} else {
								muitoDistante = 0f;
								SortearEstato ();
							}
						}
					}
				}

				if (muitoProximo > maxMuitoProximo && estato != 0) {
					SortearEstato ();
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
	}

	/// <summary>
	/// Faz chover bola de fogo.
	/// </summary>
	public void JogarBolaDeFogo () {
		GameObject temp = Instantiate (spawsBolasDeFogo);
		temp.GetComponent<SpawBolasDeFogo> ().tipoDeRotinaBolaDeFogo = 0;
		temp.GetComponent<SpawBolasDeFogo> ().maxBoloDeFogo = Random.Range (10, 20);

		sorteouPosicaoDeredor = false;
		tempoMudarEstatoRandom = Random.Range (minMudarEstatoTeleporte, maxMudarEstatoTeleporte);
		estato = 2;
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

		float chanceRandomAtaque = Random.Range (0f, changeEstatoAtacando);
		float chanceRandomParado = Random.Range (0f, changeEstatoParado);
		float chanceRandomTeleportar = Random.Range (0f, changeEstatoTeleportar);

		//Estato atacando
		if (chanceRandomAtaque >= chanceRandomParado && chanceRandomAtaque > chanceRandomTeleportar) {
			estato = 0;
			tempoAtacar = 0f;
			tempoMudarEstatoRandom = Random.Range (minMudarEstatoAtacando, maxMudarEstatoAtacando);

			if (Random.Range (0f, 100f) < 40f) {
				JogarBolaDeFogo ();
			}
			//Estato parado perdo do Abejide
		} else if (chanceRandomParado >= chanceRandomAtaque && chanceRandomParado > chanceRandomTeleportar) {
			estato = 1;
			sorteouPosicaoDeredor = false;
			tempoMudarEstatoRandom = Random.Range (minMudarEstatoParado, maxMudarEstatoParado);

			if (Random.Range (0f, 100f) < 35f) {
				JogarBolaDeFogo ();
			} else if (Random.Range (0f, 100f) < 35f) {
				temporizador = -1f;
				ataqueBolaDeFogo = true;
				contadorBola = 0;
				maxContadorBola = Random.Range (50, 70);
			}
		} else {
			sorteouPosicaoDeredor = false;
			tempoMudarEstatoRandom = Random.Range (minMudarEstatoTeleporte, maxMudarEstatoTeleporte);
			estato = 2;

			if (Random.Range (0f, 100f) < 35f) {
				temporizador = -1f;
				ataqueBolaDeFogo = true;
				contadorBola = 0;
				maxContadorBola = Random.Range (50, 70);
			}
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
			meuAudioSource.clip = danoSons [Random.Range (0, danoSons.Length - 1)];
			meuAudioSource.Play ();

			if (Random.Range (0f, 100f) < 45f) {
				sorteouPosicaoDeredor = false;
				tempoMudarEstatoRandom = Random.Range (minMudarEstatoTeleporte, maxMudarEstatoTeleporte);
				estato = 2;
			}
		}
	}

	/// <summary>
	/// Desativa todas as coisas deste que o objeto pode esta ativado, tipo o <NavMeshAgent>;
	/// </summary>
	public void DesativarCodigo () {
		dadosDaFase.pontuacao += 10000;

		Vector3 rotacao = new Vector3 (Random.Range (0f, 360f), Random.Range (0f, 360f), Random.Range (0f, 360f));
		Instantiate (Resources.Load ("Prefab/Vida", typeof(GameObject)), transform.position, Quaternion.Euler (rotacao));

		for (int i = 0; i < 3; i++) {
			if (Random.Range (0f, 100f) < 70f) {
				rotacao = new Vector3 (Random.Range (0f, 360f), Random.Range (0f, 360f), Random.Range (0f, 360f));
				Instantiate (Resources.Load ("Prefab/Vida", typeof(GameObject)), transform.position, Quaternion.Euler (rotacao));
			}
		}
		
		transform.tag = "Untagged";
		Destroy (meuNavMeshAgent);
		Destroy (seta.gameObject);
		StartCoroutine ("DesroirSom");
		Destroy (GetComponentInChildren<Arma> ().gameObject);
		GetComponent<Boss> ().enabled = false;
	}

	IEnumerator DesroirSom () {
		yield return new WaitForSeconds (2);
		Destroy (meuAudioSource);
		Destroy (GetComponent<Boss> ());
	}
}
