using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Aqui tem a mecânicas:
 * 		Fazer o Abejide atacar e a dele trocar de arma;
 * 		Ficar alternando entre a da mecânica do abejide olhando para o inimigo e de movimentação livre.
 * 
 * A massa total fica aqui, então os outros scripts tem que fazer referencia a esta viriável.
 */

public class AbejideAtaque : MonoBehaviour {

	public Slider estaminaSlider;
	public Transform seta;
	public Transform inimigo;
	public Animator corpoAnimator;
	public Animator peAnimator;
	public GameObject[] armas;
	public GameObject[] magias;
	public float[] maxTempoMagia;
	public GameObject mao;
	public Camera abejideCamera;
	//Como o corpo é separado das pernas, são necesarios duas variaveis para controlar as animação das partes separadas.
	public RuntimeAnimatorController corpoController;
	public RuntimeAnimatorController peController;
	private AnimatorStateInfo corpoState;

	private bool estaAtaqueAndando;
	private bool estaAtaqueParado;
	private bool olhandoInimigo;
	private bool jogouMagia;
	private bool sentidoHorario;
	private bool perdeEstamina;

	private float estamina;
	public float addEstamina;
	public float subEstaminaAtacando;
	public float subEstaminaCorrendo;
	public float maxEstamina;
	public float maxTempoAndandoAtaque;
	private float tempoCombo;
	public float maxTempoCombo;
	private float tempoAtaqueAndando;
	public float maxTempoAtaqueAndando;
	private float tempoMagia;
	//Salva a distancia do eixo z entra a câmera e o jogador, para que ela sempre fique nesta distancia quando Abejide se mover
	public float distanceCameraZ;
	public float distanceCameraY;
	private float anguloInicial;
	private float anguloChegar;
	private float angulo;
	/*
	 * 0 Parado
	 * 1 Frente;
	 * 2 Direita;
	 * 3 Baixo;
	 * 4 Esquerda;
	 */
	private int direcaoAtaque;
	private int animacaoID;

	void Start () {
		animacaoID = 0;
		direcaoAtaque = 0;

		perdeEstamina = false;

		AtivarCodigo ();
	}

	public void AtivarCodigo () {
		abejideCamera.GetComponent<CameraAto01> ().enabled = false;
		abejideCamera.GetComponent<Animator> ().enabled = false;
		abejideCamera.fieldOfView = 60f;

		estaAtaqueParado = false;
		estaAtaqueAndando = false;
		olhandoInimigo = false;
		jogouMagia = false;

		tempoCombo = 0;
		tempoMagia = 0;

		estamina = maxEstamina;
		estaminaSlider.maxValue = maxEstamina;
		abejideCamera.transform.eulerAngles = new Vector3 (50, 0 ,0);

		corpoAnimator.runtimeAnimatorController = corpoController as RuntimeAnimatorController;
		peAnimator.runtimeAnimatorController = peController as RuntimeAnimatorController;

		GetComponent<DadosForcaResultante> ().armaMassa = armas [corpoAnimator.GetInteger ("ArmaAtual")].GetComponent<Arma> ().massa;
		armas [corpoAnimator.GetInteger("ArmaAtual")].GetComponent<Renderer> ().enabled = true;
		for (int linha = 1; linha < armas.Length; linha++) {
			armas [linha].GetComponent<Renderer> ().enabled = false;
		}

		GetComponent<AbejideAtaque> ().enabled = true;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.P)) {
			estamina = maxEstamina;
		}

		if (estamina < maxEstamina && tempoCombo > maxTempoCombo * 2 && corpoAnimator.GetFloat("AceleracaoAndando") < 1.2f) {
			estamina += addEstamina;
		} else if (corpoAnimator.GetFloat("AceleracaoAndando") > 1.3f) {
			estamina -= subEstaminaCorrendo;
		}
		estaminaSlider.value = estamina;

		if (gameObject.GetComponent<AbejideOlhando> ().enabled) {
			transform.LookAt (new Vector3(inimigo.position.x, transform.position.y, inimigo.position.z));
		} else if (olhandoInimigo && !gameObject.GetComponent<AbejideOlhando> ().enabled) {
			seta.LookAt (new Vector3 (inimigo.position.x, seta.position.y, inimigo.position.z));
			anguloChegar = seta.eulerAngles.y - transform.eulerAngles.y;
			if (anguloChegar < 0) {
				anguloChegar += 360;
			}
			if (angulo == 0 && anguloChegar < 180) {
				sentidoHorario = true;
			}
			RodarAtor ();
		}

		//Faz a câmera seguir o jogador.
		abejideCamera.transform.position = new Vector3 (transform.position.x, gameObject.transform.position.y + distanceCameraY, gameObject.transform.position.z - distanceCameraZ);

		direcaoAtaque = 0;
		Atacar ();

		peAnimator.SetInteger ("DirecaoAtaque", direcaoAtaque);
		corpoState = corpoAnimator.GetCurrentAnimatorStateInfo (0);
		if (!corpoState.IsTag("Atacando") && corpoAnimator.GetFloat ("AceleracaoAndando") == 0f) {
			TrocarDeArma ();

			//Joga a magia.
			tempoMagia += Time.deltaTime;
			if (corpoAnimator.GetInteger ("ArmaAtual") != 0 && tempoMagia > maxTempoMagia[corpoAnimator.GetInteger ("ArmaAtual") - 1] && Input.GetKeyDown (KeyCode.A)) {
				corpoAnimator.SetTrigger ("Magia");
				tempoMagia = 0;
				jogouMagia = true;
			} else if (jogouMagia && tempoMagia > 0.15f) {
				Instantiate (magias [corpoAnimator.GetInteger ("ArmaAtual") - 1], mao.transform.position, mao.transform.rotation);
				jogouMagia = false;
			}
		}

		//Alterna entre a mecânica de movimentação livre e olhando para o inimigo.
		if (Input.GetKeyDown (KeyCode.LeftAlt)) {
			gameObject.GetComponent<AbejideAndando> ().MudarAngulo (transform.eulerAngles.y);

			olhandoInimigo = !olhandoInimigo;
			corpoAnimator.SetBool ("OlhandoInimigo", olhandoInimigo);
			if (olhandoInimigo) {
				gameObject.GetComponent<AbejideAndando> ().enabled = !olhandoInimigo;
				angulo = 0;
				anguloInicial = transform.eulerAngles.y;
				sentidoHorario = false;
			} else {
				gameObject.GetComponent<AbejideAndando> ().enabled = !olhandoInimigo;
				gameObject.GetComponent<AbejideOlhando> ().enabled = olhandoInimigo;
			}
		}

		//Fecha o jogo quando for aperdato a tecla 'esc' do teclado.
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
	}

	void Atacar() {
		if (estaAtaqueParado) {
			if (!perdeEstamina) {
				perdeEstamina = true;
				estamina -= subEstaminaAtacando;
				animacaoID = corpoState.fullPathHash;
			} else if (animacaoID != corpoState.fullPathHash) {
				perdeEstamina = false;
			}
		}

		if (Input.GetKeyDown (KeyCode.Z)) {
			tempoCombo = 0;

			//Se o jogador não estiver executando a animação de ataque após aperar <z>, quer dizer que o jogador vai começar o combo agora.
			if (!corpoState.IsTag ("Atacando")) {
				//Caso o jogador esteja andando e ataquem, Abejide vai dar uma investida.
				if (corpoAnimator.GetFloat ("AceleracaoAndando") != 0f && !estaAtaqueAndando && !estaAtaqueParado && !olhandoInimigo) {
					corpoAnimator.SetBool ("Atacando", true);
					peAnimator.SetBool ("Atacando", true);
					tempoAtaqueAndando = 0;
					estaAtaqueAndando = true;
					corpoAnimator.SetTrigger ("AtaqueAndando");
					peAnimator.SetTrigger ("AtaqueAndando");
					estamina -= subEstaminaAtacando;
				//Se não for a invezdida, quer dizer que o Abejide esta parado, enão vai executar os ataques dele parado.
				} else if (!estaAtaqueParado && !estaAtaqueAndando) {
					estaAtaqueParado = true;
					corpoAnimator.SetBool ("Atacando", true);
					peAnimator.SetBool ("Atacando", true);
				}
			}
		}

		if (estaAtaqueParado) {
			DirecarAtaque (transform.eulerAngles.y);

			if (direcaoAtaque != 0 && !olhandoInimigo) {
				direcaoAtaque = 1;
			}
		} else if (estaAtaqueAndando) {
			tempoAtaqueAndando += Time.deltaTime;

			//Tempo que o Abejide vai ficar dando a investida antes de começar a desacelerar.
			if (tempoAtaqueAndando > maxTempoAtaqueAndando) {
				GetComponent<DadosForcaResultante> ().SubNewtonAndando (1);
				//Faz o Abejide parar mais rapido quando sua ele estiver correndo.
				if (GetComponent<DadosForcaResultante> ().PegarNewtonAndando () > GetComponent<DadosForcaResultante> ().maxNewtonAndando) {
					GetComponent<DadosForcaResultante> ().SubNewtonAndando (1);
				}

				if (GetComponent<DadosForcaResultante> ().PegarNewtonAndando() == 0) {
					//Passa o novo angulo para o estilo de movimentação que estiver ativom pois pode acontecer que no meio desse ataque o Abejide mude de angulo.
					if (olhandoInimigo) {
					} else {
						GetComponent<AbejideAndando> ().MudarAngulo (transform.eulerAngles.y);
					}
					estaAtaqueAndando = false;
					corpoAnimator.SetBool ("Atacando", false);
					peAnimator.SetBool ("Atacando", false);
					GetComponent<DadosForcaResultante> ().MudarNewtonAndando (0);
					corpoAnimator.SetFloat ("AceleracaoAndando", 0);
					peAnimator.SetFloat ("AceleracaoAndando", 0);
				}
			} else {
				GetComponent<DadosForcaResultante> ().AddNewtonAndando (1);
			}
			transform.Translate(0, 0, GetComponent<DadosForcaResultante> ().PegarAceleracaoAndando () * Time.deltaTime);
		}

		//Se o jogador não ficar apertando <z>, depois de um tempo o loop de ataque acaba e o Abejide volta ao seu estado parado.
		tempoCombo += Time.deltaTime;
		if (tempoCombo > maxTempoCombo && estaAtaqueParado) {
			animacaoID = 0;
			perdeEstamina = false;
			estaAtaqueParado = false;
			corpoAnimator.SetBool ("Atacando", false);
			peAnimator.SetBool ("Atacando", false);
		}
	}

	void TrocarDeArma() {
		//TrocaDeArma
		if (Input.GetKeyDown (KeyCode.Alpha1) && corpoAnimator.GetInteger("ArmaAtual") != 0) {
			TrocarDeArma (0);
		} else if (Input.GetKeyDown (KeyCode.Alpha2) && corpoAnimator.GetInteger("ArmaAtual") != 1) {
			TrocarDeArma (1);
		} else if (Input.GetKeyDown (KeyCode.Alpha3) && corpoAnimator.GetInteger("ArmaAtual") != 2) {
			TrocarDeArma (2);
		} else if (Input.GetKeyDown (KeyCode.Alpha4) && corpoAnimator.GetInteger("ArmaAtual") != 3) {
			TrocarDeArma (3);
		}
	}

	void TrocarDeArma (int novaArma) {
		//Habilita a arma segundo o id que foi passado.
		armas [corpoAnimator.GetInteger("ArmaAtual")].GetComponent<Renderer> ().enabled = false;
		corpoAnimator.SetInteger("ArmaAtual", novaArma);
		corpoAnimator.SetTrigger ("TrocarDeArma");
		armas [novaArma].GetComponent<Renderer> ().enabled = true;


		//Atualiza a variavel que quarda a massa da arma atual que Abejide esta usando.
		GetComponent<DadosForcaResultante> ().armaMassa = armas [novaArma].GetComponent<Arma> ().massa;
	}

	private void DirecarAtaque (float angulo) {
		//Sai do metodo caso o jogador esteja apertando duas teclas oposto de movimento, pois uma vai acabar anulando o movimento da outra.
		if (Input.GetKey (KeyCode.LeftArrow) && Input.GetKey (KeyCode.RightArrow)) {
			return;
		} else if (Input.GetKey (KeyCode.UpArrow) && Input.GetKey (KeyCode.DownArrow)) {
			return;
		}

		if (Input.GetKey (KeyCode.UpArrow)) {
			if (angulo > 45 && angulo <= 135) {
				direcaoAtaque = 4;
			} else if (angulo > 135 && angulo <= 225) {
				direcaoAtaque = 3;
			} else if (angulo > 225 && angulo < 315) {
				direcaoAtaque = 2;
			} else {
				direcaoAtaque = 1;
			}
		} if (Input.GetKey (KeyCode.RightArrow)) {
			if (angulo > 45 && angulo <= 135) {
				direcaoAtaque = 1;
			} else if (angulo > 135 && angulo <= 225) {
				direcaoAtaque = 4;
			} else if (angulo > 225 && angulo < 315) {
				direcaoAtaque = 3;
			} else {
				direcaoAtaque = 2;
			}
		} if (Input.GetKey (KeyCode.DownArrow)) {
			if (angulo > 45 && angulo <= 135) {
				direcaoAtaque = 2;
			} else if (angulo > 135 && angulo <= 225) {
				direcaoAtaque = 1;
			} else if (angulo > 225 && angulo < 315) {
				direcaoAtaque = 4;
			} else {
				direcaoAtaque = 3;
			}
		} if (Input.GetKey (KeyCode.LeftArrow)) {
			if (angulo > 45 && angulo <= 135) {
				direcaoAtaque = 3;
			} else if (angulo > 135 && angulo <= 225) {
				direcaoAtaque = 2;
			} else if (angulo > 225 && angulo < 315) {
				direcaoAtaque = 1;
			} else {
				direcaoAtaque = 4;
			}
		} 
	}

	private void RodarAtor () {
		GetComponent<DadosForcaResultante> ().AddNewtonRodando ();

		if (sentidoHorario) {
			angulo += (GetComponent<DadosForcaResultante> ().PegarNewtonRodando () / 45);

			if (angulo + anguloChegar > 360) {
				gameObject.GetComponent<AbejideOlhando> ().enabled = true;
			}
		} else {
			angulo -= (GetComponent<DadosForcaResultante> ().PegarNewtonRodando ()) / 45;

			if (angulo + anguloChegar < 0) {
				gameObject.GetComponent<AbejideOlhando> ().enabled = true;
			}
		}

		transform.eulerAngles = new Vector3 (transform.eulerAngles.x, anguloInicial + angulo, transform.eulerAngles.z);
	}

	public float PegarEstamina () {
		return estamina;
	}

	public bool EstaAtacandoParadoAndando () {
		return direcaoAtaque != 0;
	}
	
	public bool TempoComboMaiorQueLimite () {
		return (tempoCombo > maxTempoCombo);
	}

	public bool PegarEstaAtaqueAndando () {
		return estaAtaqueAndando;
	}
}