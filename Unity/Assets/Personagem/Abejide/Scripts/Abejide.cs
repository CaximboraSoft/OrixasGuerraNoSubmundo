﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Abejide : MonoBehaviour {

	public Transform inimigo;
	private Animator abejideAnimator;
	public Text message;
	public Camera abejideCamera;
	public GameObject gameObjectAbejide;
	public GameObject cameraPosiiton;
	public GameObject[] armas;

	private bool estaAndando;
	private bool estaRodando;
	public bool olhandoInimigo;

	public float massa = 0;
	private float massaArma;
	public float peso;
	private float aceleracaoAndando;
	public float newtonAndando; //força(newton) = massa(kg) * aceleração(m/s²)
	public float addNewtonAndando;
	public float subNewtonAndando;
	public float maxNewtonAndando;
	public float maxNewtonCorrendo;
	public float minPlayAndando = 0.5f; //Só pode disparar a animação de andando quando a velocidade do Abejide for menor que isso.

	public float aceleracaoRotacao;
	public float addNewtonRotacao;
	public float maxAceleracaoRotacao;

	private AnimatorStateInfo animacaoState;

	//Salva a distancia do eixo z entra a câmera e o jogador, para que ela sempre fique nesta distancia quando Abejide se mover
	private float distanceCameraZ;
	private float distanceCameraY;
	private float angulo;
	private float chegarAngulo;

	public float tempoCombo;
	public float maxTempoCombo;

	// Use this for initialization
	void Start () {
		estaRodando = false;
		olhandoInimigo = true;

		tempoCombo = 0;
		aceleracaoAndando = 0;
		aceleracaoRotacao = 0;
		distanceCameraY = gameObject.transform.position.y - abejideCamera.transform.position.y;
		distanceCameraZ = gameObject.transform.position.z - abejideCamera.transform.position.z;

		abejideAnimator = gameObjectAbejide.GetComponent<Animator>();

		massaArma = armas [abejideAnimator.GetInteger ("ArmaAtual")].GetComponent<Arma> ().massa;
		armas [abejideAnimator.GetInteger("ArmaAtual")].GetComponent<Renderer> ().enabled = true;
		for (int linha = 1; linha < armas.Length; linha++) {
			armas [linha].GetComponent<Renderer> ().enabled = false;
		}
	}

	// Update is called once per frame
	void Update () {
		//Faz a câmera seguir o jogador.
		abejideCamera.transform.position = new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y - distanceCameraY, gameObject.transform.position.z - distanceCameraZ);
		animacaoState = abejideAnimator.GetCurrentAnimatorStateInfo (0);

		Atacar ();
		if (animacaoState.IsTag("Andando") || animacaoState.IsTag("Parado")) {
			MoverRodarAbejide ();
		}
		CalcularFisica ();

		if (Input.GetKeyDown (KeyCode.LeftAlt)) {
			olhandoInimigo = !olhandoInimigo;
		}
		//Fecha o jogo quando for aperdato a tecla 'esc' do teclado.
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
	}

	void Atacar () {
		if (!animacaoState.IsTag("Atacando") && !abejideAnimator.GetBool ("Andando") ) {
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

	void MoverRodarAbejide () {
		estaAndando = false;

		if (olhandoInimigo) {
			transform.LookAt (new Vector3(inimigo.position.x, transform.position.y, inimigo.position.z));
			angulo = transform.eulerAngles.y;

			if (Input.GetKey (KeyCode.LeftArrow)) {
				estaAndando = true;

				if (angulo > 45 && angulo <= 135) {
					gameObject.transform.Translate(0, 0, -aceleracaoAndando * Time.deltaTime);
				} else if (angulo > 135 && angulo <= 225) {
					gameObject.transform.Translate(aceleracaoAndando * Time.deltaTime, 0, 0);
				} else if (angulo > 225 && angulo < 315) {
					gameObject.transform.Translate(0, 0, aceleracaoAndando * Time.deltaTime);
				} else {
					gameObject.transform.Translate(-aceleracaoAndando * Time.deltaTime, 0, 0);
				}
			} else if (Input.GetKey (KeyCode.RightArrow)) {
				estaAndando = true;

				if (angulo > 45 && angulo <= 135) {
					gameObject.transform.Translate(0, 0, aceleracaoAndando * Time.deltaTime);
				} else if (angulo > 135 && angulo <= 225) {
					gameObject.transform.Translate(-aceleracaoAndando * Time.deltaTime, 0, 0);
				} else if (angulo > 225 && angulo < 315) {
					gameObject.transform.Translate(0, 0, -aceleracaoAndando * Time.deltaTime);
				} else {
					gameObject.transform.Translate(aceleracaoAndando * Time.deltaTime, 0, 0);
				}
			} else if (Input.GetKey (KeyCode.UpArrow)) {
				estaAndando = true;

				if (angulo > 45 && angulo <= 135) {
					gameObject.transform.Translate(-aceleracaoAndando * Time.deltaTime, 0, 0);
				} else if (angulo > 135 && angulo <= 225) {
					gameObject.transform.Translate(0, 0, -aceleracaoAndando * Time.deltaTime);
				} else if (angulo > 225 && angulo < 315) {
					gameObject.transform.Translate(aceleracaoAndando * Time.deltaTime, 0, 0);
				} else {
					gameObject.transform.Translate(0, 0, aceleracaoAndando * Time.deltaTime);
				}
			} else if (Input.GetKey (KeyCode.DownArrow)) {
				estaAndando = true;

				if (angulo > 45 && angulo <= 135) {
					gameObject.transform.Translate(aceleracaoAndando * Time.deltaTime, 0, 0);
				} else if (angulo > 135 && angulo <= 225) {
					gameObject.transform.Translate(0, 0, aceleracaoAndando * Time.deltaTime);
				} else if (angulo > 225 && angulo < 315) {
					gameObject.transform.Translate(-aceleracaoAndando * Time.deltaTime, 0, 0);
				} else {
					gameObject.transform.Translate(0, 0, -aceleracaoAndando * Time.deltaTime);
				}
			}
		} else {
			/*Se na hora de rotacionar o Abejide de acordo com o que o jogador estiver pressionando for necessario passar de -180,
		 *ficar menor que -180, quer dizer que o novo angulo do Abejide tem que ser positivo e é isso que este if faz, deixa
		 * o angulo positivo, pois o código só funciona sem bugs com angulos entre -180 e 180.
		 */
			if (angulo < chegarAngulo - 180) {
				angulo += 360;
				/*Se na hora de rotacionar o Abejide de acordo com o que o jogador estiver pressionando for necessario passar de 180,
		 *ficar maior que 180, quer dizer que o novo angulo do Abejide tem que ser negativo e é isso que este elseif faz, deixa
		 * o angulo negativo, pois o código só funciona sem bugs com angulos entre -180 e 180.
		 */
			} else if (angulo > chegarAngulo + 180) {
				angulo -= 360;
			}


			if (Input.GetKey (KeyCode.LeftArrow)) {
				estaAndando = true;

				if (Input.GetKey (KeyCode.UpArrow)) {
					chegarAngulo = -45;
					RotacionarAbejide ();
				} else if (Input.GetKey (KeyCode.DownArrow)) {
					chegarAngulo = -135;
					RotacionarAbejide ();
				} else {
					/*
				 * Se o jogador apertar para Abejide virar para a esquerda, é feito uma decisão para saber se o menor
				 * caminho para chegar em -90 graus é girando para a esquerda ou para a direita, entãi se a rotação
				 * estiver acima de 90, quer dizer que para ele rotacionar ate chegar em -90, o caminho mais rapido
				 * é por baixo, então é necessario subtrair 360 do angulo atual, pois depois que o angulo chega em 180 passa
				 * automaticamente para -180 e vise versa.
				 */
					chegarAngulo = -90;
					RotacionarAbejide ();
				}
			} else if (Input.GetKey (KeyCode.RightArrow)) {
				estaAndando = true;

				if (Input.GetKey (KeyCode.UpArrow)) {
					chegarAngulo = 45;
					RotacionarAbejide ();
				} else if (Input.GetKey (KeyCode.DownArrow)) {
					chegarAngulo = 135;
					RotacionarAbejide ();
				} else {
					chegarAngulo = 90;
					RotacionarAbejide ();
				}
			} else if (Input.GetKey (KeyCode.UpArrow)) {
				estaAndando = true;

				chegarAngulo = 0;
				RotacionarAbejide ();
			} else if (Input.GetKey (KeyCode.DownArrow)) {
				estaAndando = true;
			
				/*
			 * A roração para baixo não tem como usar o metodo 'RotacionarAbejide()' para rotacionar, pois após o angulo
			 * passar de -180, ele tem que vira 180 para não bugar o codigo e depois que ele passa de 180 tem que virar
			 * -180, isso se deve ao fato de quando se rotaciona o Objeto do cenario, se for para a esquerda a toração
			 * é negativa e se for para a direita a rotação é positiva, então precisa existir uma transição no número 180.
			 */
				if (angulo < 0) {
					/*
				 * Serve para resolver um bug, pois quando o jogador aperta rapidamente a seta da direita e esquerda, o if
				 * lá em cima que impede que o angulo passe do -180 e 180 entra, então é necessario desfazer essa soma de
				 * ajuste de angulo, por que é muito rapido o processo e acaba ajustando o angulo sem ser necessario.
				 */
					if (angulo < -180) {
						angulo += 720;
					}

					angulo -= aceleracaoRotacao * Time.deltaTime;
					chegarAngulo = -180;
					if (angulo < -180) {
						angulo = -180;
						estaRodando = false;
					}
					if (angulo != -180) {
						estaRodando = true;
					}
				} else {
					/*
				 * Serve para resolver um bug, pois quando o jogador aperta rapidamente a seta da direita e esquerda, igual
				 * o mesmo if que esta na desição do angulo negativo
				 */
					if (angulo > 180) {
						angulo -= 720;
					}

					angulo += aceleracaoRotacao * Time.deltaTime;
					chegarAngulo = 180;
					if (angulo > 180) {
						angulo = 180;
						estaRodando = false;
					}
					if (angulo != 180) {
						estaRodando = true;
					}
				}
			} else if (estaRodando) {
				estaRodando = false;
			}
		}
	}

	void CalcularFisica () {
		message.text = "Velocidade: " + aceleracaoAndando.ToString () + " Peso total: " + (massa + massaArma).ToString () + " AnguloY: " + (transform.eulerAngles.y).ToString ();

		//Rotação do Abejide
		if (estaRodando && aceleracaoRotacao < maxAceleracaoRotacao) {
			aceleracaoRotacao += (addNewtonRotacao + aceleracaoAndando * massa) / massa;

			if (aceleracaoRotacao > maxAceleracaoRotacao) {
				aceleracaoRotacao = maxAceleracaoRotacao;
			}
		} else if (!estaRodando) {
			aceleracaoRotacao = 0;
		}

		//Movimentaão do Abejide:
		if (Input.GetKey (KeyCode.LeftShift)) {
			newtonAndando += (addNewtonAndando * 2);

			if (newtonAndando > maxNewtonAndando + maxNewtonCorrendo) {
				newtonAndando = maxNewtonAndando + maxNewtonCorrendo;
			}
		} else {
			if (estaAndando && newtonAndando <= maxNewtonAndando) {
				newtonAndando += addNewtonAndando;

				if (newtonAndando > maxNewtonAndando) {
					newtonAndando = maxNewtonAndando;
				}
			} else if (aceleracaoAndando != 0) {
				newtonAndando -= subNewtonAndando;

				if (newtonAndando < 0) {
					newtonAndando = 0;
				}
			}
		}

		aceleracaoAndando = (newtonAndando / (massa + massaArma));

		if (!olhandoInimigo) {
			gameObject.transform.eulerAngles = new Vector3 (0, angulo, 0);
			gameObject.transform.Translate(0, 0, aceleracaoAndando * Time.deltaTime);
		}

		//Muda a animação do Abejide:
		//Se Abejide estiver se movendo, pode ser que a animação precise ser mudada dependendo da ocasião.
		if (estaAndando) {
			if (aceleracaoAndando <= (maxNewtonAndando + (maxNewtonCorrendo / 2)) / (massa + massaArma)) {
				//Mudar a animação para andando quando Abejide começar a movendo.
				//de correndo para andando.
				if (!abejideAnimator.GetBool ("Andando")) {
					abejideAnimator.SetBool ("Andando", true);
				//Faz a transição da animação correndo para andando.
				} else if (abejideAnimator.GetBool ("Correndo")) {
					abejideAnimator.SetBool ("Correndo", false);
				}
				//Muda a animação para correndo quando Abejide começar a correr.
			} else if (!abejideAnimator.GetBool ("Correndo")) {
				abejideAnimator.SetBool ("Correndo", true);
			}
		} else if (abejideAnimator.GetBool ("Andando") && aceleracaoAndando <= (maxNewtonAndando + (maxNewtonCorrendo / 3)) / (massa + massaArma)) {
			//Muda a animação para andando quando o jogador não estiver mais apertando a tecla de movimento, pois se o jogador começar a correr mais depois parar
			//é preciso trocar da animação correndo para andando, por que dessa forma a animação fica mais suave.
			if (abejideAnimator.GetBool ("Correndo")) {
				abejideAnimator.SetBool ("Correndo", false);
			//Mudar a animação para parado quando Abejide não estiver se movendo.
			} else if (aceleracaoAndando < minPlayAndando) {
				abejideAnimator.SetBool ("Andando", false);
			}
		}
	}

	/// <summary>
	/// Rotacina Abejide ate chegar um determinado angulo que foi passado na variável 'chegarAngulo', se esse número
	/// for negativo tem que rotacionar para a esquerda, pois o menor caminho para chegar no angulo que foi passado é
	/// girando para esse lado, porém se for positivo tem que girar para a direita com o mesmo intuito de achar o menor
	/// caminho, por que se a rotação for negativa quer dizer que o angulo do Abejide esta abaixo de zero e, se a rotação for
	/// positiva quer dizer que o angulo do Abejide esta acima de zero.
	/// </summary>
	/// <param name="negativo">If set to <c>true</c> negativo.</param>
	/// <param name="chegarAngulo">Chegar angulo.</param>
	void RotacionarAbejide () {
		if (angulo != chegarAngulo) {
			estaRodando = true;
		}

		if (chegarAngulo < 0) {
			if (angulo > chegarAngulo) {
				angulo -= aceleracaoRotacao * Time.deltaTime;
				if (angulo < chegarAngulo) {
					angulo = chegarAngulo;
					estaRodando = false;
				}
			} else {
				angulo += aceleracaoRotacao * Time.deltaTime;
				if (angulo > chegarAngulo) {
					angulo = chegarAngulo;
					estaRodando = false;
				}
			}
		} else {
			if (angulo < chegarAngulo) {
				angulo += aceleracaoRotacao * Time.deltaTime;
				if (angulo > chegarAngulo) {
					angulo = chegarAngulo;
					estaRodando = false;
				}
			} else {
				angulo -= aceleracaoRotacao * Time.deltaTime;
				if (angulo < chegarAngulo) {
					angulo = chegarAngulo;
					estaRodando = false;
				}
			}
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

	public void MudarPeso(float gravidade) {
		peso = massa * gravidade;
	}
}