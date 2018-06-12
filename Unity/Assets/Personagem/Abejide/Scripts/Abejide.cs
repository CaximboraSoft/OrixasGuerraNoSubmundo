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
	public Transform inimigoParaFocar;
	private float distanciaDoInimigo = 0f;
	public Transform seta;
	public Transform satPosicaoFora;

	public bool focandoInimigoAnimator;
	private bool focandoInimigoLocal;
	public bool estaMovendo;
	public  bool esquivando;

	public float maxDistanciaOlhando = 8f;
	public float distanciaProcurarOutro = 2f;
	public float tempoBuscarInimigo = 1f;
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
	public float peDirecao = 0f;
	private float direcaoAngulo; //Usada no método <PeDirecao>, pois não se pode usar uma variavel local em uma função de la
	private float forcaDeMovimento;
	private float tempoParaPoderPular;

	private bool chegouNoAnguloDaEsquiva;
	private float anguloDaEsquiva;

	private int armaAtual = 0;
	private int andarVertical = 0;
	private int andarHorizontal = 0;

	private bool estouNoChao;
	private bool estaAtaqueAndando;
	public bool possoPular = true;

	void Start () {
		if (GetComponent<Abejide> ().enabled) {
			AtivarCodigo ();
		}
	}

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

		for (int linha = 0; linha < armas.Length; linha++) {
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

		peStateInfo = meuAnimator.GetCurrentAnimatorStateInfo (0);
		if (armaAtual + 1 < meuAnimator.layerCount - 1) {
			armaStateInfo =  meuAnimator.GetCurrentAnimatorStateInfo (armaAtual + 1);
		}
		esquivaStateInfo = meuAnimator.GetCurrentAnimatorStateInfo (meuAnimator.layerCount - 2);

		if (!peStateInfo.IsTag ("Recuperando") && !esquivaStateInfo.IsTag ("Recuperando")) {
			if (!focandoInimigoAnimator || esquivando) {
				MovimentacaoLivre ();
			}

			Esquiva ();

			if (estouNoChao) {
				if (!esquivando) {
					Atacar ();
				}
			}
		} else {
			forcaDeMovimento = 0f;
		}

		meuAnimator.SetFloat ("PeDirecao", peDirecao);
		meuAnimator.SetBool ("FocandoInimigo", focandoInimigoAnimator);
		meuAnimator.SetBool ("EstouNoChao", estouNoChao);
		meuAnimator.SetFloat ("VelocidadeAtacando", velocidadeAtacando);
		if (estouNoChao) {
			meuAnimator.SetFloat ("Velocidade", meuDadosMovimentacao.PegarMeuRigidbody ().velocity.magnitude);
		} else {
			meuAnimator.SetFloat ("Velocidade", 0);
		}

		if (Input.GetKeyDown (KeyCode.V)) {
			focandoInimigoLocal = !focandoInimigoLocal;
			focandoInimigoAnimator = focandoInimigoLocal;

			tempoBuscarInimigo = 1f;
		}


		if (focandoInimigoAnimator && inimigoParaFocar == null) {
			focandoInimigoAnimator = false;
		} else if (focandoInimigoLocal && inimigoParaFocar != null) {
			if (distanciaDoInimigo < maxDistanciaOlhando - 1f) {
				focandoInimigoAnimator = true;
			}
		}

		tempoBuscarInimigo += Time.deltaTime;
		if (focandoInimigoLocal && tempoBuscarInimigo > 0.5f) {
			if (inimigoParaFocar == null) {
				BuscarInimigoFocar ();
			} else if (Vector3.Distance (transform.position, inimigoParaFocar.position) > distanciaProcurarOutro) {
				BuscarInimigoFocar ();
			}
		}
	}

	void FixedUpdate () {
		if (!focandoInimigoAnimator || esquivando) {
			//Faz sempre o Abejide se mover para frente.
			meuDadosMovimentacao.PegarMeuRigidbody ().AddForce (transform.forward * forcaDeMovimento, ForceMode.Force);
		} else {
			MovimentacaoOlhando ();
		}

		if (Input.GetKey (KeyCode.H)) {
			meuDadosMovimentacao.PegarMeuRigidbody ().AddForce (transform.forward * forcaDeMovimento * 8f, ForceMode.Force);
		}
	}

	void SistemaDePulo () {
		//Faz o abejide pular
		if (Input.GetKeyDown (KeyCode.C) && possoPular && !armaStateInfo.IsTag("Atacando") && !esquivando) {
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
					tempoParaPoderPular = 0f;
				} else if (estouNoChao) {
					meuAnimator.SetTrigger ("SubirOuDescer");
					meuAnimator.SetBool ("Subindo", false);
					estouNoChao = false;
					possoPular = false;
				}
			}

			if (!estouNoChao && (meuAbejideChao.enabled && meuAbejideChao.bateuNoChao)) {
				meuAbejideChao.enabled = false;
				meuDadosMovimentacao.PegarMeuRigidbody().velocity = Vector3.zero;
				estouNoChao = true;

				//Só entra neste if se o jogador tiver usando o ataque do Abejide corredo
				if (meuAnimator.GetBool ("Atacando")) {
					tempoParaAtacar = -0.2f;
					estaAtaqueAndando = false;
					meuAnimator.SetBool ("Atacando", false);
					meuAnimator.SetBool ("Correndo", false);
				}
			}

			if (!possoPular && estouNoChao) {
				tempoParaPoderPular += Time.deltaTime;

				if (tempoParaPoderPular > 0.3f) {
					possoPular = true;
				}
			}
			ultimaDistanciaDoChao = distanciaDoChao;
		}
	}

	void Esquiva () {
		if (Input.GetKeyDown (KeyCode.Space) && !esquivando && estouNoChao && meuAnimator.GetFloat ("Velocidade") != 0f) {
			esquivando = true;
			chegouNoAnguloDaEsquiva = false;
			meuAnimator.SetBool ("Atacando", false);

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
			if (!chegouNoAnguloDaEsquiva) {
				float tempoDeRotacao = meuDadosMovimentacao.velocidadeRotacao * 4f;
				transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (new Vector3 (0, anguloDaEsquiva, 0)), tempoDeRotacao * Time.deltaTime);

			if (Mathf.Abs(anguloDaEsquiva - transform.eulerAngles.y) < 25 || (anguloDaEsquiva == 0 && transform.eulerAngles.y > 350)) {
					meuAnimator.SetTrigger ("Esquivar");
					chegouNoAnguloDaEsquiva = true;
				}
			}
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
				//Corre.
				if (Input.GetKey (KeyCode.LeftShift)) {
					tempoParaAtacar += Time.deltaTime;

					meuAnimator.SetBool ("Correndo", true);
					forcaDeMovimento = meuDadosMovimentacao.velocidadeCorrendo;
				} else {
					forcaDeMovimento = meuDadosMovimentacao.velocidadeAndando;

					if (meuAnimator.GetBool ("Correndo")) {
						meuAnimator.SetBool ("Correndo", false);
					} if (tempoParaAtacar != 0f) {
						tempoParaAtacar = 0;
					}
				}

				float tempoDeRotacao = meuDadosMovimentacao.velocidadeRotacao + meuDadosMovimentacao.PegarMeuRigidbody ().velocity.magnitude;
				if (!estouNoChao) {
					tempoDeRotacao /= 2;

					if (estaAtaqueAndando) {
						tempoDeRotacao /= 2;
					}
				}
				
				//Faz o Abejide apontar para um angulo segundo as setas que o jogador pode esta apertando.
				transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (angulo), tempoDeRotacao * Time.deltaTime);
			} else if (chegouNoAnguloDaEsquiva) {
				float tempoDeRotacao = meuDadosMovimentacao.velocidadeRotacao * 1.2f;
				transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (angulo), tempoDeRotacao * Time.deltaTime);

				forcaDeMovimento = velocidadeEsquiva;
			}
		} else {
			forcaDeMovimento = 0f;
		}
	}

	void MovimentacaoOlhando () {
		seta.LookAt (inimigoParaFocar);

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

		float divisor = 1.1f;
		if (vetorDeMovimento != Vector3.zero) {
			if (Input.GetKey (KeyCode.LeftShift)) {
				tempoParaAtacar += Time.deltaTime;

				divisor = 0.8f;
				meuAnimator.SetBool ("Correndo", true);
			} else if (tempoParaAtacar != 0 || meuAnimator.GetBool ("Correndo")) {
				tempoParaAtacar = 0;
				meuAnimator.SetBool ("Correndo", false);
			}
		} else if (meuAnimator.GetBool ("Correndo")) {
			meuAnimator.SetBool ("Correndo", false);
		}

		vetorDeMovimento.Normalize ();
		meuDadosMovimentacao.PegarMeuRigidbody ().AddForce (vetorDeMovimento * (meuDadosMovimentacao.velocidadeAndando / divisor), ForceMode.Force);

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

		PeDirecao ();

		meuAnimator.SetFloat ("AndarVertical", andarVertical);
		meuAnimator.SetFloat ("AndarHorizontal", andarHorizontal);
	}
	
	private void PeDirecao () {
		float minhaRotacao = transform.eulerAngles.y;

		/* Sempre faz a perna apontar para frente, isto é, na direção do inimigo focado
		 * Como por exemplo, caso o jogador esteja em baixo do inimigo focado, as pernas do Abejide sempre vai ficar apoontando para frene;
		 * Outro exemplo é caso o Abejide estiver na direita do inimigo focado, as pernas do Abejide sempre vai ficar apontando para a Esquerda.
		 */
		if (transform.eulerAngles.y > 135f && transform.eulerAngles.y < 225f) {
			direcaoAngulo = 90f;

			PeDirecaoDuasTeclas (22.5f, -22.5f);
		} else if (transform.eulerAngles.y > 225f && transform.eulerAngles.y < 315f) {
			direcaoAngulo = 45f;

			PeDirecaoDuasTeclas (-22.5f, 22.5f);
		} else if (transform.eulerAngles.y > 315f && transform.eulerAngles.y < 360f) {
			direcaoAngulo = 0f;

			PeDirecaoDuasTeclas (22.5f, -22.5f);
		} else if (transform.eulerAngles.y > 0f && transform.eulerAngles.y < 45f) {
			direcaoAngulo = 180f;
		} else {
			direcaoAngulo = 135f;

			PeDirecaoDuasTeclas (-22.5f, 22.5f);
		}
		
		peDirecao = Mathf.Lerp(peDirecao, 180f - (direcaoAngulo + (minhaRotacao / 2f)), 9f * Time.deltaTime);

		if (Input.GetKeyDown (KeyCode.P)) {
			transform.position = satPosicaoFora.position;
		}
	}

	private void PeDirecaoDuasTeclas (float valor1, float valor2) {
		if ((Input.GetKey (KeyCode.UpArrow) && Input.GetKey (KeyCode.LeftArrow)) ||
			(Input.GetKey (KeyCode.DownArrow) && Input.GetKey (KeyCode.RightArrow))) {
			direcaoAngulo += valor1;
		} else if ((Input.GetKey (KeyCode.UpArrow) && Input.GetKey (KeyCode.RightArrow)) ||
			(Input.GetKey (KeyCode.DownArrow) && Input.GetKey (KeyCode.LeftArrow))) {
			direcaoAngulo += valor2;
		}
	}

	private void BuscarInimigoFocar () {
		GameObject[] inimigoTemp = GameObject.FindGameObjectsWithTag ("Inimigo");
		if (inimigoTemp.Length != 0) {

			int indiceMenorDistancia = 0;
			float distanciaTemp;
			distanciaDoInimigo = Vector3.Distance (transform.position, inimigoTemp [0].transform.position);

			for (int i = 1; i < inimigoTemp.Length; i++) {
				distanciaTemp = Vector3.Distance (transform.position, inimigoTemp [i].transform.position);

				if (distanciaTemp < distanciaDoInimigo) {
					distanciaDoInimigo = distanciaTemp;
					indiceMenorDistancia = i;
				}
			}

			if (distanciaDoInimigo < maxDistanciaOlhando) {
				inimigoParaFocar = inimigoTemp [indiceMenorDistancia].GetComponent<Transform> ();
				return;
			}
		}

		inimigoParaFocar = null;
	}
}
