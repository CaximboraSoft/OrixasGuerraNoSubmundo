using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Aqui tem a mecânicas:
 * 		Fazer o Abejide atacar e a dele trocar de arma;
 * 		Ficar alternando entre a da mecânica do abejide olhando para o inimigo e de movimentação livre.
 * 
 * A massa total fica aqui, então os outros scripts tem que fazer referencia a esta viriável.
 */

public class AbejideAtaque : MonoBehaviour {

	public Transform seta;
	public Transform inimigo;
	private Animator abejideAnimator;
	public GameObject[] armas;
	public GameObject[] magias;
	public float[] maxTempoMagia;
	public GameObject mao;
	public Camera abejideCamera;
	public RuntimeAnimatorController controllerAndando;
	public RuntimeAnimatorController controllerOlhando;
	private AnimatorStateInfo animacaoState;

	private bool estaAtaqueAndando;
	private bool estaAtaqueParado;
	private bool olhandoInimigo;
	private bool jogouMagia;
	/*
	 * 0 Frente;
	 * 1 Direita;
	 * 2 Baixo;
	 * 3 Esquerda;
	 */

	public bool[] direcaoAtaque = new bool[4];

	public float tempoAndandoAtaque;
	public float maxTempoAndandoAtaque;
	private float tempoCombo;
	public float maxTempoCombo;
	private float tempoAtaqueAndando;
	public float maxTempoAtaqueAndando;
	private float tempoMagia;
	//Salva a distancia do eixo z entra a câmera e o jogador, para que ela sempre fique nesta distancia quando Abejide se mover
	public float distanceCameraZ;
	public float distanceCameraY;

	public float distanciaAngulo;

	void Start () {
		for (int linha = 0; linha < direcaoAtaque.Length; linha++) {
			direcaoAtaque [linha] = false;
		}

		AtivarCodigo ();
	}

	public void AtivarCodigo () {
		estaAtaqueParado = false;
		estaAtaqueAndando = false;
		olhandoInimigo = false;
		jogouMagia = false;

		tempoCombo = 0;
		tempoMagia = 0;
		tempoAndandoAtaque = maxTempoAndandoAtaque;

		abejideCamera.transform.eulerAngles = new Vector3 (50, 0 ,0);

		abejideAnimator = GameObject.Find ("AbejideMesh").GetComponent<Animator> ();
		abejideAnimator.runtimeAnimatorController = controllerAndando as RuntimeAnimatorController;

		GetComponent<DadosForcaResultante> ().armaMassa = armas [abejideAnimator.GetInteger ("ArmaAtual")].GetComponent<Arma> ().massa;
		armas [abejideAnimator.GetInteger("ArmaAtual")].GetComponent<Renderer> ().enabled = true;
		for (int linha = 1; linha < armas.Length; linha++) {
			armas [linha].GetComponent<Renderer> ().enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (gameObject.GetComponent<AbejideOlhando> ().enabled) {
			transform.LookAt (new Vector3(inimigo.position.x, transform.position.y, inimigo.position.z));
		} else if (olhandoInimigo && !gameObject.GetComponent<AbejideOlhando> ().enabled) {
			//GetComponent<DadosForcaResultante> ().AddNewtonRodando ();
			seta.LookAt (new Vector3(inimigo.position.x, seta.position.y, inimigo.position.z));

			distanciaAngulo = transform.eulerAngles.y - seta.transform.eulerAngles.y;
			if (distanciaAngulo < 0) {
				distanciaAngulo += 360;
			} else if (distanciaAngulo > 360) {
				distanciaAngulo -= 360;
			}

			if (distanciaAngulo > 180) {
//				transform.Rotate (0, (newtonRodando / GetComponent<DadosForcaResultante> ().PegarMassaTotal ()) * Time.deltaTime, 0);

				if (distanciaAngulo >= 350) {
					gameObject.GetComponent<AbejideOlhando> ().enabled = true;
				}
			} else {
//				transform.Rotate (0, -(newtonRodando / GetComponent<DadosForcaResultante> ().PegarMassaTotal ()) * Time.deltaTime, 0);

				if (distanciaAngulo <= 10) {
					gameObject.GetComponent<AbejideOlhando> ().enabled = true;
				}
			}
		}

		//Faz a câmera seguir o jogador.
		abejideCamera.transform.position = new Vector3 (transform.position.x, gameObject.transform.position.y + distanceCameraY, gameObject.transform.position.z - distanceCameraZ);

		Atacar ();

		animacaoState = abejideAnimator.GetCurrentAnimatorStateInfo (0);
		if (!animacaoState.IsTag("Atacando") && abejideAnimator.GetFloat ("AceleracaoAndando") == 0f) {
			TrocarDeArma ();

			//Joga a magia.
			tempoMagia += Time.deltaTime;
			if (abejideAnimator.GetInteger ("ArmaAtual") != 0 && tempoMagia > maxTempoMagia[abejideAnimator.GetInteger ("ArmaAtual") - 1] && Input.GetKeyDown (KeyCode.A)) {
				abejideAnimator.SetTrigger ("Magia");
				tempoMagia = 0;
				jogouMagia = true;
			} else if (jogouMagia && tempoMagia > 0.15f) {
				Instantiate (magias [abejideAnimator.GetInteger ("ArmaAtual") - 1], mao.transform.position, mao.transform.rotation);
				jogouMagia = false;
			}
		}

		//Alterna entre a mecânica de movimentação livre e olhando para o inimigo.
		if (Input.GetKeyDown (KeyCode.LeftAlt)) {
			gameObject.GetComponent<AbejideAndando> ().MudarAngulo (transform.eulerAngles.y);

			olhandoInimigo = !olhandoInimigo;
			//gameObject.GetComponent<AbejideOlhando> ().enabled = olhandoInimigo;
			gameObject.GetComponent<AbejideAndando> ().enabled = !olhandoInimigo;
			if (gameObject.GetComponent<AbejideAndando> ().enabled) {
				gameObject.GetComponent<AbejideOlhando> ().enabled = false;
				//newtonRodando = 0;
			}
		}

		//Fecha o jogo quando for aperdato a tecla 'esc' do teclado.
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
	}

	void Atacar() {
		tempoCombo += Time.deltaTime;

		if (Input.GetKeyDown (KeyCode.Z)) {
			tempoCombo = 0;

			if (!animacaoState.IsTag ("Atacando")) {
				if (abejideAnimator.GetFloat ("AceleracaoAndando") != 0f && !estaAtaqueAndando && !estaAtaqueParado) {
					abejideAnimator.SetFloat ("AceleracaoAndando",  0);
					abejideAnimator.SetBool ("Atacando", true);
					tempoAtaqueAndando = 0;
					estaAtaqueAndando = true;
					abejideAnimator.SetTrigger ("AtacandoCorrendo");
				} else if (!estaAtaqueParado && !estaAtaqueAndando) {
					estaAtaqueParado = true;
					abejideAnimator.SetBool ("Atacando", true);
				}
			}

			if (estaAtaqueParado && tempoAndandoAtaque >= maxTempoAndandoAtaque) {
				for (int linha = 0; linha < direcaoAtaque.Length; linha++) {
					direcaoAtaque [linha] = false;
				}
				tempoAndandoAtaque = 0;
				GetComponent<DadosForcaResultante> ().MudarNewtonAndando (0);

				DirecarAtaque (transform.eulerAngles.y);
			}
		}

		//Faz o personagem andar um pouco para o lado que o jogador estiver apertando segundo a seta quando ele estiver atacando.
		if (estaAtaqueParado && tempoAndandoAtaque < maxTempoAndandoAtaque) {
			tempoAndandoAtaque += Time.deltaTime;

			GetComponent<DadosForcaResultante> ().AddNewtonAndando ();
			abejideAnimator.SetFloat ("AceleracaoAndando",  (GetComponent<DadosForcaResultante>().PegarAceleracaoAndando ()));

			if (direcaoAtaque[0]) {
				transform.Translate(0, 0, abejideAnimator.GetFloat ("AceleracaoAndando") * Time.deltaTime);
			} else if (direcaoAtaque[2]) {
				transform.Translate(0, 0, -abejideAnimator.GetFloat ("AceleracaoAndando") * Time.deltaTime);
			}
			if (direcaoAtaque[1]) {
				transform.Translate(abejideAnimator.GetFloat ("AceleracaoAndando") * Time.deltaTime, 0, 0);
			} else if (direcaoAtaque[3]) {
				transform.Translate(-abejideAnimator.GetFloat ("AceleracaoAndando") * Time.deltaTime, 0, 0);
			}

		//Sistema para o Abejide correr um pouco com a espada apos ele apertar para atacar quando o mesmo estiver andando.
		} else if (estaAtaqueAndando) {
			tempoAtaqueAndando += Time.deltaTime;

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
					abejideAnimator.SetBool ("Atacando", false);
				}
			} else {
				GetComponent<DadosForcaResultante> ().AddNewtonAndando ();
			}
			transform.Translate(0, 0, GetComponent<DadosForcaResultante> ().PegarAceleracaoAndando () * Time.deltaTime);
		}

		if (tempoCombo > maxTempoCombo && estaAtaqueParado) {
			GetComponent<DadosForcaResultante> ().MudarNewtonAndando (0);
			abejideAnimator.SetFloat ("AceleracaoAndando",  0);
			estaAtaqueParado = false;
			abejideAnimator.SetBool ("Atacando", false);
		}
	}

	void TrocarDeArma() {
		//TrocaDeArma
		if (Input.GetKeyDown (KeyCode.Alpha1) && abejideAnimator.GetInteger("ArmaAtual") != 0) {
			TrocarDeArma (0);
		} else if (Input.GetKeyDown (KeyCode.Alpha2) && abejideAnimator.GetInteger("ArmaAtual") != 1) {
			TrocarDeArma (1);
		} else if (Input.GetKeyDown (KeyCode.Alpha3) && abejideAnimator.GetInteger("ArmaAtual") != 2) {
			TrocarDeArma (2);
		} else if (Input.GetKeyDown (KeyCode.Alpha4) && abejideAnimator.GetInteger("ArmaAtual") != 3) {
			TrocarDeArma (3);
		}
	}

	void TrocarDeArma (int novaArma) {
		//Habilita a arma segundo o id que foi passado.
		armas [abejideAnimator.GetInteger("ArmaAtual")].GetComponent<Renderer> ().enabled = false;
		abejideAnimator.SetInteger("ArmaAtual", novaArma);
		abejideAnimator.SetTrigger ("TrocarDeArma");
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
				direcaoAtaque [3] = true;
			} else if (angulo > 135 && angulo <= 225) {
				direcaoAtaque [2] = true;
			} else if (angulo > 225 && angulo < 315) {
				direcaoAtaque [1] = true;
			} else {
				direcaoAtaque [0] = true;
			}
		} if (Input.GetKey (KeyCode.RightArrow)) {
			if (angulo > 45 && angulo <= 135) {
				direcaoAtaque [0] = true;
			} else if (angulo > 135 && angulo <= 225) {
				direcaoAtaque [3] = true;
			} else if (angulo > 225 && angulo < 315) {
				direcaoAtaque [2] = true;
			} else {
				direcaoAtaque [1] = true;
			}
		} if (Input.GetKey (KeyCode.DownArrow)) {
			if (angulo > 45 && angulo <= 135) {
				direcaoAtaque [1] = true;
			} else if (angulo > 135 && angulo <= 225) {
				direcaoAtaque [0] = true;
			} else if (angulo > 225 && angulo < 315) {
				direcaoAtaque [3] = true;
			} else {
				direcaoAtaque [2] = true;
			}
		} if (Input.GetKey (KeyCode.LeftArrow)) {
			if (angulo > 45 && angulo <= 135) {
				direcaoAtaque [2] = true;
			} else if (angulo > 135 && angulo <= 225) {
				direcaoAtaque [1] = true;//estaAndandoX = true;
				//newtonX = CalcularFisica (newtonX, aceleracaoX, true);
			} else if (angulo > 225 && angulo < 315) {
				direcaoAtaque [0] = true;
			} else {
				direcaoAtaque [3] = true;
			}
		} 
	}

	public bool TempoComboMaiorQueLimite () {
		return (tempoCombo > maxTempoCombo);
	}

	public bool PegarEstaAtaqueAndando () {
		return estaAtaqueAndando;
	}
}
