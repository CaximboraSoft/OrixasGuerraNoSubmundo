using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class AbejideAto02 : MonoBehaviour {

	private MetodosDaCutscene meuMetodosDaCutscene;
	public GameObject cutsceneObjetos;
	public Camera cameraPrincipal;
	public Canvas hud;
	public Canvas conversas;
	public Conversas conversasBoca;
	private Animator corpoAnimator;
	public Renderer lanca;
	public RuntimeAnimatorController animacaoOriginal;
	public RuntimeAnimatorController animacaoDaCurscene;
	public Canvas canvasTelaPreta;
	private Image telaPreta;
	private Text texto;

	private float tempoEspera;
	private float maxTempoEspera;

	private string fala;

	void Start () {
		hud.enabled = false;

		if (GetComponent<AbejideAto02> ().enabled) {
			AtivarCodigo ();
		}
	}

	public void AtivarCodigo () {
		GetComponent<Rigidbody> ().useGravity = false;
		GetComponent<Collider> ().enabled = false;

		tempoEspera = 10;
		maxTempoEspera = 0;

		meuMetodosDaCutscene = GetComponent<MetodosDaCutscene> ();
		meuMetodosDaCutscene.MudarNome("Abejide:");
		meuMetodosDaCutscene.MudarIndicePosicao (0);
		meuMetodosDaCutscene.MudarAtor (0);
		meuMetodosDaCutscene.posicoesNome = "AbejidePosicao1";
		meuMetodosDaCutscene.PosicionarInicial ();

		corpoAnimator = GetComponentInChildren<Animator> ();
		corpoAnimator.runtimeAnimatorController = animacaoDaCurscene as RuntimeAnimatorController;
		corpoAnimator.SetTrigger ("Crusificado");

		hud.enabled = false;
		lanca.enabled = false;


		telaPreta = canvasTelaPreta.GetComponentInChildren<Image> ();
		texto = canvasTelaPreta.GetComponentInChildren<Text> ();

		canvasTelaPreta.enabled = true;
		texto.text = "";
		telaPreta.color = new Color (0, 0, 0, 1);

		GetComponent<AbejideAto02> ().enabled = true;
	}

	// Update is called once per frame
	void Update () {

		if (!meuMetodosDaCutscene.PegarEstaAtuando () && tempoEspera > maxTempoEspera) {

			switch (meuMetodosDaCutscene.PegarAto ()) {
			case 0:
				if (meuMetodosDaCutscene.AtorJaPassouDoAto (2, 1)) {
					fala = "...";
					meuMetodosDaCutscene.Falar (fala, meuMetodosDaCutscene.PegarNome (), 0, 3, true);
					tempoEspera = 0;
					maxTempoEspera = 3;
				}
				break;

			case 1:
				meuMetodosDaCutscene.IncrementarAto ();
				break;
			case 2:
				if (meuMetodosDaCutscene.AtorJaPassouDoAto (2, 3)) {
					corpoAnimator.SetTrigger ("PerfuraPerfura");
					tempoEspera = 0;
					maxTempoEspera = 3f;
					meuMetodosDaCutscene.IncrementarAto ();
					lanca.enabled = true;
				}
				break;
			case 3:
				canvasTelaPreta.enabled = true;
				meuMetodosDaCutscene.IncrementarAto ();
				break;
			case 4:
				telaPreta.color = new Color (0, 0, 0, Mathf.Lerp(telaPreta.color.a, 1, 0.5f * Time.deltaTime));

				if (telaPreta.color.a >= 0.98f) {
					telaPreta.color = new Color (0, 0, 0, 1);
					texto.text = "Abejide: Será que a minha familia sofreu tanto assim? \n\nVoz desconhecida: Agora, creça garoto! nos livre dessa maldição.\n\n" +
						"Abejide: Mão, pais, vcs estão vivos?";
					tempoEspera = 0;
					maxTempoEspera = 5f;
					GetComponent<MetodosDaCutscene> ().IncrementarAto ();
				}
				break;
			case 5:
				texto.text = "";
				fala = "AAAAAAaaaaaaHHHHHHhhhhhhhh...";
				meuMetodosDaCutscene.Falar (fala, meuMetodosDaCutscene.PegarNome (), 0, 3, true);
				tempoEspera = 0;
				maxTempoEspera = 3.1f;
				break;
			case 6:
				telaPreta.color = new Color (0, 0, 0, Mathf.Lerp(telaPreta.color.a, 0, 1.2f * Time.deltaTime));

				if (telaPreta.color.a <= 0.02f) {
					canvasTelaPreta.enabled = false;
					telaPreta.color = new Color (0, 0, 0, 0);
					meuMetodosDaCutscene.IncrementarAto ();
				}
				break;

			case 7:
				corpoAnimator.SetTrigger ("Caindo");
				Destroy (lanca.gameObject);
				GetComponent<Collider> ().enabled = true;
				GetComponent<Rigidbody> ().useGravity = true;
				tempoEspera = 0;
				maxTempoEspera = 1f;
				meuMetodosDaCutscene.IncrementarAto ();
				break;
			case 8:
				corpoAnimator.SetBool ("Andando", false);
				corpoAnimator.SetTrigger ("VoltarHaAndar");
				tempoEspera = 0;
				maxTempoEspera = 3f;
				meuMetodosDaCutscene.IncrementarAto ();
				break;
			case 9:
				if (meuMetodosDaCutscene.AtorJaPassouDoAto (2, 7)) {
					fala = "Maconha, devolve a minha machona!!!!!!";
					meuMetodosDaCutscene.Falar (fala, meuMetodosDaCutscene.PegarNome (), 0, 3, true);
					tempoEspera = 0;
					maxTempoEspera = 3f;
				}
				break;
			case 10:
				canvasTelaPreta.enabled = false;
				conversas.enabled = false;
				cameraPrincipal.GetComponent<Animator> ().enabled = false;
				conversasBoca.enabled = false;

				hud.enabled = true;
				corpoAnimator.runtimeAnimatorController = animacaoOriginal as RuntimeAnimatorController;
				GetComponent<Abejide> ().AtivarCodigo ();

				GetComponent<AbejideAto02> ().enabled = false;
				break;
			}
		} else {
			tempoEspera += Time.deltaTime;
		}

		//Faz a fade do preto para o transparente.
		if (meuMetodosDaCutscene.PegarAto () < 2) {
			telaPreta.color = new Color (0, 0, 0, Mathf.Lerp(telaPreta.color.a, 0, 0.6f * Time.deltaTime));

			if (telaPreta.color.a < 0.02f) {
				canvasTelaPreta.enabled = false;
			}
		}
	}
}
