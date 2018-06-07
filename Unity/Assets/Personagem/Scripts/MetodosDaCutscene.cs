using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MetodosDaCutscene : MonoBehaviour {

	private DadosMovimentacao meuDadosMovimentacao;
	public Transform boca;
	public Transform rostoMostrar;
	private Transform objeto;
	public Transform seta; //Guarda o angulo que o personagem tem que chegar, isso é dado segundo o método <LookAt>
	private Transform posicao;
	private DadosDaFase meuDadosDaFase;
	private float rotacao;

	public bool mudarTexto; 
	private bool cima;
	private bool estaAtuando;
	private bool acabouAtuacao;
	private bool eixoX;
	private bool eixoY;
	private bool eixoZ;
	private bool mostrarEsteRosto;
	private bool detectarColisaoComDestino;
	private bool manterAnimacaoAndando;
	private bool manterDesativadaAGravidade;

	private float distanciaX;
	private float distanciaY;
	private float distanciaZ;
	private float distancia;
	private float hipotenusa;
	private float cateto1;
	private float cateto2;
	private float tempoEspera;
	private float maxTempoEspera;
	private float vezes;
	private float velocidade;
	private float velocidadeCrescente;
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
	public int ato;

	public string nome;
	public string menssagem;
	public string posicoesNome;

	// Use this for initialization
	void Start () {
		meuDadosDaFase = GameObject.FindGameObjectWithTag ("Terra").GetComponent<DadosDaFase> ();
		meuDadosMovimentacao = GetComponent<DadosMovimentacao> ();

		ato = 0;
		tipoDeAtuacao = 0;
		indicePosicoes = 0;
		tempoMudarTexto = 0;
		maxTempoMudarTexto = 0;
		velocidadeCrescente = 0;

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
			case 2:
				MudarRotacao ();
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
			if (cima) {
				transform.Translate (0, velocidade * Time.deltaTime, 0);

				if (transform.position.y > distancia) {
					acabouAtuacao = true;
				}
			} else {
				transform.Translate (0, -velocidade* Time.deltaTime, 0);

				if (transform.position.y < distancia) {
					acabouAtuacao = true;
				}
			}
		} else {
			if (tempoEspera >= maxTempoEspera) {
				if (!manterDesativadaAGravidade) {
					GetComponent<Collider> ().enabled = true;
					GetComponent<Rigidbody> ().useGravity = true;
				}

				estaAtuando = false;
			} else {
				tempoEspera += Time.deltaTime;
			}
		}
	}

	/// <summary>
	/// Faz o ator rodar até chegar no angulo que foi passado no método <ComecarAtuacaoRotacao>
	/// </summary>
	private void MudarRotacao() {
		if (!acabouAtuacao) {
			seta.eulerAngles = new Vector3 (0, rotacao, 0);
			transform.rotation = Quaternion.Lerp (transform.rotation, seta.rotation, meuDadosMovimentacao.velocidadeRotacao * Time.deltaTime * vezes);

			if (Mathf.Abs(transform.eulerAngles.y - seta.eulerAngles.y) < 5) {
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
			velocidadeCrescente = Mathf.Lerp (velocidadeCrescente, velocidade, 1f * Time.deltaTime);
			transform.Translate (velocidadeCrescente * Time.deltaTime, 0, 0);
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
			velocidadeCrescente = Mathf.Lerp (velocidadeCrescente, velocidade, 1f * Time.deltaTime);
			transform.Translate (0, 0, velocidadeCrescente * Time.deltaTime);

			seta.LookAt (new Vector3 (posicao.position.x, seta.position.y, posicao.position.z));
			transform.rotation = Quaternion.Lerp (transform.rotation, seta.rotation, meuDadosMovimentacao.velocidadeRotacao * Time.deltaTime);


		} else {
			if (tempoEspera >= maxTempoEspera) {
				meuDadosMovimentacao.MudarBoolDoMeuAnimator("Andando", manterAnimacaoAndando);

				if (!manterAnimacaoAndando) {
					velocidadeCrescente = 0;
				}

				estaAtuando = false;
			} else {
				tempoEspera += Time.deltaTime;
			}
		}
	}

	/// <summary>
	/// Método que prepara para o ator olhar para um objeto.
	/// </summary>
	/// <param name="posicao">Posicao.</param>
	/// <param name="sentidoHorario">If set to <c>true</c> sentido horario.</param>
	/// <param name="manterNewtonRodando">If set to <c>true</c> manter newton rodando.</param>
	public void ComecarAtuacaoOlharParaObjeto (Transform posicao, bool sentidoHorario, bool manterNewtonRodando) {
		this.posicao = posicao;

		ato++;
		tipoDeAtuacao = 21;
		estaAtuando = true;
	}

	/// <summary>
	/// Método que prepara o ator para rotacionar até o angulo que foi passado.
	/// </summary>
	/// <param name="anguloChegar">Angulo chegar.</param>
	/// <param name="sentidoHorario">If set to <c>true</c> sentido horario.</param>
	/// <param name="manterNewtonRodando">If set to <c>true</c> manter newton rodando.</param>
	/// <param name="maxTempoEspera">Max tempo espera.</param>
	public void ComecarAtuacaoRotacao (float rotacao, float vezes, float maxTempoEspera) {
		this.vezes = vezes;
		this.maxTempoEspera = maxTempoEspera;
		this.rotacao = rotacao;

		tempoEspera = 0;
		ato++;
		tipoDeAtuacao = 2;
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
	public void ComecarAtuacaoPosicao (float velocidade, bool manterAnimacaoAndando, bool correndo, float maxTempoEspera) {
		this.velocidade = velocidade;
		this.maxTempoEspera = maxTempoEspera;
		this.manterAnimacaoAndando = manterAnimacaoAndando;

		indicePosicoes++;
		posicao = GameObject.Find(posicoesNome + " (" + indicePosicoes.ToString () + ")").GetComponent<Transform> ();
		meuDadosMovimentacao.MudarBoolDoMeuAnimator("Andando", true);
		meuDadosMovimentacao.MudarBoolDoMeuAnimator("Correndo", correndo);

		ato++;
		tempoEspera = 0;
		tipoDeAtuacao = 1;
		acabouAtuacao = false;
		estaAtuando = true;
	}

	public void ComecarAtuacaoTeleporte () {
		indicePosicoes++;
		posicao = GameObject.Find (posicoesNome + " (" + indicePosicoes.ToString () + ")").GetComponent<Transform> ();
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
	public void ComecarAtuacaoPosicaoDeLado (float velocidade, float maxTempoEspera) {
		this.velocidade = velocidade;
		this.maxTempoEspera = maxTempoEspera;

		indicePosicoes++;
		posicao = GameObject.Find(posicoesNome + " (" + indicePosicoes.ToString () + ")").GetComponent<Transform> ();

		ato++;
		tempoEspera = 0;
		velocidadeCrescente = 0;
		tipoDeAtuacao = 13;
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
	public void ComecarAtuacaoMoverNoEixoY(float distancia, bool cima, bool manterDesativadaAGravidade, float velocidade, float maxTempoEspera) {
		GetComponent<Collider> ().enabled = false;
		GetComponent<Rigidbody> ().useGravity = false;

		this.maxTempoEspera = maxTempoEspera;
		this.cima = cima;
		this.velocidade = velocidade;
		this.manterDesativadaAGravidade = manterDesativadaAGravidade;

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

	public DadosMovimentacao PegarMeuDadosMovimentacao() {
		return meuDadosMovimentacao;
	}

	void OnTriggerEnter(Collider collision) {
		if (estaAtuando && collision.transform.name == posicao.name) {
			acabouAtuacao = true;
		}
	}

	public void PosicionarInicial () {
		objeto = GameObject.Find (posicoesNome + " (" + indicePosicoes.ToString () + ")").GetComponent<Transform> ();
		transform.position = objeto.position;
		transform.eulerAngles = objeto.eulerAngles;
	}

	/// <summary>
	/// Retorna o ato de um determinado ator ja passou do número desejado, no caso a referencia a ele éfeita pelo indice dele no jogo.
	/// </summary>
	/// <returns><c>true</c>, if ja passou do ato was atored, <c>false</c> otherwise.</returns>
	/// <param name="indiceDoAtor">Indice do ator.</param>
	/// <param name="numeroDoAto">Numero do ato.</param>
	public bool AtorJaPassouDoAto (int indiceDoAtor, int numeroDoAto) {
		return meuDadosDaFase.atores[indiceDoAtor].GetComponent<MetodosDaCutscene> ().PegarAto () > numeroDoAto;
	}

	/// <summary>
	/// Retornar o transform do ator que esta no indice que foi passado.
	/// </summary>
	/// <returns>The ator.</returns>
	public Transform PegarOutroAtor (int indice) {
		return meuDadosDaFase.atores [indice];
	}

	public int PegarSat () {
		return meuDadosDaFase.sat;
	}
}