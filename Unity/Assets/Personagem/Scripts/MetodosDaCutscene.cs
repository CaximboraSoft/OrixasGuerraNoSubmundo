using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetodosDaCutscene : MonoBehaviour {

	public Transform seta;
	private Transform posicao;
	private float rotacao;

	private bool cima;
	private bool estaAtuando;
	private bool sentidoHorario;
	private bool chegouNoAngulo;
	private bool correndo;
	private bool manterNewtonAndando;
	private bool manterNewtonRodando;

	private float distancia;
	private float newton;
	private float newtonAndando;
	private float newtonRodando;
	private float hipotenusa;
	private float cateto1;
	private float cateto2;
	private float angulo;
	private float anguloInicial;

	/*
	 * 0 faz nada.
	 * 1 muda a posição
	 * 11 mover para cima.
	 * 2 muda a rotação
	 * 21 faz o ator apontar para um objeto
	 */
	private int tipoDeAtuacao;

	// Use this for initialization
	void Start () {
		newtonAndando = 0;
		newtonRodando = 0;
		tipoDeAtuacao = 0;

		estaAtuando = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (estaAtuando) {
			switch (tipoDeAtuacao) {
			case 1:
				MudarPosicaoAndando ();
				break;
			case 11:
				MoverNoEixoY ();
				break;
			case 2:
				MudarRotacao ();
				break;
			case 21:
				ApontarParaObjeto ();
				break;
			}
		}
	}

	private void MoverNoEixoY() {
		newton += GetComponent<DadosForcaResultante> ().addNewtonAndando;
		if (correndo) {
			if (newton > GetComponent<DadosForcaResultante> ().maxNewtonCorrendo) {
				newton = GetComponent<DadosForcaResultante> ().maxNewtonCorrendo;
			}
		} else {
			if (newton > GetComponent<DadosForcaResultante> ().maxNewtonAndando) {
				newton = GetComponent<DadosForcaResultante> ().maxNewtonAndando;
			}
		}

		if (cima) {
			transform.Translate (0, (newton / GetComponent<DadosForcaResultante> ().PegarMassaTotal ()) * Time.deltaTime, 0);

			if (transform.position.y > distancia) {
				estaAtuando = false;
			}
		} else {
			transform.Translate (0, -(newton / GetComponent<DadosForcaResultante> ().PegarMassaTotal ()) * Time.deltaTime, 0);

			if (transform.position.y > distancia) {
				estaAtuando = false;
			}
		}
	}

	/// <summary>
	/// Faz o ator apontar para o objeto que foi passado no método <ComecarAtuacaoOlharParaObjeto>
	/// </summary>
	private void ApontarParaObjeto() {
		//Faz o personagem rotacionar.
		if (!chegouNoAngulo) {
			seta.LookAt (new Vector3 (posicao.position.x, seta.position.y, posicao.position.z));
			rotacao = seta.eulerAngles.y - transform.eulerAngles.y;
			if (rotacao < 0) {
				rotacao += 360;
			}
			RodarAtor ();
		} else {
			transform.LookAt (new Vector3 (posicao.position.x, transform.position.y, posicao.position.z));
		}
	}

	/// <summary>
	/// Faz o ator rodar até chegar no angulo que foi passado no método <ComecarAtuacaoRotacao>
	/// </summary>
	private void MudarRotacao() {
		RodarAtor ();

		if (chegouNoAngulo) {
			estaAtuando = false;
		}
	}

	private void MudarPosicaoAndando () {
		//Faz o ator andar.
		newtonAndando += GetComponent<DadosForcaResultante> ().addNewtonAndando;
		if (correndo) {
			if (newtonAndando > GetComponent<DadosForcaResultante> ().maxNewtonCorrendo) {
				newtonAndando = GetComponent<DadosForcaResultante> ().maxNewtonCorrendo;
			}
		} else {
			if (newtonAndando > GetComponent<DadosForcaResultante> ().maxNewtonAndando) {
				newtonAndando = GetComponent<DadosForcaResultante> ().maxNewtonAndando;
			}
		}

		transform.Translate (0, 0, (newtonAndando / GetComponent<DadosForcaResultante> ().PegarMassaTotal ()) * Time.deltaTime);

		cateto1 = transform.position.x - posicao.position.x;
		cateto2 = transform.position.z - posicao.position.z;
		hipotenusa = Mathf.Sqrt (Mathf.Pow (cateto1, 2) + Mathf.Pow (cateto2, 2));

		if (hipotenusa < 0.4f) {
			if (!manterNewtonAndando) {
				newtonAndando = 0;
			}
			if (!manterNewtonRodando) {
				newtonRodando = 0;
			}
			estaAtuando = false;
		}

		//Faz o personagem rotacionar.
		if (!chegouNoAngulo) {
			seta.LookAt (new Vector3 (posicao.position.x, seta.position.y, posicao.position.z));
			rotacao = seta.eulerAngles.y - transform.eulerAngles.y;
			if (rotacao < 0) {
				rotacao += 360;
			}
			RodarAtor ();
		}
	}

	private void RodarAtor () {
		newtonRodando += GetComponent<DadosForcaResultante> ().addNewtonRodando;
		if (newtonRodando > GetComponent<DadosForcaResultante> ().maxNewtonRodando) {
			newtonRodando = GetComponent<DadosForcaResultante> ().maxNewtonRodando;
		}

		if (sentidoHorario) {
			angulo += ((newtonRodando / GetComponent<DadosForcaResultante> ().PegarMassaTotal ()) * 30) * Time.deltaTime;

			if (angulo + rotacao > 360) {
				chegouNoAngulo = true;
			}
		} else {
			angulo -= ((newtonRodando / GetComponent<DadosForcaResultante> ().PegarMassaTotal ()) * 30) * Time.deltaTime;

			if (angulo + rotacao < 0) {
				chegouNoAngulo = true;
			}
		}

		transform.eulerAngles = new Vector3 (transform.eulerAngles.x, anguloInicial + angulo, transform.eulerAngles.z);
	}

	/// <summary>
	/// Método que prepara para o ator olhar para um objeto.
	/// </summary>
	/// <param name="posicao">Posicao.</param>
	/// <param name="sentidoHorario">If set to <c>true</c> sentido horario.</param>
	/// <param name="manterNewtonRodando">If set to <c>true</c> manter newton rodando.</param>
	public void ComecarAtuacaoOlharParaObjeto(Transform posicao, bool sentidoHorario, bool manterNewtonRodando) {
		this.posicao = posicao;
		this.sentidoHorario = sentidoHorario;
		this.manterNewtonRodando = manterNewtonRodando;

		anguloInicial = transform.eulerAngles.y;
		angulo = 0;
		tipoDeAtuacao = 21;
		chegouNoAngulo = false;
		estaAtuando = true;
	}

	/// <summary>
	/// Método que prepara o ator para rotacionar até o angulo que foi passado.
	/// </summary>
	/// <param name="anguloChegar">Angulo chegar.</param>
	/// <param name="sentidoHorario">If set to <c>true</c> sentido horario.</param>
	public void ComecarAtuacaoRotacao (float anguloChegar, bool sentidoHorario, bool manterNewtonRodando) {
		this.manterNewtonRodando = manterNewtonRodando;
		this.sentidoHorario = sentidoHorario;
		rotacao = transform.eulerAngles.y - anguloChegar;
		if (this.rotacao < 0) {
			this.rotacao += 360;
		}

		angulo = 0;
		anguloInicial = transform.eulerAngles.y;
		tipoDeAtuacao = 2;
		chegouNoAngulo = false;
		estaAtuando = true;
	}

	/// <summary>
	/// Método que prepara o ator para começar a sua atuação da mudança de posição no cenario.
	/// </summary>
	/// <param name="posicao">Posicao.</param>
	/// <param name="sentidoHorario">If set to <c>true</c> sentido horario.</param>
	/// <param name="correndo">If set to <c>true</c> correndo.</param>
	/// <param name="manterNewtonAndando">If set to <c>true</c> manter newton andando.</param>
	/// <param name="manterNewtonRodando">If set to <c>true</c> manter newton rodando.</param>
	public void ComecarAtuacaoPosicao (Transform posicao, bool sentidoHorario, bool correndo, bool manterNewtonAndando, bool manterNewtonRodando) {
		this.posicao = posicao;
		this.sentidoHorario = sentidoHorario;
		this.correndo = correndo;
		this.manterNewtonAndando = manterNewtonAndando;
		this.manterNewtonRodando = manterNewtonRodando;

		anguloInicial = transform.eulerAngles.y;
		angulo = 0;
		tipoDeAtuacao = 1;
		chegouNoAngulo = false;
		estaAtuando = true;
	}

	public void ComecarAtuacaoMoverNoEixoY(float distancia, bool cima, bool correndo) {
		this.cima = cima;
		this.correndo = correndo;

		if (cima) {
			this.distancia = transform.position.y + distancia;
		} else {
			this.distancia = transform.position.y - distancia;
		}

		newton = 0;
		tipoDeAtuacao = 11;
		estaAtuando = true;
	}

	/// <summary>
	/// Pega o valor da variável <estaAtuando> desse script, ela serve para informar se o ator ainda não acabou sua rotina que
	/// lhe foi passada.
	/// </summary>
	/// <returns><c>true</c>, if esta atuando was pegared, <c>false</c> otherwise.</returns>
	public bool PegarEstaAtuando () {
		return estaAtuando;
	}

	/// <summary>
	/// Pega o valor da variável <newtonAndando>.
	/// </summary>
	/// <returns>The newton andando.</returns>
	public float PegarNewtonAndando () {
		return newtonAndando;
	}

	public void MudarEstaAtuando (bool valor) {
		estaAtuando = valor;
	}
}
