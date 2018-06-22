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
			GetComponentInChildren<Animator> ().runtimeAnimatorController = animacaoDaCurscene as RuntimeAnimatorController;
			AtivarCodigo ();
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

		cameraPrincipal.GetComponent<CameraAto01> ().enabled = false;
		cameraPrincipal.GetComponent<CameraAto02> ().AtivarCodigo ();

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

		if (Input.GetKey (KeyCode.Return) && !pularCutscene) {
			maxTempoEspera = 1f;
			PularCutscene ();
		}

		if (pularCutscene && tempoEspera > maxTempoEspera) {
			AcabouCutscene ();
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
					telaPreta.color = new Color (0, 0, 0, Mathf.Lerp(telaPreta.color.a, 0.7f, 0.5f * Time.deltaTime));

					if (telaPreta.color.a >= 0.68f) {
						telaPreta.color = new Color (0, 0, 0, 0.7f);
						texto.text = "Abejide (Pensativo) \n\nTodos que conheço faleceram, minha familia assasinada e meus companheiros brutalmente massacrados\n" +
							"por um Orixa. E ainda por cima ter que arcar com o peso dessa tortura, mediante aos opressores Exus.\n" +
							"Como o destino é cruel. Sera que minha familia sofreu tanto a esse ponto.\n\n" +
							"Lembranças:\n\nUm vilarejo, um anciao, pai e mãe... presentes em um so momento.\n\n" +
							"Todos:\n\nCresça garoto. Nos livre desssa maldição e livre-se de seu passado\n\nAbejide:\n\n" +
							"Não deixarei que este seja o meu fim. Eles confiaram a mim a missão de matar essa linha estensa linha de Orixas e Exus\nque afrontam a " +
							"humanidade tirando a sua liberdade.";
						tempoEspera = 0;
						maxTempoEspera = 45f * sat; //Tempo bom: 45f
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
					maxTempoEspera = 0.75f;
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
						fala = "Não permitirei que saia impune";
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

	void FixedUpdate () {
		if (meuMetodosDaCutscene.PegarAto () == 8) {
			GetComponent<Rigidbody> ().AddForce (transform.up * -6000f);
		}
	}

	private void AcabouCutscene () {
		if (!pularCutscene) {
			GameObject.FindObjectOfType<SeteEncruzilhadasAto02> ().ColocarNaBatalhaFinal ();
		}

		GetComponentInChildren<Arma> ().enabled = true;
		GetComponent<MetodosDaCutscene> ().enabled = false;

		if (lanca != null) {
			Destroy (lanca.gameObject);
		}

		//Ativa a camera e destroi os atores dela
		cameraPrincipal.GetComponent<seguirJogador> ().enabled = true;
		Destroy (cameraPrincipal.GetComponent<CameraAto01> ());
		Destroy (cameraPrincipal.GetComponent<CameraAto02> ());
		Destroy (cameraPrincipal.GetComponent<MetodosDaCutscene> ());

		conversas.enabled = false;
		cameraPrincipal.GetComponent<Animator> ().enabled = false;
		conversasBoca.enabled = false;
		hud.enabled = true;
		canvasTelaPreta.enabled = false;
		GetComponent<Abejide> ().AtivarCodigo ();
		cameraPrincipal.GetComponent<CameraDetectarColisao> ().enabled = true;

		corpoAnimator.runtimeAnimatorController = animacaoOriginal as RuntimeAnimatorController;

		if (!pularCutscene) {
			CarcereiroAto01[] objTemp = FindObjectsOfType<CarcereiroAto01> ();
			for (int i = 0; i < objTemp.Length; i++) {
				objTemp [i].AcabouCutscene ();
			}
		}

		Destroy (cutsceneObjetos);

		GameObject.FindObjectOfType<Lula> ().enabled = true;
		Destroy (GetComponent<AbejideAto01> ());
		Destroy (GetComponent<AbejideAto02> ());
	}

	public void PularCutscene () {
		GameObject.FindObjectOfType<SeteEncruzilhadasAto02> ().ColocarNaBatalhaFinal ();

		if (!GetComponent<AbejideAto02> ().enabled) {
			GetComponent<AbejideAto02> ().enabled = true;
			meuMetodosDaCutscene.MudarIndicePosicao (-1);
			meuMetodosDaCutscene.posicoesNome = "AbejidePosicao1";
			meuMetodosDaCutscene.ComecarAtuacaoTeleporte ();
		}

		telaPreta.color = new Color (0, 0, 0, 0);

		pularCutscene = true;
		canvasTelaPreta.enabled = false;
		texto.text = "";

		conversas.enabled = false;
		cameraPrincipal.GetComponent<Animator> ().enabled = false;
		cameraPrincipal.GetComponent<seguirJogador> ().AtivarCodigo ();

		conversasBoca.enabled = false;

		CarcereiroAto01[] objTemp = FindObjectsOfType<CarcereiroAto01> ();
		for (int i = 0; i < objTemp.Length; i++) {
			objTemp [i].PularCutscene ();
		}

		InimigosNormais[] objInimigosNormais = FindObjectsOfType<InimigosNormais> ();
		for (int i = 0; i < objInimigosNormais.Length; i++) {
			objInimigosNormais [i].AtivarCodigo ();
		}
		
		GetComponent<Collider> ().enabled = true;
		GetComponent<Rigidbody> ().useGravity = true;

		if (lanca != null) {
			Destroy (lanca.gameObject);
		}
		
		corpoAnimator.runtimeAnimatorController = animacaoOriginal as RuntimeAnimatorController;
		transform.position = new Vector3 (transform.position.x, 0.2f, transform.position.z);
	}
}
