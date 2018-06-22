using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InimigosNormais : MonoBehaviour {

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
	public Renderer escudo;
	public Renderer espada;
	public Renderer lanca;
	private AudioSource meuAudioSource;

	public AudioClip[] danoSons;
	public AudioClip bloqueio;

	public bool escudoAtivo = false;
	public bool espadaAtiva = false;

	private bool sorteouPosicaoDeredor = false;
	public bool rodandoCutscene = false;
	public bool viuAbejide = false;

	private float distancia;
	public float distanciaEnxergar = 20f;
	public float velocidade;
	public float distanciaDeAtaque = 1.8f;

	private float tempoAtacar = 0f;
	private float tempoAtacarRandom = 0f;
	public float minTempoAtacar = 2f;
	public float maxTempoAtacar = 4f;

	public float minDistanciaDeredor = 2f;
	public float maxDistanciaDeredor = 9f;

	//Conrola a IA
	public float changeEstatoAtacando = 50f; //Chance de quando for mudar de Estato esta vai cair
	public float changeEstatoParado = 50f;
	public float changeEstatoDefendendo = 0f;
	private float tempoMudarEstato = 0f; //Contador que vai de zero até o número sorteado para mudar de estato
	private float tempoMudarEstatoRandom; //Guarda o número sorteado para mudar de Estato
	//Variáveis que guarda o tempo que vai mudar de Estato
	public float minMudarEstatoParado = 10f;
	public float maxMudarEstatoParado = 20f;
	public float minMudarEstatoAtacando = 10f;
	public float maxMudarEstatoAtacando = 25f;
	public float muitoProximo = 0f;
	public float maxMuitoProximo = 0f;
	public float muitoDistante = 0f;
	public float maxMuitoDistante = 4f;

	public int nivelDaIa = 0;
	/* Estatos:
	 * 0: Atacando
	 * 1: Andando por perto do abejide
	 * 2: Defendendo, vale somente para inimigos com IA nivel 2
	 */
	public int estato = 0;

	void Awake () {
		dadosDaFase = FindObjectOfType <DadosDaFase> ();

		if (nivelDaIa == 2) {
			if (Random.Range (0, 2) == 0) { //Escudo
				escudo.enabled = true;
				escudoAtivo = true;
			} else {
				escudo.enabled = false;
				escudoAtivo = false;
			} 

			if (Random.Range (0, 2) == 0) { //Espada
				espadaAtiva = true;
				espada.enabled = true;
				lanca.enabled = false;
			} else { //Lanca
				espada.enabled = false;
				lanca.enabled = true;
			}
		}

		if (nivelDaIa == 2 && !espadaAtiva) {
			distanciaDeAtaque = 2.5f;
			GetComponentInChildren<Arma> ().transform.localScale = new Vector3 (1f, 7.5f, 1f);
		}
	}

	public void AtivarCodigo () {
		abejide = GameObject.FindGameObjectWithTag ("Abejide").transform;
		meuNavMeshAgent = GetComponent<NavMeshAgent> ();
		meuDadosMovimentacao = GetComponentInChildren<DadosMovimentacao> ();
		meuAnimator = GetComponentInChildren <Animator> ();

		tempoAtacarRandom = Random.Range (2f, maxTempoAtacar);
		meuNavMeshAgent.stoppingDistance = distanciaDeAtaque;
		GetComponent<InimigosNormais> ().enabled = true;
		meuAudioSource = GetComponent<AudioSource> ();

		SortearEstato ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!rodandoCutscene && !abejide.GetComponent<Abejide> ().GameOver ()) {
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
						Debug.DrawLine (seta.position, posicaoDeredor, Color.blue);

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
				//Inimigo usando escudo para defender, tem o mesmo sistema do estato 1, porém com o diferencial de ele espre olhar para o abejide
				case 2:
					if (sorteouPosicaoDeredor && !meuAnimator.GetBool ("AndandoParaTras")) {
						Debug.DrawLine (seta.position, posicaoDeredor, Color.blue);

						meuNavMeshAgent.SetDestination (posicaoDeredor);
						OlharParaVector3 (abejide.position, meuDadosMovimentacao.velocidadeRotacao * 2f);

						if (Vector3.Distance (abejide.position, posicaoDeredor) > maxDistanciaDeredor) {
							sorteouPosicaoDeredor = false;
						}
						//Mas caso ainda não tenha sido sorteado uma posição valida, o inimigo segue o Abejide
					} else {
						SeguirAbejide ();
					}
					break;
				}

				velocidade = meuNavMeshAgent.velocity.magnitude;
			}
		

			if (nivelDaIa != 0 && distancia < distanciaDeAtaque - 0.5f) {
				muitoDistante = 0f;
				muitoProximo += Time.deltaTime;
				GetComponent<Rigidbody> ().AddForce (transform.forward * -2000f);

				if (nivelDaIa == 2) {
					if (estato == 1) {
						estato = 0;
					}

					meuAnimator.SetBool ("AndandoParaTras", true);
				}
			} else {
				muitoProximo = 0f;

				if (nivelDaIa == 2 && meuAnimator.GetBool ("AndandoParaTras")) {
					meuAnimator.SetBool ("AndandoParaTras", false);
				} else if (distancia > distanciaDeAtaque + 0.5f && estato == 0) {
					muitoDistante += Time.deltaTime;

					if (muitoDistante > maxMuitoDistante) {
						muitoDistante = 0f;
						SortearEstato ();
					}
				}
			}

			if (muitoProximo > maxMuitoProximo && estato != 0) {
				if (nivelDaIa == 2) {
					tempoAtacar = 999;
					estato = 0;
					meuAnimator.SetBool ("Defendendo", false);
				}
			}

			//Só chama a função de ataque caso o inimigo esteja dentro do alcance de sua arma
			if (distancia < distanciaDeAtaque + 0.5f && estato == 0) {
				Atacar ();
			} else if (tempoAtacar != 0f) {
				//Caso não esteja dentro do ancance, é feito uma outra coisa de acordo com o nivel da IA
				switch (nivelDaIa) {
				//IA de nivel 0 é somente zerado o tempo de para atacar
				case 0:
					tempoAtacar = 0f;
					break;

				//IA de nivel 1 é sorteado um outro tempo de ataque porem com um tempo de ataque, porém com um maximo tempo um pouco menor
				case 1:
					tempoAtacar = 0f;
					tempoAtacarRandom = Random.Range (minTempoAtacar, maxTempoAtacar / 1.5f);
					break;
				}
			}

			if (!sorteouPosicaoDeredor && estato != 0) {
				SortearPosicaoDeredor ();
			}

			meuAnimator.SetFloat ("Velocidade", velocidade);
		} else {
			meuAnimator.SetFloat ("Velocidade", 0);
			meuNavMeshAgent.SetDestination (transform.position);
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

			if (!espadaAtiva) {
				meuAnimator.SetInteger ("IndiceAtaque", Random.Range (2, 4));
			} else {
				meuAnimator.SetInteger ("IndiceAtaque", Random.Range (0, 3));
			}
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
		float chanceRandomDefendendo = 0f;
		if (escudoAtivo) {
			chanceRandomDefendendo = Random.Range (0f, changeEstatoDefendendo);
		} else if (nivelDaIa == 2) {
			chanceRandomParado += Random.Range (0f, changeEstatoDefendendo);
		}

		switch (nivelDaIa) {
		case 0:
			if (dadosDaFase.inimigosIa0Atacando >= 4) {
				chanceRandomParado = 100f;
			}
			break;
		case 1:
			if (dadosDaFase.inimigosIa1Atacando >= 4) {
				chanceRandomParado = 100f;
			}
			break;
		case 2:
			if (dadosDaFase.inimigosIa2Atacando >= 4) {
				chanceRandomParado = Random.Range (0f, 50f);
				chanceRandomDefendendo = Random.Range (0f, 50f);
			}
			break;
		}

		if (nivelDaIa == 2) {
			meuAnimator.SetBool ("Defendendo", false);
		}
		//Estato atacando
		if (chanceRandomAtaque >= chanceRandomParado && chanceRandomAtaque >= chanceRandomDefendendo) {
			estato = 0;
			tempoAtacar = 0f;
			tempoMudarEstatoRandom = Random.Range (minMudarEstatoAtacando, maxMudarEstatoAtacando);

		//Estato parado perdo do Abejide
		} else if (chanceRandomParado >= chanceRandomAtaque && chanceRandomParado >= chanceRandomDefendendo) {
			estato = 1;
			sorteouPosicaoDeredor = false;
			tempoMudarEstatoRandom = Random.Range (minMudarEstatoParado, maxMudarEstatoParado);
		} else if (chanceRandomDefendendo > chanceRandomAtaque && chanceRandomDefendendo > chanceRandomParado) {
			estato = 2;
			meuAnimator.SetBool ("Defendendo", true);
			sorteouPosicaoDeredor = false;
			tempoMudarEstatoRandom = Random.Range (minMudarEstatoParado, maxMudarEstatoParado);
		}
	}

	/// <summary>
	/// Faz o inimigo seguir o Abejide
	/// </summary>
	private void SeguirAbejide () {
		meuNavMeshAgent.SetDestination (abejide.position);

		meuStateInfo = meuAnimator.GetCurrentAnimatorStateInfo (1);
		if (meuStateInfo.IsTag ("Atacando")) {
			if (nivelDaIa == 2) {
				OlharParaVector3 (abejide.position, meuDadosMovimentacao.velocidadeRotacao);
			}
		} else {
			OlharParaVector3 (abejide.position, meuDadosMovimentacao.velocidadeRotacao);
		}
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
			Debug.DrawLine (seta.position, meuRaycastHit.point, Color.blue);
		//Se não tiver nada o inimigo vai até a posição sorteada
		} else {
			posicaoDeredor = seta.position + seta.forward * distanciaSorteada;
			sorteouPosicaoDeredor = true;
			Debug.DrawLine (seta.position, seta.position + seta.forward * distanciaSorteada, Color.blue);
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

		switch (nivelDaIa) {
		//Caso esteja com IA nivel 0, irá somente sortear um novo estato
		case 0:
			SortearEstato ();
			break;
		//Caso estaj com IA nivel 1, o inimigo irá entar automaticamente no modo ataque
		case 1:
			estato = 0;
			tempoMudarEstatoRandom = Random.Range (minMudarEstatoAtacando, maxMudarEstatoAtacando);
			tempoAtacar = 0f;
			tempoAtacarRandom = Random.Range (minTempoAtacar, maxTempoAtacar);
			break;
		case 2:
			estato = 0;
			if (tempoAtacar > 0) {
				tempoAtacar -= 0.25f;
			}
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
		Instantiate (Resources.Load ("Prefab/Morte", typeof(GameObject)), transform.position, Quaternion.identity);

		bool droparVida = false;

		//Verifica quantos inimigos com esse mesmo nivel de IA esta atacando, mas dependendo do nivel de IA tembem é contabilizado os outros
		switch (nivelDaIa) {
		case 0:
			dadosDaFase.pontuacao += 100;
			if (Random.Range (0f, 100f) < 5) {
				droparVida = true;
			}
			break;
		case 1:
			dadosDaFase.pontuacao += 200;
			if (Random.Range (0f, 100f) < 25) {
				droparVida = true;
			}
			break;
		case 2:
			dadosDaFase.pontuacao += 300;
			if (Random.Range (0f, 100f) < 50) {
				droparVida = true;
			}
			break;
		}

		if (droparVida) {
			Vector3 rotacao = new Vector3 (Random.Range (0f, 360f), Random.Range (0f, 360f), Random.Range (0f, 360f));
			Instantiate (Resources.Load ("Prefab/Vida", typeof(GameObject)), transform.position, Quaternion.Euler (rotacao));
		}

		transform.tag = "Untagged";
		Destroy (meuNavMeshAgent);
		Destroy (seta.gameObject);
		StartCoroutine ("DesroirSom");
		Destroy (GetComponentInChildren<Arma> ().gameObject);
		GetComponent<InimigosNormais> ().enabled = false;
	}

	IEnumerator DesroirSom () {
		yield return new WaitForSeconds (2);
		Destroy (meuAudioSource);
		Destroy (GetComponent<InimigosNormais> ());
	}
}
