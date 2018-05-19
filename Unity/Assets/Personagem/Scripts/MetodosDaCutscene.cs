using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MetodosDaCutscene : MonoBehaviour {

	public Transform boca;
	public Transform rostoMostrar;
	private Transform objeto;
	public Transform seta;
	private Transform posicao;
	public float[] rotacoes;
	private float rotacao;

	public bool mudarTexto; 
	private bool cima;
	private bool estaAtuando;
	private bool sentidoHorario;
	private bool chegouNoAngulo;
	private bool correndo;
	private bool manterNewtonAndando;
	private bool manterNewtonRodando;
	private bool acabouAtuacao;
	private bool eixoX;
	private bool eixoY;
	private bool eixoZ;
	private bool mostrarEsteRosto;

	private float distanciaX;
	private float distanciaY;
	private float distanciaZ;
	private float distancia;
	private float newton;
	//private float newtonRodando;
	private float hipotenusa;
	private float cateto1;
	private float cateto2;
	private float angulo;
	private float anguloInicial;
	private float tempoEspera;
	private float maxTempoEspera;
	private float vezes;
	public float tempoMudarTexto;
	public float maxTempoMudarTexto;
	public float maxTempoVidaTexto;

	/*
	 * 0 faz nada.
	 * 1 muda a posição
	 *   11 mover para cima.
	 *   12 ator segue outro ator, mesma posições <x, y, z> do outro ator.
	 *   13 move ator de lado;
	 *   14 mover nos eixos <x, z>
	 * 2 muda a rotação
	 *   21 faz o ator apontar para um objeto
	 */
	private int tipoDeAtuacao;
	public int indicePosicoes;
	public int indiceRotacoes;
	public int ato;

	public string nome;
	public string menssagem;
	public string posicoesNome;

	// Use this for initialization
	void Start () {
		ato = 0;
		newton = 0;
		tipoDeAtuacao = 0;
		indicePosicoes = 0;
		indiceRotacoes = -1;
		tempoMudarTexto = 0;
		maxTempoMudarTexto = 0;

		objeto = GameObject.Find(posicoesNome + " (" + indicePosicoes.ToString () + ")").GetComponent<Transform> ();
		transform.position = objeto.position;
		transform.eulerAngles = objeto.eulerAngles;

		estaAtuando = false;
		mudarTexto = false;
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
			case 12:
				SeguirOutroAtor ();
				break;
			case 13:
				MudarPosicaoDeLado ();
				break;
			case 14:
				MudarPosicaoNosEixosXZ ();
				break;
			case 2:
				MudarRotacao ();
				break;
			case 21:
				ApontarParaObjeto ();
				break;
			}
		}

		//Muda a mensagem para a nova que foi passada após seu tempo de espera acabar.
		if (mudarTexto) {
			tempoMudarTexto += Time.deltaTime;
			if (tempoMudarTexto >= maxTempoMudarTexto) {
				boca.GetComponent<Text> ().text = menssagem;
				mudarTexto = false;
				boca.GetComponent<Conversas> ().nome.text = nome;
				boca.GetComponent<Conversas> ().conversas.enabled = true;

				if (mostrarEsteRosto) {
					boca.GetComponent<Conversas> ().MudarRostoMostrar (rostoMostrar);
					boca.GetComponent<Conversas> ().MudarTempoLimparTexto(maxTempoVidaTexto);
				}
			}
		}
	}

	private void MudarPosicaoNosEixosXZ () {
		
	}

	private void SeguirOutroAtor () {
		if (eixoX) {
			transform.position = new Vector3 (objeto.position.x - distanciaX, transform.position.y, transform.position.z);
		}
		if (eixoY) {
			transform.position = new Vector3 (transform.position.x, objeto.position.y - distanciaY, transform.position.z);
		}
		if (eixoZ) {
			transform.position = new Vector3 (transform.position.x, transform.position.y, objeto.position.z - distanciaZ);
		}
	}

	private void MoverNoEixoY() {
		if (!acabouAtuacao) {
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
				transform.Translate (0, (newton / GetComponent<DadosForcaResultante> ().PegarMassaTotal () * vezes) * Time.deltaTime, 0);

				if (transform.position.y > distancia) {
					acabouAtuacao = true;
				}
			} else {
				transform.Translate (0, -(newton / GetComponent<DadosForcaResultante> ().PegarMassaTotal () * vezes) * Time.deltaTime, 0);

				if (transform.position.y < distancia) {
					acabouAtuacao = true;
				}
			}
		} else {
			if (tempoEspera >= maxTempoEspera) {
				if (!manterNewtonAndando) {
					newton = 0;
				}
				estaAtuando = false;
			} else {
				tempoEspera += Time.deltaTime;
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
		if (!acabouAtuacao) {
			RodarAtor ();

			if (chegouNoAngulo) {
				transform.eulerAngles = new Vector3 (transform.eulerAngles.x, rotacoes[indiceRotacoes], transform.eulerAngles.z);
				acabouAtuacao = true;
			}
		} else {
			if (tempoEspera >= maxTempoEspera) {
				estaAtuando = false;
			} else {
				tempoEspera += Time.deltaTime;
			}
		}
	}

	private void MudarPosicaoDeLado () {
		if (!acabouAtuacao) {
			//Faz o ator andar.

			if (correndo) {
				GetComponent<DadosForcaResultante> ().AddNewtonCorrendo (2);
			} else {
				GetComponent<DadosForcaResultante> ().AddNewtonAndando (1);
			}

			if (transform.position.x < posicao.position.x) {
				transform.Translate (-(GetComponent<DadosForcaResultante> ().PegarAceleracaoAndando ()) * Time.deltaTime, 0, 0);
			} else {
				transform.Translate ((GetComponent<DadosForcaResultante> ().PegarAceleracaoAndando ()) * Time.deltaTime, 0, 0);
			}

			cateto1 = transform.position.x - posicao.position.x;
			cateto2 = transform.position.z - posicao.position.z;
			hipotenusa = Mathf.Sqrt (Mathf.Pow (cateto1, 2) + Mathf.Pow (cateto2, 2));

			if (hipotenusa < 0.4f) {
				if (!manterNewtonAndando) {
					GetComponent<DadosForcaResultante> ().MudarNewtonAndando (0);
				}
				if (!manterNewtonRodando) {
					GetComponent<DadosForcaResultante> ().MudarNewtonRodando (0);
				}
				acabouAtuacao = true;
			}
				
		} else {
			if (tempoEspera >= maxTempoEspera) {
				estaAtuando = false;
			} else {
				tempoEspera += Time.deltaTime;
			}
		}
	}

	private void MudarPosicaoAndando () {
		if (!acabouAtuacao) {
			//Faz o ator andar.
			if (correndo) {
				GetComponent<DadosForcaResultante> ().AddNewtonCorrendo (2);
			} else {
				GetComponent<DadosForcaResultante> ().AddNewtonAndando (1);
			}

			transform.Translate (0, 0, (GetComponent<DadosForcaResultante> ().PegarAceleracaoAndando ()) * Time.deltaTime);

			cateto1 = transform.position.x - posicao.position.x;
			cateto2 = transform.position.z - posicao.position.z;
			hipotenusa = Mathf.Sqrt (Mathf.Pow (cateto1, 2) + Mathf.Pow (cateto2, 2));

			if (hipotenusa < 0.4f) {
				if (!manterNewtonAndando) {
					GetComponent<DadosForcaResultante> ().MudarNewtonAndando (0);
				}
				if (!manterNewtonRodando) {
					GetComponent<DadosForcaResultante> ().MudarNewtonRodando (0);
				}
				acabouAtuacao = true;
			}

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
		} else {
			if (tempoEspera >= maxTempoEspera) {
				estaAtuando = false;
			} else {
				tempoEspera += Time.deltaTime;
			}
		}
	}

	private void RodarAtor () {
		GetComponent<DadosForcaResultante> ().AddNewtonRodando ();

		if (sentidoHorario) {
			angulo += (GetComponent<DadosForcaResultante> ().PegarNewtonRodando () / 180);

			if (angulo + rotacao > 360) {
				chegouNoAngulo = true;
			}
		} else {
			angulo -= (GetComponent<DadosForcaResultante> ().PegarNewtonRodando ()) / 180;

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
		ato++;
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
	/// <param name="manterNewtonRodando">If set to <c>true</c> manter newton rodando.</param>
	/// <param name="maxTempoEspera">Max tempo espera.</param>
	public void ComecarAtuacaoRotacao (bool sentidoHorario, bool manterNewtonRodando, float maxTempoEspera) {
		this.manterNewtonRodando = manterNewtonRodando;
		this.sentidoHorario = sentidoHorario;
		this.maxTempoEspera = maxTempoEspera;
		indiceRotacoes++;
		rotacao = transform.eulerAngles.y - rotacoes[indiceRotacoes];

		if (this.rotacao < 0) {
			this.rotacao += 360;
		}

		tempoEspera = 0;
		ato++;
		angulo = 0;
		anguloInicial = transform.eulerAngles.y;
		tipoDeAtuacao = 2;
		chegouNoAngulo = false;
		acabouAtuacao = false;
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
	/// <param name="maxTempoEspera">Max tempo espera.</param>
	public void ComecarAtuacaoPosicao (bool sentidoHorario, bool correndo, bool manterNewtonAndando, bool manterNewtonRodando, float maxTempoEspera) {
		this.sentidoHorario = sentidoHorario;
		this.correndo = correndo;
		this.manterNewtonAndando = manterNewtonAndando;
		this.manterNewtonRodando = manterNewtonRodando;
		this.maxTempoEspera = maxTempoEspera;

		indicePosicoes++;
		posicao = GameObject.Find(posicoesNome + " (" + indicePosicoes.ToString () + ")").GetComponent<Transform> ();

		anguloInicial = transform.eulerAngles.y;
		ato++;
		tempoEspera = 0;
		angulo = 0;
		tipoDeAtuacao = 1;
		chegouNoAngulo = false;
		acabouAtuacao = false;
		estaAtuando = true;
	}

	public void ComecarAtuacaoTeleporte () {
		indicePosicoes++;
		posicao = GameObject.Find(posicoesNome + " (" + indicePosicoes.ToString () + ")").GetComponent<Transform> ();
		transform.eulerAngles = posicao.eulerAngles;
		transform.position = posicao.position;
		ato++;
	}

	public void MudarIndicePosicao (int valor) {
		indicePosicoes = valor;
	}

	/// <summary>
	/// Método que prepara o ator para começar a andar de lado.
	/// </summary>
	/// <param name="correndo">If set to <c>true</c> correndo.</param>
	/// <param name="manterNewtonAndando">If set to <c>true</c> manter newton andando.</param>
	/// <param name="manterNewtonRodando">If set to <c>true</c> manter newton rodando.</param>
	/// <param name="maxTempoEspera">Max tempo espera.</param>
	public void ComecarAtuacaoPosicaoDeLado (bool correndo, bool manterNewtonAndando, bool manterNewtonRodando, float maxTempoEspera) {
		this.correndo = correndo;
		this.manterNewtonAndando = manterNewtonAndando;
		this.maxTempoEspera = maxTempoEspera;

		indicePosicoes++;
		posicao = GameObject.Find(posicoesNome + " (" + indicePosicoes.ToString () + ")").GetComponent<Transform> ();

		ato++;
		tempoEspera = 0;
		tipoDeAtuacao = 13;
		chegouNoAngulo = false;
		acabouAtuacao = false;
		estaAtuando = true;
	}

	/// <summary>
	/// Método que prepara o ator para se mover no eixo y, ele pode subir ou descer dependendo dos parametros que foi passado.
	/// </summary>
	/// <param name="distancia">Distancia.</param>
	/// <param name="cima">If set to <c>true</c> cima.</param>
	/// <param name="correndo">If set to <c>true</c> correndo.</param>
	/// <param name="vezes">Vezes.</param>
	/// <param name="maxTempoEspera">Max tempo espera.</param>
	public void ComecarAtuacaoMoverNoEixoY(float distancia, bool cima, bool correndo, bool manterNewtonAndando, float vezes, float maxTempoEspera) {
		this.maxTempoEspera = maxTempoEspera;
		this.cima = cima;
		this.correndo = correndo;
		this.vezes = vezes;
		this.manterNewtonAndando = manterNewtonAndando;

		if (cima) {
			this.distancia = transform.position.y + distancia;
		} else {
			this.distancia = transform.position.y - distancia;
		}

		ato++;
		tempoEspera = 0;
		tipoDeAtuacao = 11;
		acabouAtuacao = false;
		estaAtuando = true;
	}

	/// <summary>
	/// Método que prepara o ator para ele começar a serguir outro ator.
	/// </summary>
	/// <param name="objeto">Objeto.</param>
	/// <param name="eixoX">If set to <c>true</c> eixo x.</param>
	/// <param name="eixoY">If set to <c>true</c> eixo y.</param>
	/// <param name="eixoZ">If set to <c>true</c> eixo z.</param>
	public void ComecarAtuacaoSeguirOutroAtor (Transform objeto, bool eixoX, bool eixoY, bool eixoZ) {
		this.eixoX = eixoX;
		this.eixoY = eixoY;
		this.eixoZ = eixoZ;

		this.objeto = objeto;
		distanciaX = objeto.position.x - transform.position.x;
		distanciaY = objeto.position.y - transform.position.y;
		distanciaZ = objeto.position.z - transform.position.z;

		ato++;
		tipoDeAtuacao = 12;
		estaAtuando = true;
	}

	/// <summary>
	/// Este método coloca um dialogo na tela para o jogador ler, é possivel fazer com a nova mensagem espere o outro acabar.
	/// </summary>
	/// <param name="menssagem">Menssagem.</param>
	/// <param name="maxTempoMudarTexto">Max tempo mudar texto.</param>
	/// <param name="maxTempoVidaTexto">Max tempo vida texto.</param>
	/// <param name="esperarAcabar">If set to <c>true</c> esperar acabar.</param>
	public void Falar (string menssagem, string nome, float maxTempoMudarTexto, float maxTempoVidaTexto, bool mostrarEsteRosto) {
		this.nome = nome;
		this.menssagem = menssagem;
		this.maxTempoMudarTexto = maxTempoMudarTexto;
		this.maxTempoVidaTexto = maxTempoVidaTexto;

		mudarTexto = true;

		this.mostrarEsteRosto = mostrarEsteRosto;
		ato++;
	}

	public void MoverNosEixosXZ (float distanciaX, float distanciaZ, bool manterNewtonAndando, float maxTempoEspera) {
		this.manterNewtonAndando = manterNewtonAndando;
		this.maxTempoEspera = maxTempoEspera;
		this.distanciaX = distanciaX;
		this.distanciaZ = distanciaZ;

		ato++;
		tipoDeAtuacao = 12;
		acabouAtuacao = false;
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
	
	public void MudarEstaAtuando (bool valor) {
		estaAtuando = valor;
	}

	public int PegarAto() {
		return ato;
	}

	public void IncrementarAto() {
		ato++;
	}

	public void MudarAtor(int valor) {
		ato = valor;
	}

	public float PegarAnguloDaPosicao() {
		return objeto.transform.eulerAngles.y;
	}

	public void MudarNome(string valor) {
		nome = valor;
	}

	public string PegarNome() {
		return nome;
	}
}