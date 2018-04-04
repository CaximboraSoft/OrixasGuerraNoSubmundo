using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Abejide : MonoBehaviour {

	private Animator abejideAnimator;
	public Text message;
	public Camera abejideCamera;
	public GameObject gameObjectAbejide;
	public GameObject cameraPosiiton;
	public GameObject[] armas;

	public bool estaAndando;
	public bool estaRodando;

	public float velocidadeAndando;
	public float addVelocidadeAndando;
	public float subVelocidadeAndando;
	public float maxVelocidadeAndando;
	public float velocidadeCorrendo;
	public float pararVezesSub = 3; //Esse vai ser quantas vezes mais rapido o Abejide vai para de correr.
	public float minPlayAndando = 0.5f; //Só pode disparar a animação de andando quando a velocidade do Abejide for menor que isso.

	public float velocidadeRotacao;
	private int termosPaRoacao;
	public float addVelocidadeRotacao;
	public float maxVelocidadeRotacao;

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

		tempoCombo = 0;
		velocidadeAndando = 0;
		velocidadeRotacao = 0;
		termosPaRoacao = 0;
		distanceCameraY = gameObject.transform.position.y - abejideCamera.transform.position.y;
		distanceCameraZ = gameObject.transform.position.z - abejideCamera.transform.position.z;

		abejideAnimator = gameObjectAbejide.GetComponent<Animator>();

		armas [0].GetComponent<Renderer> ().enabled = true;
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
		}
		else if (Input.GetKey (KeyCode.RightArrow)) {
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
		}
		else if (Input.GetKey (KeyCode.UpArrow)) {
			estaAndando = true;

			chegarAngulo = 0;
			RotacionarAbejide ();
		}
		else if (Input.GetKey (KeyCode.DownArrow)) {
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

				angulo -= velocidadeRotacao * Time.deltaTime;
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

				angulo += velocidadeRotacao * Time.deltaTime;
				chegarAngulo = 180;
				if (angulo > 180) {
					angulo = 180;
					estaRodando = false;
				}
				if (angulo != 180) {
					estaRodando = true;
				}
			}
		}
		else if (estaRodando) {
			estaRodando = false;
		}

		gameObject.transform.eulerAngles = new Vector3 (0, angulo, 0);
		gameObject.transform.Translate(0, 0, velocidadeAndando * Time.deltaTime);
	}

	void CalcularFisica () {
		if (true) {
			//Rotação do Abejide
			if (estaRodando) {
				velocidadeRotacao *= (termosPaRoacao * Time.deltaTime);
				velocidadeRotacao += ((addVelocidadeRotacao * velocidadeAndando) * Time.deltaTime);
				termosPaRoacao++;
				if (velocidadeRotacao > maxVelocidadeRotacao) {
					velocidadeRotacao = maxVelocidadeRotacao;
				}
			} else if (!estaRodando) {
				velocidadeRotacao = 0;
				termosPaRoacao = 0;
			}

			//Aceleração do Abejide
			if (estaAndando && Input.GetKey (KeyCode.LeftShift)) {
				if (estaAndando && velocidadeAndando < maxVelocidadeAndando + velocidadeCorrendo) {
					velocidadeAndando += ((addVelocidadeAndando * (velocidadeCorrendo / 2)) * Time.deltaTime);

					if (velocidadeAndando > maxVelocidadeAndando + velocidadeCorrendo) {
						velocidadeAndando = maxVelocidadeAndando + velocidadeCorrendo;
					}
				}
			} else {
				if (estaAndando && velocidadeAndando < maxVelocidadeAndando) {
					velocidadeAndando += (addVelocidadeAndando * Time.deltaTime);

					if (velocidadeAndando > maxVelocidadeAndando) {
						velocidadeAndando = maxVelocidadeAndando;
					}
				} else if (velocidadeAndando != 0) {
					if (velocidadeAndando > maxVelocidadeAndando) {
						velocidadeAndando -= (subVelocidadeAndando * Time.deltaTime);
					} else {
						velocidadeAndando -= (subVelocidadeAndando * Time.deltaTime);
					}

					if (velocidadeAndando < 0) {
						velocidadeAndando = 0;
					}
				}
			}
		}

		//Se Abejide estiver se movendo, pode ser que a animação precise ser mudada dependendo da ocasião.
		if (estaAndando) {
			if (velocidadeAndando <= maxVelocidadeAndando + (velocidadeCorrendo / 2)) {
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
		} else if (abejideAnimator.GetBool ("Andando") && velocidadeAndando <= maxVelocidadeAndando + (velocidadeCorrendo / 1.2)) {
			//Muda a animação para andando quando o jogador não estiver mais apertando a tecla de movimento, pois se o jogador começar a correr mais depois parar
			//é preciso trocar da animação correndo para andando, por que dessa forma a animação fica mais suave.
			if (abejideAnimator.GetBool ("Correndo")) {
				abejideAnimator.SetBool ("Correndo", false);
			//Mudar a animação para parado quando Abejide não estiver se movendo.
			} else if (velocidadeAndando < minPlayAndando) {
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
				angulo -= velocidadeRotacao * Time.deltaTime;
				if (angulo < chegarAngulo) {
					angulo = chegarAngulo;
					estaRodando = false;
				}
			} else {
				angulo += velocidadeRotacao * Time.deltaTime;
				if (angulo > chegarAngulo) {
					angulo = chegarAngulo;
					estaRodando = false;
				}
			}
		} else {
			if (angulo < chegarAngulo) {
				angulo += velocidadeRotacao * Time.deltaTime;
				if (angulo > chegarAngulo) {
					angulo = chegarAngulo;
					estaRodando = false;
				}
			} else {
				angulo -= velocidadeRotacao * Time.deltaTime;
				if (angulo < chegarAngulo) {
					angulo = chegarAngulo;
					estaRodando = false;
				}
			}
		}
	}

	void TrocarDeArma (int novaArma) {
		armas [abejideAnimator.GetInteger("ArmaAtual")].GetComponent<Renderer> ().enabled = false;
		abejideAnimator.SetInteger("ArmaAtual", novaArma);
		armas [novaArma].GetComponent<Renderer> ().enabled = true;
		abejideAnimator.SetTrigger ("TrocarDeArma");
	}
}