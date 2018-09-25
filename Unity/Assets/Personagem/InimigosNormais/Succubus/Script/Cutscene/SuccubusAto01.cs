using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class SuccubusAto01 : MonoBehaviour {

	private Animator atorAnimator;
	public Transform abejideSeta;
	public Transform lugarDoAbejide;
	public Transform rostoMostrarSuccubus;
	public Transform rostoMostrarAbejide;
	private Transform abejide;
	private InimigosNormais[] inimigosNormais;
	private DadosDaFase dadosDaFase;

	public Conversas boca;

	public bool abejidePegouEspada = false;
	private bool marreu = false;

	private float temporizador;
	private float maxTemporizador;

	private string fala;

	private int ato = 0;

	void Awake () {
		abejide = GameObject.FindGameObjectWithTag ("Abejide").transform;
		atorAnimator = GetComponentInChildren<Animator> ();
		dadosDaFase = FindObjectOfType<DadosDaFase> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (ato != -100 && ato > 0 && ato < 8 && abejidePegouEspada && Input.GetKeyDown (KeyCode.Return)) {
			ato = -100;
			PularCutscene ();
		}

		switch (ato) {
		case 0:
			if (Vector3.Distance (transform.position, abejide.position) < 15f) {
				ato++;
			}
			break;
		case 1:
			abejide.GetComponent<Abejide> ().enabled = false;
			inimigosNormais = FindObjectsOfType<InimigosNormais> ();
			for (int i = 0; i < inimigosNormais.Length; i++) {
				inimigosNormais [i].rodandoCutscene = true;
			}
			ato++;

			abejide.GetComponentInChildren<Animator> ().SetFloat ("Velocidade", 3);
			break;
		//Abejide se move até a posição da cutscene
		case 2:
			abejideSeta.LookAt ((new Vector3 (lugarDoAbejide.position.x, abejideSeta.position.y, lugarDoAbejide.position.z)));
			abejide.rotation = Quaternion.Lerp (abejide.rotation, abejideSeta.rotation, 3 * Time.deltaTime);
			abejide.position = Vector3.Lerp (abejide.position, lugarDoAbejide.position, 0.5f * Time.deltaTime);

			if (Vector3.Distance (lugarDoAbejide.position, abejide.position) < 1f) {
				boca.enabled = true;
				boca.GetComponent<Conversas> ().MudarRostoMostrar (rostoMostrarAbejide);
				if (dadosDaFase.PegarIndioma ()) {
					fala = "Não pode ser...";
				} else {
					fala = "Ingles";
				}
				boca.GetComponent<Conversas> ().nome.text = "Abejide:";
				boca.GetComponent<Text> ().text = fala;
				boca.GetComponent<Conversas> ().MudarTempoLimparTexto (2f);
				boca.GetComponent<Conversas> ().conversas.enabled = true;
				temporizador = 0;
				maxTemporizador = 2f;
				abejide.GetComponentInChildren<Animator> ().SetFloat ("Velocidade", 0);
				ato++;
			}
			break;
		case 3:
			abejideSeta.LookAt ((new Vector3 (transform.position.x, abejideSeta.position.y, transform.position.z)));
			abejide.rotation = Quaternion.Lerp (abejide.rotation, abejideSeta.rotation, 2 * Time.deltaTime);

			temporizador += Time.deltaTime;
			if (Mathf.Abs(abejide.transform.eulerAngles.y - abejideSeta.eulerAngles.y) < 5 && temporizador > maxTemporizador) {
				ato++;
			}
			break;
		case 4:
			boca.GetComponent<Conversas> ().MudarRostoMostrar (rostoMostrarSuccubus);
			boca.GetComponent<Conversas> ().nome.text = "Succubus:";
			if (dadosDaFase.PegarIndioma ()) {
				fala = "Ei garotão.\nNós sentimos o seu cheiro....";
			} else {
				fala = "Ingles";
			}
			boca.GetComponent<Text> ().text = fala;
			boca.GetComponent<Conversas> ().MudarTempoLimparTexto (4f);
			boca.GetComponent<Conversas> ().conversas.enabled = true;
			temporizador = 0f;
			maxTemporizador = 4f;
			ato++;
			break;

		//A succubus começa a partir par acima do abejide.
		case 5:
			if (temporizador > maxTemporizador / 1.5f) {
				transform.position = Vector3.Lerp (transform.position, new Vector3 (abejide.position.x, transform.position.y, abejide.position.z), 0.25f * Time.deltaTime);
			}

			temporizador += Time.deltaTime;
			if (temporizador > maxTemporizador) {
				boca.GetComponent<Conversas> ().MudarRostoMostrar (rostoMostrarAbejide);
				boca.GetComponent<Conversas> ().nome.text = "Abejide:";
				if (dadosDaFase.PegarIndioma ()) {
					fala = "Não permitirei isso.";
				} else {
					fala = "Ingles";
				}
				boca.GetComponent<Text> ().text = fala;
				boca.GetComponent<Conversas> ().MudarTempoLimparTexto (4f);
				boca.GetComponent<Conversas> ().conversas.enabled = true;
				temporizador = 0f;
				maxTemporizador = 4f;
				ato++;
			}
			break;
		case 6:
			transform.position = Vector3.Lerp (transform.position, new Vector3 (abejide.position.x - 1f, transform.position.y, abejide.position.z), 0.25f * Time.deltaTime);

			temporizador += Time.deltaTime;
			if (temporizador > maxTemporizador) {
				ato++;
			}
			break;
		//Abejide apunhala a succubus.
		case 7:
			if (abejidePegouEspada) {
				abejide.GetComponentInChildren<Animator> ().SetBool ("Atacando", true);
				abejide.GetComponentInChildren<Animator> ().SetFloat ("VelocidadeAtacando", 2.3f);
			}
			temporizador = 0f;
			maxTemporizador = 0.2f;
			ato++;
			break;

		case 8:
			temporizador += Time.deltaTime;
			if (temporizador > maxTemporizador) {
				if (abejidePegouEspada) {
					marreu = true;
					atorAnimator.SetTrigger ("Morrendo");
					abejide.GetComponentInChildren<Animator> ().SetBool ("Atacando", false);
				} else {
					atorAnimator.SetTrigger ("Atacar");
					abejide.GetComponentInChildren<Animator> ().SetInteger ("IndiceGatilho", 999);
					abejide.GetComponentInChildren<Animator> ().SetTrigger ("Gatilho");
				}
				ato++;
			}
			break;
		case 9:
			temporizador = 0f;
			maxTemporizador = 3f;

			if (abejidePegouEspada) {
				MatarSuccubus ();
			} else {
				MatarAbejide ();
			}
			break;
		case 10:
			temporizador += Time.deltaTime;
			if (temporizador > maxTemporizador) {
				if (!abejidePegouEspada) {
					abejide.GetComponent<Abejide> ().vidas = 0;
				}
				StartCoroutine ("DestroirDados");
			}
			break;
		}
	}

	private void MatarAbejide () {
		ato++;
	}

	private void MatarSuccubus () {
		boca.GetComponent<Conversas> ().MudarRostoMostrar (rostoMostrarSuccubus);
		boca.GetComponent<Conversas> ().nome.text = "Succubus:";
		fala = "Ahhhhhh.";
		boca.GetComponent<Text> ().text = fala;
		boca.GetComponent<Conversas> ().MudarTempoLimparTexto (3f);
		boca.GetComponent<Conversas> ().conversas.enabled = true;
		ato++;
	}

	private void PularCutscene () {
		if (!marreu) {
			atorAnimator.SetTrigger ("Morrendo");
		}

		GetComponent<Animator> ().SetTrigger ("PularCutscene");
		StartCoroutine ("DestroirDados");
	}

	private void AcabouCutscene () {
		boca.enabled = false;
		boca.GetComponent<Conversas> ().conversas.enabled = false;

		Destroy (atorAnimator);
		Destroy (rostoMostrarSuccubus.gameObject);
		Destroy(GetComponent<SuccubusAto01> ());

		for (int i = 0; i < inimigosNormais.Length; i++) {
			inimigosNormais [i].rodandoCutscene = false;
		}

		abejide.GetComponent<Abejide> ().AtivarCodigoLula ();
	}

	IEnumerator DestroirDados () {
		yield return new WaitForSeconds (2);
		AcabouCutscene ();
	}
}
