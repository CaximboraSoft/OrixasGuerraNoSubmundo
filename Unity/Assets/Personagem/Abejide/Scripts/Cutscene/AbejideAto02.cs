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

	private bool pularCutscene;

	private float tempoEspera;
	private float maxTempoEspera;

	private int sat;

	private string fala;

	void Awake () {
		lanca.enabled = false;
		hud.enabled = false;
		pularCutscene = false;

		corpoAnimator = GetComponentInChildren<Animator> ();
		meuMetodosDaCutscene = GetComponent<MetodosDaCutscene> ();
		telaPreta = canvasTelaPreta.GetComponentInChildren<Image> ();
		texto = canvasTelaPreta.GetComponentInChildren<Text> ();

		if (GetComponent<AbejideAto02> ().enabled) {
			AtivarCodigo ();
			GetComponentInChildren<Animator> ().runtimeAnimatorController = animacaoDaCurscene as RuntimeAnimatorController;
		}
	}
	
	public void AtivarCodigo () {
		
		if (!pularCutscene) {
			GetComponent<Rigidbody> ().useGravity = false;
			GetComponent<Collider> ().enabled = false;

			tempoEspera = 10;
			maxTempoEspera = 0;

			corpoAnimator.SetTrigger ("Crusificado");

			hud.enabled = false;

			canvasTelaPreta.enabled = true;
			texto.text = "";
			telaPreta.color = new Color (0, 0, 0, 1);
		}

		meuMetodosDaCutscene.MudarNome("Abejide:");
		meuMetodosDaCutscene.MudarIndicePosicao (0);
		meuMetodosDaCutscene.MudarAtor (0);
		meuMetodosDaCutscene.posicoesNome = "AbejidePosicao1";
		meuMetodosDaCutscene.PosicionarInicial ();

		GetComponent<AbejideAto02> ().enabled = true;
	}

	// Update is called once per frame
	void Update () {
		sat = meuMetodosDaCutscene.PegarSat ();

		if (Input.GetKey (KeyCode.Space) && !pularCutscene) {
			maxTempoEspera = 3f;
			PularCutscene ();
		}

		if (pularCutscene && tempoEspera > maxTempoEspera) {
			telaPreta.color = new Color (0, 0, 0, Mathf.Lerp(telaPreta.color.a, 0, 0.8f * Time.deltaTime));

			GetComponent<Rigidbody> ().AddForce (new Vector3 (0, -100000, 0));

			if (telaPreta.color.a < 0.3f) {
				AcabouCutscene ();
				
			}
		} else {
			if (!meuMetodosDaCutscene.PegarEstaAtuando () && tempoEspera > maxTempoEspera) {

				switch (meuMetodosDaCutscene.PegarAto ()) {
				case 0:
					if (meuMetodosDaCutscene.AtorJaPassouDoAto (2, 1)) {
						fala = "...";
						meuMetodosDaCutscene.Falar (fala, meuMetodosDaCutscene.PegarNome (), 0, 3 * sat, true);
						tempoEspera = 0;
						maxTempoEspera = 3 * sat;
					}
					break;

				case 1:
					meuMetodosDaCutscene.IncrementarAto ();
					break;
				case 2:
					if (meuMetodosDaCutscene.AtorJaPassouDoAto (2, 3)) {
						corpoAnimator.SetTrigger ("PerfuraPerfura");
						tempoEspera = 0;
						maxTempoEspera = 3f * sat;
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
							"Abejide: Mão, pais, vocês estão vivos?";
						tempoEspera = 0;
						maxTempoEspera = 5f * sat;
						GetComponent<MetodosDaCutscene> ().IncrementarAto ();
					}
					break;
				case 5:
					texto.text = "";
					fala = "AAAAAAaaaaaaHHHHHHhhhhhhhh...";
					meuMetodosDaCutscene.Falar (fala, meuMetodosDaCutscene.PegarNome (), 0, 3 * sat, true);
					tempoEspera = 0;
					maxTempoEspera = 3.1f * sat;
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
					maxTempoEspera = 1f *sat;
					meuMetodosDaCutscene.IncrementarAto ();
					break;
				case 8:
					corpoAnimator.SetBool ("Andando", false);
					corpoAnimator.SetTrigger ("VoltarHaAndar");
					tempoEspera = 0;
					maxTempoEspera = 3f * sat;
					meuMetodosDaCutscene.IncrementarAto ();
					break;
				case 9:
					if (meuMetodosDaCutscene.AtorJaPassouDoAto (2, 7)) {
						fala = "Maconha, devolve a minha machona!!!!!!";
						meuMetodosDaCutscene.Falar (fala, meuMetodosDaCutscene.PegarNome (), 4, 3 * sat, true);
						tempoEspera = 0;
						maxTempoEspera = (4f + 3f) * sat;
					}
					break;
				case 10:
					if (meuMetodosDaCutscene.AtorJaPassouDoAto(2, 8)) {
						AcabouCutscene ();
					}
					break;
				}
			} else {
				tempoEspera += Time.deltaTime;
			}
		}

		//Faz a fade do preto para o transparente.
		if (meuMetodosDaCutscene.PegarAto () < 2) {
			telaPreta.color = new Color (0, 0, 0, Mathf.Lerp(telaPreta.color.a, 0, 0.6f * sat * Time.deltaTime));

			if (telaPreta.color.a < 0.02f) {
				canvasTelaPreta.enabled = false;
			}
		}
	}

	private void AcabouCutscene () {
		GetComponent<MetodosDaCutscene> ().enabled = false;

		if (lanca != null) {
			Destroy (lanca.gameObject);
		}

		SeteEncruzilhadasAto02 destroir = GameObject.FindObjectOfType<SeteEncruzilhadasAto02> ();
		if (destroir != null) {
			Destroy (destroir.gameObject);
		}

		conversas.enabled = false;
		cameraPrincipal.GetComponent<Animator> ().enabled = false;
		conversasBoca.enabled = false;
		hud.enabled = true;
		canvasTelaPreta.enabled = false;
		GetComponent<Abejide> ().AtivarCodigo ();
		cameraPrincipal.GetComponent<CameraDetectarColisao> ().enabled = true;

		corpoAnimator.runtimeAnimatorController = animacaoOriginal as RuntimeAnimatorController;

		GetComponent<AbejideAto02> ().enabled = false;

		if (!pularCutscene) {
			CarcereiroAto01[] objTemp = FindObjectsOfType<CarcereiroAto01> ();
			for (int i = 0; i < objTemp.Length; i++) {
				objTemp [i].AcabouCutscene ();
			}
		}

		Destroy (cutsceneObjetos);
	}

	public void PularCutscene () {
		if (!GetComponent<AbejideAto02> ().enabled) {
			GetComponent<AbejideAto02> ().enabled = true;
			meuMetodosDaCutscene.MudarIndicePosicao (-1);
			meuMetodosDaCutscene.posicoesNome = "AbejidePosicao1";
			meuMetodosDaCutscene.ComecarAtuacaoTeleporte ();
		}

		telaPreta.color = new Color (0, 0, 0, 1);

		pularCutscene = true;
		canvasTelaPreta.enabled = true;
		texto.text = "";

		conversas.enabled = false;
		cameraPrincipal.GetComponent<Animator> ().enabled = false;
		conversasBoca.enabled = false;

		CarcereiroAto01[] objTemp = FindObjectsOfType<CarcereiroAto01> ();
		for (int i = 0; i < objTemp.Length; i++) {
			objTemp [i].PularCutscene ();
		}
		
		GetComponent<Collider> ().enabled = true;
		GetComponent<Rigidbody> ().useGravity = true;

		if (lanca != null) {
			Destroy (lanca.gameObject);
		}

		Destroy (GameObject.FindObjectOfType<SeteEncruzilhadasAto02> ().gameObject);
		corpoAnimator.runtimeAnimatorController = animacaoOriginal as RuntimeAnimatorController;
	}
}
