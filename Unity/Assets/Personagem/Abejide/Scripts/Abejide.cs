using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abejide : MonoBehaviour {

	public Vector3 vetorDeMovimento;
	private DadosMovimentacao meuDadosMovimentacao;
	private AnimatorStateInfo armaStateInfo;
	private AnimatorStateInfo peStateInfo;
	private AnimatorStateInfo esquivaStateInfo;
	public GameObject[] armas;
	public GameObject[] magias;
	public AbejideChao meuAbejideChao;
	private Vector3 angulo = Vector3.zero;
	private Animator meuAnimator;
	public Transform inimigo;
	public Transform seta;

	public bool focandoInimigo;
	public bool estaMovendo;
	private  bool esquivando;

	public float velocidadeEsquiva;
	private float velocidadeAtacando;
	private float tempoCombo;
	public float maxTempoCombo;
	public float maxTempoAtaqueAndando;
	private float tempoParaAndar;
	public float maxTempoParaAndar;
	public float distanciaDoChao;
	private float ultimaDistanciaDoChao;
	private float tempoCalcularDistancia;
	public float tempoParaAtacar; //Quarda o tempo que o jogodor tem que esperar par poder usar o ataque de andando

	private bool chegouNoAnguloDaEsquiva;
	private float anguloDaEsquiva;

	private int armaAtual;
	private int andarVertical = 0;
	private int andarHorizontal = 0;

	public Vector3 velocidade;

	private bool estouNoChao;
	private bool estaAtaqueAndando;
	public bool possoPular = true;

	// Executa mesmo se o código estiver desatiado
	public void AtivarCodigo () {
		meuDadosMovimentacao = GetComponent<DadosMovimentacao> ();
		meuAnimator = GetComponentInChildren<Animator> ();

		tempoParaAtacar = 0;
		tempoCalcularDistancia = 0;

		esquivando = false;
		estouNoChao = true;
		possoPular = true;
		estaMovendo = false;
		estaAtaqueAndando = false;

		armaAtual = 0;
		armas [armaAtual].GetComponent<Renderer> ().enabled = true;
		for (int linha = 1; linha < armas.Length; linha++) {
			armas [linha].GetComponent<Renderer> ().enabled = false;
		}

		GetComponent<Abejide> ().enabled = true;

		meuAbejideChao = GetComponentInChildren<AbejideChao> ();
		meuAbejideChao.GetComponent<Collider> ().enabled = true;
		meuAbejideChao.enabled = false;
	}

	// Update is called once per frame
	void Update () {
		SistemaDePulo ();

		velocidade = meuDadosMovimentacao.PegarMeuRigidbody().velocity;

		peStateInfo = meuAnimator.GetCurrentAnimatorStateInfo (0);
		if (armaAtual + 1 < meuAnimator.layerCount) {
			armaStateInfo =  meuAnimator.GetCurrentAnimatorStateInfo (armaAtual + 1);
		}
		esquivaStateInfo = meuAnimator.GetCurrentAnimatorStateInfo (2);

		if (!peStateInfo.IsTag("Recuperando") && !esquivaStateInfo.IsTag("Recuperando")) {
			if (!focandoInimigo) {
				MovimentacaoLivre ();
			} else {
				MovimentacaoOlhando ();
			}

			Esquiva ();

			if (estouNoChao) {
				if (!esquivando) {
					Atacar ();
				}
			}
		}

		meuAnimator.SetBool ("FocandoInimigo", focandoInimigo);
		meuAnimator.SetBool ("EstouNoChao", estouNoChao);
		meuAnimator.SetFloat ("VelocidadeAtacando", velocidadeAtacando);
		if (estouNoChao) {
			meuAnimator.SetFloat ("Velocidade", meuDadosMovimentacao.PegarMeuRigidbody ().velocity.magnitude);
		} else {
			meuAnimator.SetFloat ("Velocidade", 0);
		}

		if (Input.GetKeyDown (KeyCode.LeftAlt)) {
			focandoInimigo = !focandoInimigo;
		}
	}

	void SistemaDePulo () {
		//Faz o abejide pular
		if (Input.GetKeyDown (KeyCode.Space) && possoPular && !peStateInfo.IsTag("Atacando")) {
			estouNoChao = false;
			meuAnimator.SetBool ("Subindo", true);
			meuAnimator.SetTrigger ("SubirOuDescer");
			ultimaDistanciaDoChao = distanciaDoChao;
			meuDadosMovimentacao.PegarMeuRigidbody().AddForce (transform.up * meuDadosMovimentacao.velocidadePulando);
			possoPular = false;
		}

		RaycastHit pontoDeColisao;

		Physics.Raycast (meuAbejideChao.transform.position, meuAbejideChao.transform.TransformDirection(-Vector3.up), out pontoDeColisao, Mathf.Infinity);
		Debug.DrawLine (meuAbejideChao.transform.position, meuAbejideChao.transform.position - Vector3.up * pontoDeColisao.distance, Color.red);
		distanciaDoChao = pontoDeColisao.distance;

		//Troca a animação de caindo ou pulando dependendo da ocasiao
		tempoCalcularDistancia += Time.deltaTime;
		if (tempoCalcularDistancia > 0.05f) {
			tempoCalcularDistancia = 0;

			if (distanciaDoChao > 0.5f && !meuAbejideChao.enabled) {
				meuAbejideChao.bateuNoChao = false;
				meuAbejideChao.enabled = true;
			}

			//If que que se o abejide esta subindo
			//if (ultimaDistanciaDoChao < distanciaDoChao - 0.2f) {
			//	Debug.Log ("Subindo");
			if (!estaAtaqueAndando && ultimaDistanciaDoChao > distanciaDoChao + 0.2f) {
				if (meuAnimator.GetBool ("Subindo")) {
					meuAnimator.SetBool ("Subindo", false);
					meuAnimator.SetTrigger ("SubirOuDescer");
				} else if (estouNoChao) {
					meuAnimator.SetTrigger ("SubirOuDescer");
					meuAnimator.SetBool ("Subindo", false);
					estouNoChao = false;
					possoPular = false;
				}
			}

			if (!estouNoChao && (meuAbejideChao.enabled && meuAbejideChao.bateuNoChao)) {
				meuAbejideChao.enabled = false;
				possoPular = true;
				meuDadosMovimentacao.PegarMeuRigidbody().velocity = Vector3.zero;
				estouNoChao = true;

				//Só entra neste if se o jogador tiver usando o ataque do Abejide corredo
				if (meuAnimator.GetBool ("Atacando")) {
					tempoParaAtacar = -0.2f;
					estaAtaqueAndando = false;
					meuAnimator.SetBool ("Atacando", false);
				}
			}

			ultimaDistanciaDoChao = distanciaDoChao;
		}
	}

	void Esquiva () {
		if (Input.GetKeyDown (KeyCode.X) && !esquivando && estouNoChao && meuAnimator.GetFloat ("Velocidade") != 0f) {
			esquivando = true;
			chegouNoAnguloDaEsquiva = false;

			if (Input.GetKey (KeyCode.UpArrow)) {
				anguloDaEsquiva = 0;
			} else if (Input.GetKey(KeyCode.DownArrow)) {
				anguloDaEsquiva = 180;
			} else if (Input.GetKey (KeyCode.RightArrow)) {
				anguloDaEsquiva = 90;
			} else if (Input.GetKey (KeyCode.LeftArrow)) {
				anguloDaEsquiva = 270;
			} else{
				chegouNoAnguloDaEsquiva = true;
			}
		} else if (esquivaStateInfo.IsTag("Vazio") && chegouNoAnguloDaEsquiva) {
			esquivando = false;
		}

		if (esquivando) {
			float tempVelocidadeEsquiva = velocidadeEsquiva;

			if (!chegouNoAnguloDaEsquiva) {
				float tempoDeRotacao = meuDadosMovimentacao.velocidadeRotacao * 4f;
				transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (new Vector3 (0, anguloDaEsquiva, 0)), tempoDeRotacao * Time.deltaTime);
				tempVelocidadeEsquiva /= 2f;


			if (Mathf.Abs(anguloDaEsquiva - transform.eulerAngles.y) < 25 || (anguloDaEsquiva == 0 && transform.eulerAngles.y > 350)) {
					meuAnimator.SetTrigger ("Esquivar");
					chegouNoAnguloDaEsquiva = true;
				}
			}

			//Faz sempre o Abejide se mover para frente.
			meuDadosMovimentacao.PegarMeuRigidbody().AddForce (transform.forward * tempVelocidadeEsquiva, ForceMode.Force);
		}
	}

	void Atacar() {
		if (Input.GetKeyDown (KeyCode.Z)) {
			if (meuAnimator.GetBool ("Correndo")) {
				if (tempoParaAtacar > 0.4f) {
					meuDadosMovimentacao.PegarMeuRigidbody().AddForce (transform.up * (meuDadosMovimentacao.velocidadePulando / 2));
					meuAnimator.SetTrigger ("AtaqueAndando");
					estouNoChao = false;
					meuAnimator.SetBool ("Atacando", true);
					estaAtaqueAndando = true;
					possoPular = false;
					tempoParaAtacar = 0;
				}
			} else {
				if (!armaStateInfo.IsTag ("Atacando") || tempoCombo < 0.135f) {
					velocidadeAtacando = 2.3f;
				} else if (tempoCombo < 0.21f) {
					velocidadeAtacando = 2f;
				} else  if (tempoCombo < 0.31f) {
					velocidadeAtacando = 1.6f;
				} else {
					velocidadeAtacando = 1.2f;
				}

				tempoCombo = 0;
				meuAnimator.SetBool ("Atacando", true);
			}
		}
			
		if (!estaAtaqueAndando) {
			tempoCombo += Time.deltaTime;

			if (tempoCombo > maxTempoCombo && meuAnimator.GetBool ("Atacando")) {
				meuAnimator.SetBool ("Atacando", false);
			}
		}
	}

	void MovimentacaoLivre() {
		//Movimentacao do abejide, e tambem faz ele apontar para a "seta" que foi pressionada
		if (Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow)) {
			estaMovendo = true;

			angulo.Set (angulo.x, 0, angulo.z);
		}
		if (Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.UpArrow)) {
			estaMovendo = true;

			angulo.Set (angulo.x, 180, angulo.z);
		}
		if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow)) {
			if (estaMovendo) {
				angulo.Set (angulo.x, 45 + (angulo.y / 2), angulo.z);
			} else {
				estaMovendo = true;
				angulo.Set (angulo.x, 90, angulo.z);
			}
		}
		if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow	)) {
			if (estaMovendo) {
				angulo.Set (angulo.x, (45 + (angulo.y / 2)) * -1, angulo.z);
			} else {
				estaMovendo = true;
				angulo.Set (angulo.x, 270, angulo.z);
			}
		}

		if (estaMovendo) {
			estaMovendo = false;

			if (!esquivando) {
				float forcaDeMovimento = meuDadosMovimentacao.velocidadeAndando;

				//Corre.
				if (Input.GetKey (KeyCode.LeftShift)) {
					tempoParaAtacar += Time.deltaTime;

					meuAnimator.SetBool ("Correndo", true);
					forcaDeMovimento = meuDadosMovimentacao.velocidadeCorrendo;

				} else if (meuAnimator.GetBool ("Correndo")) {
					meuAnimator.SetBool ("Correndo", false);
				} else if (tempoParaAtacar != 0) {
					tempoParaAtacar = 0;
				}

				float tempoDeRotacao = meuDadosMovimentacao.velocidadeRotacao + meuDadosMovimentacao.PegarMeuRigidbody ().velocity.magnitude;
				if (!estouNoChao) {
					tempoDeRotacao /= 2;

					if (estaAtaqueAndando) {
						tempoDeRotacao /= 2;
					}
				}

				//Faz sempre o Abejide se mover para frente.
				meuDadosMovimentacao.PegarMeuRigidbody ().AddForce (transform.forward * forcaDeMovimento, ForceMode.Force);
				//Faz o Abejide apontar para um angulo segundo as setas que o jogador pode esta apertando.
				transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (angulo), tempoDeRotacao * Time.deltaTime);
			} else if (chegouNoAnguloDaEsquiva) {
				float tempoDeRotacao = meuDadosMovimentacao.velocidadeRotacao * 1.2f;
				transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (angulo), tempoDeRotacao * Time.deltaTime);
			}
		}
	}

	void MovimentacaoOlhando () {
		seta.LookAt (inimigo);

		//Faz o abejide olhar suavemente para o inimigo focado
		float tempoDeRotacao = meuDadosMovimentacao.velocidadeRotacao + meuDadosMovimentacao.PegarMeuRigidbody ().velocity.magnitude;
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3 (0, seta.eulerAngles.y, 0)), tempoDeRotacao * 1.5f * Time.deltaTime);


		//Sistema de movimentação
		vetorDeMovimento = Vector3.zero;
		if (Input.GetKey (KeyCode.UpArrow)) {
			vetorDeMovimento.Set (0, 0, 1);
		} if (Input.GetKey (KeyCode.DownArrow)) {
			vetorDeMovimento.Set (0, 0, vetorDeMovimento.z - 1);
		} if (Input.GetKey (KeyCode.RightArrow)) {
			vetorDeMovimento.Set (1, 0, vetorDeMovimento.z);
		} if (Input.GetKey (KeyCode.LeftArrow)) {
			vetorDeMovimento.Set (vetorDeMovimento.x - 1, 0, vetorDeMovimento.z);
		}

		vetorDeMovimento.Normalize ();
		meuDadosMovimentacao.PegarMeuRigidbody ().AddForce (vetorDeMovimento * (meuDadosMovimentacao.velocidadeAndando / 1.1f), ForceMode.Force);

		AnimacaoMovimentacaoOlhando ();
	}

	void AnimacaoMovimentacaoOlhando () {
		andarVertical = 0;
		andarHorizontal = 0;
		float meuAngulo = transform.eulerAngles.y;

		if (Input.GetKey (KeyCode.LeftArrow)) {
			if (meuAngulo > 45 && meuAngulo <= 135) {
				andarVertical = -1;
			} else if (meuAngulo > 135 && meuAngulo <= 225) {
				;andarHorizontal = 1;
			} else if (meuAngulo > 225 && meuAngulo < 315) {
				andarVertical = 1;
			} else {
				andarHorizontal = -1;
			}
		} 
		if (Input.GetKey (KeyCode.RightArrow)) {
			if (meuAngulo > 45 && meuAngulo <= 135) {
				andarVertical = 1;
			} else if (meuAngulo > 135 && meuAngulo <= 225) {
				andarHorizontal = -1;
			} else if (meuAngulo > 225 && meuAngulo < 315) {
				andarVertical = -1;
			} else {
				andarHorizontal = 1;
			}
		} if (Input.GetKey (KeyCode.UpArrow)) {
			if (meuAngulo > 45 && meuAngulo <= 135) {
				andarHorizontal = 1;
			} else if (meuAngulo > 135 && meuAngulo <= 225) {
				andarVertical = -1;
			} else if (meuAngulo > 225 && meuAngulo < 315) {
				andarHorizontal = 1;
			} else {
				andarVertical = 1;
			}
		} if (Input.GetKey (KeyCode.DownArrow)) {
			if (meuAngulo > 45 && meuAngulo <= 135) {
				andarHorizontal = -1;
			} else if (meuAngulo > 135 && meuAngulo <= 225) {
				andarVertical = 1;
			} else if (meuAngulo > 225 && meuAngulo < 315) {
				andarHorizontal = -1;
			} else {
				andarVertical = -1;
			}
		}

		meuAnimator.SetFloat ("AndarVertical", andarVertical);
		meuAnimator.SetFloat ("AndarHorizontal", andarHorizontal);
	}
}
