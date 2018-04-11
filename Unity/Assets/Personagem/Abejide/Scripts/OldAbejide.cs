using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldAbejide : MonoBehaviour {

	private Animator abejideAnimator;
	public Camera abejideCamera;
	public GameObject gameObjectAbejide;
	public GameObject cameraPosiiton;

	private bool animationHasChange;
	private bool estaAndandoFrente;
	private bool estaAndandoAtras;
	private bool estaRodandoDireita;
	private bool estaRodandoEsquerda;

	public float velocidadeAndando;
	public float addVelocidadeAndando;
	public float subVelocidadeAndando;
	public float maxVelocidadeAndando;
	public float velocidadeCorrendo;

	public float velocidadeRotacao;
	public float addVelocidadeRotacao;
	public float subVelocidadeRotacao;
	public float maxVelocidadeRotacao;

	private float distanceCameraZ;

	// Use this for initialization
	void Start () {
		animationHasChange = false;
		estaAndandoFrente = false;
		estaAndandoAtras = false;
		estaRodandoDireita = false;
		estaRodandoEsquerda = false;

		velocidadeAndando = 0;
		velocidadeRotacao = 0;
		distanceCameraZ = gameObject.transform.position.z - abejideCamera.transform.position.z;

		abejideAnimator = gameObjectAbejide.GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update ()
	{
		abejideCamera.transform.position = new Vector3 (gameObject.transform.position.x, abejideCamera.transform.position.y, gameObject.transform.position.z - distanceCameraZ);

		AplicarFisica();
		LeitorTeclado();
		//AplicarAnimacao();
	}

	void AplicarFisica()
	{
		/*
         * Roda o Abejide para esquerda quando a seta da esquerda estiver sendo pressionada, tem uma
         * aceleração.
         */
		if (estaRodandoEsquerda && velocidadeRotacao > -maxVelocidadeRotacao)
		{
			velocidadeRotacao -= ((subVelocidadeRotacao + velocidadeAndando) * Time.deltaTime);
		}
		/* Se a seta da esquerda não estiver sendo pressionada e a seta da direita tambem não estiver
         * sendo pressionada, a velocidade de rotação do Abejide vai sendo diminuida para que ele
         * não pare de rodar subtamente quando o jogador parar de pressionar as teclas de rotação.
         */
		else if (!estaRodandoDireita && velocidadeRotacao < 0)
		{
			velocidadeRotacao += ((subVelocidadeRotacao + velocidadeAndando) * Time.deltaTime);
			if (velocidadeRotacao > 0)
			{
				velocidadeRotacao = 0;
			}
		}
		/*
         * Roda o Abejide para direia quando a seta da dirita estiver sendo pressionada, tem uma
         * aceleração.
         */
		if (estaRodandoDireita && velocidadeRotacao < maxVelocidadeRotacao)
		{
			velocidadeRotacao += ((addVelocidadeRotacao + velocidadeAndando) * Time.deltaTime);
		}
		//Faz Abejide parar de rotacionar suavemente, igual a seta da direita.
		else if (!estaRodandoEsquerda && velocidadeRotacao > 0)
		{
			velocidadeRotacao -= ((addVelocidadeRotacao + velocidadeAndando) * Time.deltaTime);
			if (velocidadeRotacao < 0)
			{
				velocidadeRotacao = 0;
			}
		}
		gameObject.transform.Rotate(0, velocidadeRotacao * Time.deltaTime, 0);


		if (estaAndandoFrente && velocidadeAndando < maxVelocidadeAndando)
		{
			velocidadeAndando += (addVelocidadeAndando * Time.deltaTime);
		}
		else if (!estaAndandoAtras && velocidadeAndando > 0)
		{
			velocidadeAndando -= (subVelocidadeAndando * Time.deltaTime);
			if (velocidadeAndando < 0)
			{
				velocidadeAndando = 0;
			}
		}
		if (estaAndandoAtras && velocidadeAndando > -maxVelocidadeAndando)
		{
			velocidadeAndando -= (addVelocidadeAndando * Time.deltaTime);
		}
		else if (!estaAndandoFrente && velocidadeAndando < 0)
		{
			velocidadeAndando += (subVelocidadeAndando * Time.deltaTime);
			if (velocidadeAndando > 0)
			{
				velocidadeAndando = 0;
			}
		}
		gameObject.transform.Translate(0, 0, velocidadeAndando * Time.deltaTime);
	}

	void LeitorTeclado()
	{
		//Esquerda
		if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
		{
			estaRodandoEsquerda = true;

			if (!abejideAnimator.GetBool("Walking"))
			{
				abejideAnimator.SetBool("Walking", true);
				animationHasChange = true;
			}
		}

		//Direita
		else if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
		{
			estaRodandoDireita = true;

			if (!abejideAnimator.GetBool("Walking"))
			{
				abejideAnimator.SetBool("Walking", true);
				animationHasChange = true;
			}
		}

		//Não esta apertando nenhuma tecla de rotacionar
		else if (abejideAnimator.GetBool("Walking") && (estaRodandoEsquerda || estaRodandoDireita))
		{
			abejideAnimator.SetBool("Walking", false);
			animationHasChange = true;
			estaRodandoEsquerda = false;
			estaRodandoDireita = false;
		}

		//Cima
		if (Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
		{
			estaAndandoFrente = true;

			if (!abejideAnimator.GetBool("Walking"))
			{
				abejideAnimator.SetBool("Walking", true);
				animationHasChange = true;
			}
		}

		//Baixo
		else if (Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.UpArrow))
		{
			estaAndandoAtras = true;

			if (!abejideAnimator.GetBool("Walking"))
			{
				abejideAnimator.SetBool("Walking", true);
				animationHasChange = true;
			}
		}

		//Não esta apertando nenhuma tracla de movimento
		else if (abejideAnimator.GetBool("Walking") && (estaAndandoFrente || estaAndandoAtras))
		{
			abejideAnimator.SetBool("Walking", false);
			animationHasChange = true;
			estaAndandoFrente = false;
			estaAndandoAtras = false;
		}

	}

	void AplicarAnimacao()
	{
		if (animationHasChange)
		{
			if (abejideAnimator.GetBool("Walking"))
			{
				Debug.Log("ANDANDO");
			}
			else
			{
				Debug.Log("PARADO");
			}

			animationHasChange = false;
		}
	}
}

