using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Aqui tem a mecânica de fazer o Abejide se mover e correr, porém essa movimentação não
 * é dele olhando para o inimigo, é a movimentação de rotação.
 * Também tem as trasições das animações de adando para correndo e vise versa.
 */

public class AbejideAndando : MonoBehaviour {

	public Text message;
	public Animator corpoAnimator;
	public Animator peAnimator;
	private AnimatorStateInfo corpoState;
	private AnimatorStateInfo peState;

	//private bool estaCorrendo;
	private bool estaAndando;
	private bool estaRodando;

	public float minPlayAndando = 0.5f; //Só pode disparar a animação de andando quando a velocidade do Abejide for menor que isso.

	private float angulo;
	private float chegarAngulo;

	private float diferenca;

	// Use this for initialization
	void Awake () {
		estaRodando = false;
		//estaCorrendo = false;

		corpoAnimator = GameObject.Find ("AbejideMesh").GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () {
		corpoState = corpoAnimator.GetCurrentAnimatorStateInfo (0);
		peState = peAnimator.GetCurrentAnimatorStateInfo (0);

		estaAndando = false;
		if ((corpoState.IsTag ("Andando") || corpoState.IsTag ("Parado")) && GetComponent<AbejideAtaque> ().TempoComboMaiorQueLimite ()) {
			MoverRodarAbejide ();
			CalcularFisica (1);

			//Sicroniza a animação do pé com o corpo caso eles não estejam mais alinhados.
			diferenca = Mathf.Abs(corpoState.normalizedTime - peState.normalizedTime);
			if (diferenca > 0.15f && corpoState.IsTag ("Andando")) {
				peAnimator.Play ("Andando", 0 , corpoState.normalizedTime);
			}
		} else if (GetComponent<AbejideAtaque> ().EstaAtacandoAndando ()) {
			MoverRodarAbejide ();
			CalcularFisica (2);
		}
	}

	void MoverRodarAbejide () {
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

			angulo -= GetComponent<DadosForcaResultante> ().PegarAceleracaoRodando ();
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

			angulo += GetComponent<DadosForcaResultante> ().PegarAceleracaoRodando ();
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

	void CalcularFisica (float dividirMaxNewtom) {
		//Rotação do Abejide
		if (estaRodando) {
			GetComponent<DadosForcaResultante> ().AddNewtonRodando ();
		} else if (!estaRodando) {
			GetComponent<DadosForcaResultante> ().MudarNewtonRodando (0);
		}

		//Movimentção correndo.
		if (Input.GetKey (KeyCode.LeftShift)) {
			GetComponent<DadosForcaResultante> ().AddNewtonCorrendo (1.2f);
		//Movimentção andando.
		} else {
			if (estaAndando) {
				GetComponent<DadosForcaResultante> ().AddNewtonAndando (dividirMaxNewtom);
			//Desaceleração.
			} else if (corpoAnimator.GetFloat ("AceleracaoAndando") != 0) {
				GetComponent<DadosForcaResultante> ().SubNewtonAndando (1);
			}
		}

		corpoAnimator.SetFloat ("AceleracaoAndando",  (GetComponent<DadosForcaResultante>().PegarAceleracaoAndando ()));
		peAnimator.SetFloat ("AceleracaoAndando",  (GetComponent<DadosForcaResultante>().PegarAceleracaoAndando ()));

		transform.eulerAngles = new Vector3 (0, angulo, 0);
		transform.Translate(0, 0, corpoAnimator.GetFloat ("AceleracaoAndando") * Time.deltaTime);
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
				angulo -= GetComponent<DadosForcaResultante> ().PegarAceleracaoRodando ();
				if (angulo < chegarAngulo) {
					angulo = chegarAngulo;
					estaRodando = false;
				}
			} else {
				
				angulo += GetComponent<DadosForcaResultante> ().PegarAceleracaoRodando ();
				if (angulo > chegarAngulo) {
					angulo = chegarAngulo;
					estaRodando = false;
				}
			}
		} else {
			if (angulo < chegarAngulo) {
				angulo += GetComponent<DadosForcaResultante> ().PegarAceleracaoRodando ();
				if (angulo > chegarAngulo) {
					angulo = chegarAngulo;
					estaRodando = false;
				}
			} else {
				angulo -= GetComponent<DadosForcaResultante> ().PegarAceleracaoRodando ();
				if (angulo < chegarAngulo) {
					angulo = chegarAngulo;
					estaRodando = false;
				}
			}
		}
		
		message.text = "angulo: " + angulo.ToString () + " | chegar: " + chegarAngulo.ToString ();
	}

	public void MudarAngulo (float valor) {
		angulo = valor;
	}
}