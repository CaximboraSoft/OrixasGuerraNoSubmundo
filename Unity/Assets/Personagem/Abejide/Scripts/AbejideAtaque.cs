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
	private bool olhandoInimigo;
	private bool jogouMagia;

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
		AtivarCodigo ();
	}

	public void AtivarCodigo () {
		estaAtaqueAndando = false;
		olhandoInimigo = false;
		jogouMagia = false;

		tempoCombo = 0;
		tempoMagia = 0;

		abejideCamera = Camera.main;
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
				if (abejideAnimator.GetFloat ("AceleracaoAndando") >= 0.4f && !estaAtaqueAndando) {
					estaAtaqueAndando = true;
					abejideAnimator.SetTrigger ("AtacandoCorrendo");

				} else if (!abejideAnimator.GetBool ("Atacando")) {
					abejideAnimator.SetBool ("Atacando", true);
				}
			}
		}

		if (estaAtaqueAndando) {
			tempoAtaqueAndando += Time.deltaTime;
			if (tempoAtaqueAndando > maxTempoAtaqueAndando) {
				GetComponent<DadosForcaResultante> ().SubNewtonAndando (1);
				if (GetComponent<DadosForcaResultante> ().PegarNewtonAndando () > GetComponent<DadosForcaResultante> ().maxNewtonAndando) {
					GetComponent<DadosForcaResultante> ().SubNewtonAndando (1);
				}

				if (GetComponent<DadosForcaResultante> ().PegarNewtonAndando() == 0) {
					estaAtaqueAndando = false;
				}
			} else {
				GetComponent<DadosForcaResultante> ().AddNewtonAndando ();
			}

			abejideAnimator.SetFloat ("AceleracaoAndando",  (GetComponent<DadosForcaResultante>().PegarAceleracaoAndando ()));
			transform.Translate(0, 0, abejideAnimator.GetFloat ("AceleracaoAndando") * Time.deltaTime);
		}

		if (tempoCombo > maxTempoCombo) {
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

	public bool TempoComboMaiorQueLimite () {
		return (tempoCombo > maxTempoCombo);
	}

	public bool PegarEstaAtaqueAndando () {
		return estaAtaqueAndando;
	}
}
