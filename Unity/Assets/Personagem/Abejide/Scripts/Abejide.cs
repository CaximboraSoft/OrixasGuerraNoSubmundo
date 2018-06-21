using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Abejide : MonoBehaviour {

	public Slider estamina;
	public float addEstaminaParado;
	public float addEstaminaAndando;
	public float subEstaminaAtacando;
	public float subEstaminaCorrendo;
	public float subEstaminaPulando;
	public float subEstaminaEsquivando;
	public int iDUltimoAtaque = 0;

	public Vector3 vetorDeMovimento;
	private DadosMovimentacao meuDadosMovimentacao;
	private AnimatorStateInfo armaStateInfo;
	private AnimatorStateInfo peStateInfo;
	private AnimatorStateInfo esquivaStateInfo;
	private Arma armaScript;
	public Renderer[] armas;
	public float[] armasDano;
	public float[] alcanceEspadas;
	public GameObject[] magias;
	public AbejideChao meuAbejideChao;
	private Vector3 angulo = Vector3.zero;
	private Animator meuAnimator;
	public Transform inimigoParaFocar;
	private float distanciaDoInimigo = 0f;
	public Transform seta;
	public Transform satPosicaoFora;
	public Image[] coracoes;
	public int vidas = 6;

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
	public float tempoParaAtacar; //Quarda o tempo que o jogodor tem que esperar par poder usar o ataque de andando
	public float peDirecao = 0f;
	private float direcaoAngulo; //Usada no método <PeDirecao>, pois não se pode usar uma variavel local em uma função de la
	private float forcaDeMovimento;
	private float tempoParaPoderPular;
	private float tempoSubindo;
	public float forcaCaindo = 9800f;

	private bool chegouNoAnguloDaEsquiva;
	private float anguloDaEsquiva;

	public int armaAtual = 0;
	private int andarVertical = 0;
	private int andarHorizontal = 0;

	private bool estouNoChao;
	private bool estaAtaqueAndando;
	public bool possoPular = true;

	void Awake () {
		meuAnimator = GetComponentInChildren<Animator> ();
		armaScript = GetComponentInChildren<Arma> ();
		armaScript.enabled = false;

		for (int linha = 0; linha < armas.Length; linha++) {
			armas [linha].enabled = false;
		}
	}

	void Start () {
		if (GetComponent<Abejide> ().enabled) {
			AtivarCodigo ();
		}
	}

	public void AtivarCodigoLula () {
		forcaDeMovimento = 0f;
		tempoCombo = maxTempoCombo + 1f;
		meuAnimator.SetBool ("Atacando", false);
		GetComponent<Abejide> ().enabled = true;
	}

	// Executa mesmo se o código estiver desatiado
	public void AtivarCodigo () {
		meuDadosMovimentacao = GetComponent<DadosMovimentacao> ();

		tempoParaAtacar = 0;

		esquivando = false;
		estouNoChao = true;
		possoPular = true;
		estaMovendo = false;
		estaAtaqueAndando = false;

		GetComponent<Abejide> ().enabled = true;

		meuAbejideChao = GetComponentInChildren<AbejideChao> ();
		meuAbejideChao.GetComponent<Collider> ().enabled = true;
	}

	// Update is called once per frame
	void Update () {
		for (int i = 0; i < coracoes.Length; i++) {
			if (i < vidas) {
				coracoes [i].enabled = true;
			} else {
				coracoes [i].enabled = false;
			}
		}

		if (inimigoParaFocar != null && inimigoParaFocar.GetComponent<DadosMovimentacao> ().vida < 1) {
			inimigoParaFocar = null;
			BuscarInimigoFocar ();
		}

		SistemaDePulo ();

		peStateInfo = meuAnimator.GetCurrentAnimatorStateInfo (0);
		if (armaAtual + 1 < meuAnimator.layerCount - 1) {
			armaStateInfo =  meuAnimator.GetCurrentAnimatorStateInfo (armaAtual + 2);
		}
		esquivaStateInfo = meuAnimator.GetCurrentAnimatorStateInfo (meuAnimator.layerCount - 3);

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
		if (estamina.value > estamina.maxValue / 2f) {
			meuAnimator.SetFloat ("VelocidadeAtacando", velocidadeAtacando);
		} else {
			meuAnimator.SetFloat ("VelocidadeAtacando", velocidadeAtacando * (estamina.value + (estamina.maxValue / 2f)));
		}
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
			} else if (distanciaDoInimigo > maxDistanciaOlhando + 1f) {
				focandoInimigoAnimator = false;
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

		//Estamina
		if (!meuAnimator.GetBool ("Atacando") && !armaStateInfo.IsTag ("Atacando")) {
			//Perde estamina correndo
			if (meuAnimator.GetFloat ("Velocidade") >= 4f && !esquivando) {
				estamina.value -= subEstaminaCorrendo;
			//Recupera estamina
			} else {
				iDUltimoAtaque = 0;

				if (peStateInfo.IsTag ("Parado")) {
					estamina.value += addEstaminaParado;
				} else {
					estamina.value += addEstaminaAndando;
				}
			}
		}
	}

	void FixedUpdate () {
		if (estaAtaqueAndando) {
			if (tempoSubindo > 0.5f) {
				meuDadosMovimentacao.PegarMeuRigidbody ().AddForce (transform.up * -forcaCaindo, ForceMode.Force);
			} else {
				tempoSubindo += Time.deltaTime;
			}
		} else if (!meuAnimator.GetBool ("Subindo") && !estouNoChao && !estaAtaqueAndando) {
			meuDadosMovimentacao.PegarMeuRigidbody ().AddForce (transform.up * -forcaCaindo, ForceMode.Force);
		}

		if (!focandoInimigoAnimator || esquivando) {
			//Faz sempre o Abejide se mover para frente.
			meuDadosMovimentacao.PegarMeuRigidbody ().AddForce (transform.forward * forcaDeMovimento * estamina.value, ForceMode.Force);
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
			meuDadosMovimentacao.PegarMeuRigidbody().AddForce (transform.up *
				                                               meuDadosMovimentacao.velocidadePulando);
			possoPular = false;
			tempoSubindo = 0;
			meuAbejideChao.bateuNoChao = false;

			estamina.value -= subEstaminaPulando;
		}

		if (!estouNoChao) {
			if (meuAnimator.GetBool ("Subindo") && !estaAtaqueAndando) {
				tempoSubindo += Time.deltaTime;

				if (tempoSubindo > 0.2f) {
					meuAnimator.SetBool ("Subindo", false);
					meuAnimator.SetTrigger ("SubirOuDescer");
				}
			} else if (meuAbejideChao.bateuNoChao) {
				meuDadosMovimentacao.PegarMeuRigidbody().velocity = Vector3.zero;
				tempoParaPoderPular = 0f;
				estouNoChao = true;

				//Só entra neste if se o jogador tiver usando o ataque do Abejide corredo
				if (meuAnimator.GetBool ("Atacando")) {
					tempoParaAtacar = -0.2f;
					estaAtaqueAndando = false;
					meuAnimator.SetBool ("Atacando", false);
					meuAnimator.SetBool ("Correndo", false);
				}
			}
		} else if (!possoPular) {
			tempoParaPoderPular += Time.deltaTime;

			if (tempoParaPoderPular > 0.3f) {
				possoPular = true;
			}
		} else if (!meuAbejideChao.bateuNoChao && !estaAtaqueAndando) {
			possoPular = false;
			estouNoChao = false;
			meuAnimator.SetBool ("Subindo", false);
			meuAnimator.SetTrigger ("SubirOuDescer");
		}
	}

	void Esquiva () {
		if (Input.GetKeyDown (KeyCode.Space) && !esquivando && estouNoChao && meuAnimator.GetFloat ("Velocidade") != 0f) {
			esquivando = true;
			chegouNoAnguloDaEsquiva = false;
			meuAnimator.SetBool ("Atacando", false);
			estamina.value -= subEstaminaEsquivando;

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
				float tempoDeRotacao = meuDadosMovimentacao.velocidadeRotacao * 15f;
				transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (new Vector3 (0, anguloDaEsquiva, 0)), tempoDeRotacao * estamina.value * Time.deltaTime);

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
					meuDadosMovimentacao.PegarMeuRigidbody().AddForce (transform.up * (meuDadosMovimentacao.velocidadePulando / 4f));
					meuAnimator.SetTrigger ("AtaqueAndando");
					estouNoChao = false;
					meuAnimator.SetBool ("Atacando", true);
					estaAtaqueAndando = true;
					possoPular = false;
					tempoParaAtacar = 0f;
					tempoSubindo = 0f;
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

				//Perde estamina atacando
				if (iDUltimoAtaque != armaStateInfo.fullPathHash) {
					iDUltimoAtaque = armaStateInfo.fullPathHash;
					estamina.value -= subEstaminaAtacando;
				}
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
		//Movimentacao do abejide e, tambem faz ele apontar para a "seta" que foi pressionada.
		if (Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow)) {
			estaMovendo = true; //Controle para fazer o personagem se mover.

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
					}
					if (tempoParaAtacar != 0f) {
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
				transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (angulo), tempoDeRotacao * estamina.value * Time.deltaTime);
			} else if (chegouNoAnguloDaEsquiva) {
				float tempoDeRotacao = meuDadosMovimentacao.velocidadeRotacao * 1.2f;
				transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (angulo), tempoDeRotacao * estamina.value * Time.deltaTime);

				forcaDeMovimento = velocidadeEsquiva;
			}
		} else if (!estaAtaqueAndando) {
			forcaDeMovimento = 0f;
		}
	}

	void MovimentacaoOlhando () {
		seta.LookAt (inimigoParaFocar);

		//Faz o abejide olhar suavemente para o inimigo focado
		float tempoDeRotacao = meuDadosMovimentacao.velocidadeRotacao +
			                   meuDadosMovimentacao.PegarMeuRigidbody ().velocity.magnitude;
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3 (0, seta.eulerAngles.y, 0)), tempoDeRotacao * estamina.value * 1.5f * Time.deltaTime);


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
		GameObject miniBossTemp = GameObject.FindGameObjectWithTag ("MiniBoss");

		if (inimigoTemp.Length != 0) {

			int indiceMenorDistancia = 0;
			float distanciaTemp;
			distanciaDoInimigo = 1000f;

			for (int i = 0; i < inimigoTemp.Length; i++) {
				distanciaTemp = Vector3.Distance (transform.position, inimigoTemp [i].transform.position);

				if (distanciaTemp < distanciaDoInimigo) {
					distanciaDoInimigo = distanciaTemp;
					indiceMenorDistancia = i;
				}
			}

			if (miniBossTemp != null) {
				if (Vector3.Distance (miniBossTemp.transform.position, transform.position) < distanciaDoInimigo) {
					distanciaDoInimigo = Vector3.Distance (miniBossTemp.transform.position, transform.position);
					inimigoParaFocar = miniBossTemp.transform;
					return;
				} else {
					inimigoParaFocar = inimigoTemp [indiceMenorDistancia].GetComponent<Transform> ();
					return;
				}
			} else if (distanciaDoInimigo < maxDistanciaOlhando) {
				inimigoParaFocar = inimigoTemp [indiceMenorDistancia].GetComponent<Transform> ();
				return;
			}
		}

		inimigoParaFocar = null;
	}

	public Transform LugarParaCameraOlhar () {
		if (inimigoParaFocar != null && focandoInimigoAnimator) {
			return inimigoParaFocar;
		}

		return transform;
	}

	public void TrocarDeArma (int indiceArma) {
		armas [armaAtual].enabled = false;
		armas [indiceArma].enabled = false;
		armaAtual = indiceArma;
	}

	public void PerderVida () {
		vidas--;
	}

	void OnCollisionEnter (Collision other) {
		if (other.transform.tag == "Vida" && vidas < 6) {
			Destroy (other.gameObject);
			vidas++;
		}
	}
}
