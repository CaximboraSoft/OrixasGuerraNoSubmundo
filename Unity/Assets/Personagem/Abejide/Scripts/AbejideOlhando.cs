using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbejideOlhando : MonoBehaviour {

	public Animator corpoAnimator;
	public Animator peAnimator;
	private AnimatorStateInfo corpoState;

	public bool estaAndandoZ;
	public bool estaAndandoX;
	private bool trasicaoAngulo;

	public float aceleracaoX;
	public float aceleracaoZ;
	public float newtonX; //força(newton) = massa(kg) * aceleração(m/s²)
	public float newtonZ;
	public float maisDevagar;
	public float minPlayAndando = 0.5f; //Só pode disparar a animação de andando quando a velocidade do Abejide for menor que isso.
	private float angulo;
	private float divisorMaxNewtom;

	// Use this for initialization
	void Awake () {
		trasicaoAngulo = false;

		divisorMaxNewtom = 1.5f;
	}
	
	// Update is called once per frame
	void Update () {
		corpoState = corpoAnimator.GetCurrentAnimatorStateInfo (0);

		if (corpoState.IsTag ("Andando") || corpoState.IsTag ("Parado")) {
			MoverAbejide ();
		}
	}

	void MoverAbejide() {
		estaAndandoX = false;
		estaAndandoZ = false;
		trasicaoAngulo = false;

		angulo = transform.eulerAngles.y;
			
		if (Input.GetKey (KeyCode.LeftArrow)) {
			if (angulo > 45 && angulo <= 135) {
				estaAndandoZ = true;
				newtonZ = CalcularFisica (newtonZ, aceleracaoZ, false);
			} else if (angulo > 135 && angulo <= 225) {
				estaAndandoX = true;
				newtonX = CalcularFisica (newtonX, aceleracaoX, true);
			} else if (angulo > 225 && angulo < 315) {
				estaAndandoZ = true;
				newtonZ = CalcularFisica (newtonZ, aceleracaoZ, true);
			} else {
				estaAndandoX = true;
				newtonX = CalcularFisica (newtonX, aceleracaoX, false);
			}
		} 
		if (Input.GetKey (KeyCode.RightArrow)) {
			if (angulo > 45 && angulo <= 135) {
				estaAndandoZ = true;
				newtonZ = CalcularFisica (newtonZ, aceleracaoZ, true);
			} else if (angulo > 135 && angulo <= 225) {
				estaAndandoX = true;
				newtonX = CalcularFisica (newtonX, aceleracaoX, false);
			} else if (angulo > 225 && angulo < 315) {
				estaAndandoZ = true;
				newtonZ = CalcularFisica (newtonZ, aceleracaoZ, false);
			} else {
				estaAndandoX = true;
				newtonX = CalcularFisica (newtonX, aceleracaoX, true);
			}
		} if (Input.GetKey (KeyCode.UpArrow)) {
			if (angulo > 45 && angulo <= 135) {
				estaAndandoX = true;
				newtonX = CalcularFisica (newtonX, aceleracaoX, false);
			} else if (angulo > 135 && angulo <= 225) {
				estaAndandoZ = true;
				newtonZ = CalcularFisica (newtonZ, aceleracaoZ, false);
			} else if (angulo > 225 && angulo < 315) {
				estaAndandoX = true;
				newtonX = CalcularFisica (newtonX, aceleracaoX, true);
			} else {
				estaAndandoZ = true;
				newtonZ = CalcularFisica (newtonZ, aceleracaoZ, true);
			}
		} if (Input.GetKey (KeyCode.DownArrow)) {
			if (angulo > 45 && angulo <= 135) {
				estaAndandoX = true;
				newtonX = CalcularFisica (newtonX, aceleracaoX, true);
			} else if (angulo > 135 && angulo <= 225) {
				estaAndandoZ = true;
				newtonZ = CalcularFisica (newtonZ, aceleracaoZ, true);
			} else if (angulo > 225 && angulo < 315) {
				estaAndandoX = true;
				newtonX = CalcularFisica (newtonX, aceleracaoX, false);
			} else {
				estaAndandoZ = true;
				newtonZ = CalcularFisica (newtonZ, aceleracaoZ, false);
			}
		}

		if (!trasicaoAngulo) {
			//Desacelera o Abejie quando quando o jogador não estiver mais apertando a tecla que movimenta no eixo Z.
			if (!estaAndandoZ && aceleracaoZ != 0) {
				newtonZ = Desacelerar (newtonZ, aceleracaoZ);
			}
			if (!estaAndandoX && aceleracaoX != 0) {
				newtonX = Desacelerar (newtonX, aceleracaoX);
			}
		}

		aceleracaoX = (newtonX / (gameObject.GetComponent<DadosForcaResultante>().PegarMassaTotal ()));
		aceleracaoZ = (newtonZ / (gameObject.GetComponent<DadosForcaResultante>().PegarMassaTotal ()));
		gameObject.transform.Translate((aceleracaoX / maisDevagar) * Time.deltaTime, 0, (aceleracaoZ / maisDevagar) * Time.deltaTime);

		//-----------------------------------------------------------//


		if (estaAndandoZ) {
			if (newtonZ > 0) {
				corpoAnimator.SetInteger ("DirecaoAtaque", 1);
			} else {
				corpoAnimator.SetInteger ("DirecaoAtaque", 3);
			}
		} else if (estaAndandoX) {
			if (newtonX > 0) {
				corpoAnimator.SetInteger ("DirecaoAtaque", 2);
			} else {
				corpoAnimator.SetInteger ("DirecaoAtaque", 4);
			}
		} else {
			corpoAnimator.SetInteger ("DirecaoAtaque", 0);
		}

		if (aceleracaoX != 0) {
			corpoAnimator.SetFloat ("AceleracaoAndando", Mathf.Abs(aceleracaoX));
		} else {
			corpoAnimator.SetFloat ("AceleracaoAndando", Mathf.Abs(aceleracaoZ));
		}

		peAnimator.SetBool ("Atacando", true);
		peAnimator.SetInteger ("DirecaoAtaque", corpoAnimator.GetInteger("DirecaoAtaque"));
		peAnimator.SetFloat ("AceleracaoAndando", corpoAnimator.GetFloat("AceleracaoAndando"));

		//-----------------------------------------------------------//
	}

	float CalcularFisica (float newton, float aceleracao, bool positivo) {
		if (positivo) {
			if (newton <= GetComponent<DadosForcaResultante> ().maxNewtonAndando) {
				newton += GetComponent<DadosForcaResultante> ().addNewtonAndando;

				if (newton > GetComponent<DadosForcaResultante> ().maxNewtonAndando / divisorMaxNewtom) {
					newton = GetComponent<DadosForcaResultante> ().maxNewtonAndando / divisorMaxNewtom;
				}
			}

			if (newton < 0) {
				newton += (GetComponent<DadosForcaResultante> ().addNewtonAndando * divisorMaxNewtom);
			}
		} else {
			if (newton >= -GetComponent<DadosForcaResultante> ().maxNewtonAndando / divisorMaxNewtom) {
				newton -= GetComponent<DadosForcaResultante> ().addNewtonAndando / divisorMaxNewtom;

				if (newton < -GetComponent<DadosForcaResultante> ().maxNewtonAndando / divisorMaxNewtom) {
					newton = -GetComponent<DadosForcaResultante> ().maxNewtonAndando / divisorMaxNewtom;
				}
			}

			if (newton > 0) {
				newton -= (GetComponent<DadosForcaResultante> ().addNewtonAndando * 2);
			}
		}

		//aceleracaoAndando = (newtonAndando / (gameObject.GetComponent<AbejideAtaque>().PegarMassaTotal ()));
		return newton;
	}

	float Desacelerar(float newton, float aceleracao) {
		if (aceleracao > 0) {
			newton -= GetComponent<DadosForcaResultante> ().subNewtonAndando;

			if (newton < 0) {
				newton = 0;
			}
		} else {
			newton += GetComponent<DadosForcaResultante> ().subNewtonAndando;

			if (newton > 0) {
				newton = 0;
			}
		}

		return newton;
	}
}


/*
		//Faz o personagem andar um pouco para o lado que o jogador estiver apertando segundo a seta quando ele estiver atacando.
if (estaAtaqueParado) {
	DirecarAtaque (transform.eulerAngles.y);

	if (velhaDirecaoAtaque != direcaoAtaque) {
		peAnimator.SetFloat ("AceleracaoAndando", 0);
		GetComponent<DadosForcaResultante> ().MudarNewtonAndando (0);
	}

	//Se estiver sendo um ataque de movimento vai entrar neste <if>.
	if (direcaoAtaque != 0) {
		//Então a aceleração de movimento vai ser atribuita para o <animator controller>, pos a animação de andando é disparada comforme a velocidade do Abejide.
		GetComponent<DadosForcaResultante> ().AddNewtonAndando ();
		peAnimator.SetFloat ("AceleracaoAndando", (GetComponent<DadosForcaResultante> ().PegarAceleracaoAndando ()));
		//Caso não seja uma animação de ataque andando, o aceleração do <animator controller> é zerada.

		//Faz a movimentação de acordo com a seta e angulo que foi pegada no metodo <DirecarAtaque>.
		switch (direcaoAtaque) {
		case 1: //Move para frente.
			transform.Translate (0, 0, peAnimator.GetFloat ("AceleracaoAndando") * Time.deltaTime);
			break;
		case 2: //Move para direita.
			transform.Translate (peAnimator.GetFloat ("AceleracaoAndando") * Time.deltaTime, 0, 0);
			break;
		case 3: //Move para tras.
			transform.Translate (0, 0, -peAnimator.GetFloat ("AceleracaoAndando") * Time.deltaTime);
			break;
		case 4: //Move para esquerda.
			transform.Translate (-peAnimator.GetFloat ("AceleracaoAndando") * Time.deltaTime, 0, 0);
			break;
		}
	} else {
		GetComponent<DadosForcaResultante> ().SubNewtonAndando (1);
		peAnimator.SetFloat ("AceleracaoAndando", (GetComponent<DadosForcaResultante> ().PegarAceleracaoAndando ()));
	}
	//Sistema para o Abejide correr um pouco com a espada apos ele apertar para atacar quando o mesmo estiver andando.*/