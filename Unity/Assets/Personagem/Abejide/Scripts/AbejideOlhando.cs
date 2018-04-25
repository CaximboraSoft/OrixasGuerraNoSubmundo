﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbejideOlhando : MonoBehaviour {

	private Animator abejideAnimator;
	private AnimatorStateInfo animacaoState;

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

	private string andando;

	// Use this for initialization
	void Awake () {
		trasicaoAngulo = false;

		abejideAnimator = GameObject.Find ("AbejideMesh").GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		animacaoState = abejideAnimator.GetCurrentAnimatorStateInfo (0);

		if (animacaoState.IsTag ("Andando") || animacaoState.IsTag ("Parado")) {
			MoverAbejide ();
		}
	}

	void MoverAbejide() {
		estaAndandoX = false;
		estaAndandoZ = false;
		trasicaoAngulo = false;

		angulo = transform.eulerAngles.y;

		if (Input.GetKey (KeyCode.UpArrow)) {
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
		}  if (Input.GetKey (KeyCode.RightArrow)) {
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
		} if (Input.GetKey (KeyCode.LeftArrow)) {
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

		if (!trasicaoAngulo) {
			//Desacelera o Abejie quando quando o jogador não estiver mais apertando a tecla que movimenta no eixo Z.
			if (!estaAndandoZ && aceleracaoZ != 0) {
				newtonZ = Desacelerar (newtonZ, aceleracaoZ);
			}
			if (!estaAndandoX && aceleracaoX != 0) {
				newtonX = Desacelerar (newtonX, aceleracaoX);
			}
		}

//		aceleracaoX = (newtonX / (gameObject.GetComponent<DadosForcaResultante>().PegarMassaTotal ()));
//		aceleracaoZ = (newtonZ / (gameObject.GetComponent<DadosForcaResultante>().PegarMassaTotal ()));
		gameObject.transform.Translate((aceleracaoX / maisDevagar) * Time.deltaTime, 0, (aceleracaoZ / maisDevagar) * Time.deltaTime);

		//-----------------------------------------------------------//

		if (newtonZ > 0) {
			abejideAnimator.SetBool ("AndandoParaTras", false);

			andando = "AndandoParaFrente";
		} else if (newtonZ < 0) {
			abejideAnimator.SetBool ("AndandoParaFrente", false);

			andando = "AndandoParaTras";
		}
		if (estaAndandoZ) {
			abejideAnimator.SetBool ("AndandoNegativo", false);
			abejideAnimator.SetBool ("AndandoPositivo", false);

			if (!abejideAnimator.GetBool (andando)) {
				abejideAnimator.SetBool (andando, true);
			}
		} else {
			abejideAnimator.SetBool ("AndandoParaFrente", false);
			abejideAnimator.SetBool ("AndandoParaTras", false);
		}

		//-----------------------------------------------------------//

		if (newtonX > 0) {
			abejideAnimator.SetBool ("AndandoNegativo", false);

			andando = "AndandoPositivo";
		} else if (newtonX < 0) {
			abejideAnimator.SetBool ("AndandoPositivo", false);

			andando = "AndandoNegativo";
		}
		if (estaAndandoX) {
			abejideAnimator.SetBool ("AndandoParaFrente", false);
			abejideAnimator.SetBool ("AndandoParaTras", false);

			if (!abejideAnimator.GetBool (andando)) {
				abejideAnimator.SetBool (andando, true);
			}
		} else {
			abejideAnimator.SetBool ("AndandoPositivo", false);
			abejideAnimator.SetBool ("AndandoNegativo", false);
		}

		//-----------------------------------------------------------//
	}

	float CalcularFisica (float newton, float aceleracao, bool positivo) {
		if (positivo) {
			if (newton <= GetComponent<DadosForcaResultante> ().maxNewtonAndando) {
				newton += GetComponent<DadosForcaResultante> ().addNewtonAndando;

				if (newton > GetComponent<DadosForcaResultante> ().maxNewtonAndando) {
					newton = GetComponent<DadosForcaResultante> ().maxNewtonAndando;
				}
			}

			if (newton < 0) {
				newton += (GetComponent<DadosForcaResultante> ().addNewtonAndando * 2);
			}
		} else {
			if (newton >= -GetComponent<DadosForcaResultante> ().maxNewtonAndando) {
				newton -= GetComponent<DadosForcaResultante> ().addNewtonAndando;

				if (newton < -GetComponent<DadosForcaResultante> ().maxNewtonAndando) {
					newton = -GetComponent<DadosForcaResultante> ().maxNewtonAndando;
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
