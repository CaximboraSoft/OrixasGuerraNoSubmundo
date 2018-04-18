using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutscenePersonagens : MonoBehaviour {

	public Transform seta;
	public Transform[] posicao;

	public float[] rotacoes;
	private float massaTotal;
	private float newtonAndando;
	private float newtonRodando;	
	private float hipotenusa;
	private float catetoOposto;
	private float catetoAdjacente;
	private float anguloChegar;
	private float anguloQueEstou;
	private float angulo;

	/*
	 * 0 Não atua
	 * 1 Muda a posicao
	 * 2 Muda a animação
	 * 3 Muda a rotação
	 */
	private int qualTipoDeAtuacao;
	private int indicePosicao;
	private int indiceAnimacao;
	private int indiceRotacao;

	private bool alinhouComPosicao;
	private bool acabouDeAtuar;
	private bool sentidoHorario;

	// Use this for initialization
	void Start () {
		indicePosicao = 0;
		indiceAnimacao = 0;
		indiceRotacao = 0;

		qualTipoDeAtuacao = 0;

		transform.position = new Vector3 (posicao[0].position.x, transform.position.y, posicao[0].position.z);
		transform.eulerAngles = new Vector3 (0, rotacoes[0], 0);
	}
	
	// Update is called once per frame
	void Update () {

		//Muda a posição.
		if (qualTipoDeAtuacao == 1) {
			MudarPosicao ();
		//Muda a animação.
		} else if (qualTipoDeAtuacao == 2) {
			//???
		//Muda a rotação.
		} else if (qualTipoDeAtuacao == 3) {
			MudaRotacao ();
		}
	}

	public void MudaRotacao() {
		if (rotacoes [indiceRotacao] != -999) {
			newtonRodando += GetComponent<DadosForcaResultante> ().addNewtonRodando;
			if (newtonRodando > GetComponent<DadosForcaResultante> ().maxNewtonRodando) {
				newtonRodando = GetComponent<DadosForcaResultante> ().maxNewtonRodando;
			}

			//Rotaciona o ator até ele alinhar com o seu destino
			if (!alinhouComPosicao) {
				RatacionarAtor ();
			} else {
				//transform.eulerAngles = new Vector3(transform.position.x, rotacoes[indicePosicao], transform.position.z);
				acabouDeAtuar = true;
			}
		} else {
			acabouDeAtuar = true;
		}
	}

	/// <summary>
	/// Rotaciona o ator até ele chegar no seu angulo de destino, essa rotação pode ser no sentido horario ou anti horario, isso vai
	/// depender do resultado de outro método, o <CalcularTrajetoriaDeRotacao>
	/// </summary>
	public void MudarPosicao() {
		//Acelera a velocidade de andar.
		newtonAndando += GetComponent<DadosForcaResultante>().addNewtonAndando;
		if (newtonAndando > GetComponent<DadosForcaResultante>().maxNewtonAndando) {
			newtonAndando = GetComponent<DadosForcaResultante>().maxNewtonAndando;
		}
		transform.Translate (0, 0, (newtonAndando / massaTotal) * Time.deltaTime);

		//Rotaciona o ator até ele alinhar com o seu destino
		if (!alinhouComPosicao) {
			RatacionarAtor ();
		} else {
			transform.LookAt (new Vector3(posicao[indicePosicao].position.x, transform.position.y, posicao[indicePosicao].position.z));
		}

		catetoOposto = transform.position.z - posicao [indicePosicao].position.z;
		catetoAdjacente = transform.position.x - posicao [indicePosicao].position.x;
		hipotenusa = Mathf.Sqrt (Mathf.Pow (catetoOposto, 2) + Mathf.Pow(catetoAdjacente, 2));

		//Acaba a atuação quando o ator chegar no seu ponto final
		if (hipotenusa < 0.4f) {
			qualTipoDeAtuacao = 0;
			acabouDeAtuar = true;
		}
	}

	public void RatacionarAtor () {
		transform.eulerAngles = new Vector3 (transform.eulerAngles.x, anguloQueEstou - angulo, transform.eulerAngles.z);

		//Acelera a velocidade de rotação.
		newtonRodando += GetComponent<DadosForcaResultante>().addNewtonRodando;
		if (newtonRodando > GetComponent<DadosForcaResultante>().maxNewtonRodando) {
			newtonRodando = GetComponent<DadosForcaResultante>().maxNewtonRodando;
		}

		if (sentidoHorario) {
			angulo -= ((newtonRodando / massaTotal) * 20) * Time.deltaTime;

			if (angulo + anguloChegar < 0) {
				alinhouComPosicao = true;
			}
		} else {
			angulo += ((newtonRodando / massaTotal) * 20) * Time.deltaTime;

			if (angulo + anguloChegar > 360) {
				alinhouComPosicao = true;
			}
		}
	}

	//Métodos para alterar os valores das variáveis desse script.
	public void MudarMassa (float massaTotal) {
		this.massaTotal = massaTotal;
	}

	/// <summary>
	/// Diz qual vai ser a atuação que os atores vai ter que fazer na cena
	/// </summary>
	/// <param name="qualTipoDeAtuacao">Qual tipo de atuacao.</param>
	public void MudarParametros(int qualTipoDeAtuacao) {
		if (qualTipoDeAtuacao == 1) {
			indicePosicao++;
			seta.LookAt (new Vector3(posicao[indicePosicao].position.x, seta.position.y, posicao[indicePosicao].position.z));
			newtonAndando = 0;
			newtonRodando = 0;
			CalcularTrajetoriaDeRotacao ();
		} else if (qualTipoDeAtuacao == 2) {
			indiceAnimacao++;
		} else if (qualTipoDeAtuacao == 3) {
			indiceRotacao++;
			newtonRodando = 0;
			seta.eulerAngles = new Vector3 (seta.eulerAngles.x, rotacoes[indiceRotacao], seta.eulerAngles.x);
			CalcularTrajetoriaDeRotacao ();
		}
		
		acabouDeAtuar = false;

		this.qualTipoDeAtuacao = qualTipoDeAtuacao;
	}

	/// <summary>
	/// Calcula o menor caminho para que o ator aponte para seu angulo de destino.
	/// </summary>
	public void CalcularTrajetoriaDeRotacao () {
		angulo = 0;

		/* Primeiro e feito um calculo para deixar o ator com o angulo 0 angulo de onde ele deve chegar no mesmo lugar porém
		 * considerando agora o ator com o angulo 0, ou seja, meio que rotaciona o ator e o seu angulo de destino até que o ator
		 * chegue no angulo 0
		 */
		anguloChegar = seta.eulerAngles.y;
		anguloQueEstou = transform.eulerAngles.y;
		anguloChegar -= anguloQueEstou;

		//Se o angulo ficar menor que zero, é preciso converter ele para angulo positivo sem alterar o seu angulo de destino.
		if (anguloChegar < 0) {
			anguloChegar += 360;
		}

		//Agora como o ator e seu angulo de destino estão na "mesma base", com essa decisão é se sabe que o menor caminho para se
		//chegar no meu angulo de destino é pelo sentido horario ou anti horario.
		sentidoHorario = true;
		if (anguloChegar > 180) {
			sentidoHorario = false;
		}
		alinhouComPosicao = false; //Depois que o ator chega no seu angulo de destino essa variável impedi que ele continue rodando.
	}

	public bool PegarAcabouDeAtuar() {
		return acabouDeAtuar;
	}
}
