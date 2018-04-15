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
	public GameObject mao;
	public Camera abejideCamera;
	private AnimatorStateInfo animacaoState;

	private bool olhandoInimigo;
	private bool jogouMagia;

	private float tempoCombo;
	public float maxTempoCombo;
	private float tempoMagia;
	public float[] maxTempoMagia;
	public float massa = 0;
	private float massaArma;
	//Salva a distancia do eixo z entra a câmera e o jogador, para que ela sempre fique nesta distancia quando Abejide se mover
	private float distanceCameraZ;
	private float distanceCameraY;

	// Use this for initialization
	void Start () {
		olhandoInimigo = false;
		jogouMagia = false;

		tempoCombo = 0;
		tempoMagia = 0;

		abejideCamera = Camera.main;
		distanceCameraY = gameObject.transform.position.y - abejideCamera.transform.position.y;
		distanceCameraZ = gameObject.transform.position.z - abejideCamera.transform.position.z;

		abejideAnimator = GameObject.Find ("AbejideMesh").GetComponent<Animator> ();

		massaArma = armas [abejideAnimator.GetInteger ("ArmaAtual")].GetComponent<Arma> ().massa;
		armas [abejideAnimator.GetInteger("ArmaAtual")].GetComponent<Renderer> ().enabled = true;
		for (int linha = 1; linha < armas.Length; linha++) {
			armas [linha].GetComponent<Renderer> ().enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		seta.LookAt (new Vector3(inimigo.position.x, seta.position.y, inimigo.position.z));

		//Faz a câmera seguir o jogador.
		abejideCamera.transform.position = new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y - distanceCameraY, gameObject.transform.position.z - distanceCameraZ);

		Atacar ();

		animacaoState = abejideAnimator.GetCurrentAnimatorStateInfo (0);
		if (!animacaoState.IsTag("Atacando") && !abejideAnimator.GetBool ("Andando") ) {
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
			gameObject.GetComponent<AbejideOlhando> ().enabled = olhandoInimigo;
			gameObject.GetComponent<AbejideAndando> ().enabled = !olhandoInimigo;
		}

		//Fecha o jogo quando for aperdato a tecla 'esc' do teclado.
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
	}

	void Atacar() {
		tempoCombo += Time.deltaTime;

		if (Input.GetKeyDown (KeyCode.Z) && !abejideAnimator.GetBool ("Andando")) {
			tempoCombo = 0;

			if (!abejideAnimator.GetBool ("Atacando")) {
				abejideAnimator.SetBool ("Atacando", true);
			}
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
		massaArma = armas [novaArma].GetComponent<Arma> ().massa;
	}

	public float PegarMassaTotal() {
		return (massa + massaArma);
	}
}
