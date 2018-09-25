using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class SeteEncruzilhadasAto03 : MonoBehaviour {

	private Animator meuAnimator;
	public RuntimeAnimatorController controllerOriginal;

	public Transform abejideSeta;
	public Transform lugarDoAbejide;
	public Transform rostoMostrarExuVelho;
	public Transform rostoMostrarAbejide;
	private Transform abejide;
	private InimigosNormais[] inimigosNormais;
	private DadosDaFase dadosDaFase;

	public Conversas boca;

	private float temporizador;
	private float maxTemporizador;

	private string fala;

	private int ato = 0;

	void Start () {
		dadosDaFase = FindObjectOfType<DadosDaFase> ();
	}

	public void AtivarCodigo () {
		meuAnimator = GetComponentInChildren<Animator> ();
		meuAnimator.runtimeAnimatorController = controllerOriginal as RuntimeAnimatorController;
		abejide = GameObject.FindGameObjectWithTag ("Abejide").transform;
		GetComponent<SeteEncruzilhadasAto03> ().enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (ato > 0 && Input.GetKeyDown (KeyCode.Return)) {
			AcabouCutScene ();
		}

		switch (ato) {
		case 0:
			if (Vector3.Distance (transform.position, abejide.position) < 25f) {
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
			abejide.position = Vector3.Lerp (abejide.position, lugarDoAbejide.position, 0.65f * Time.deltaTime);

			if (Vector3.Distance (lugarDoAbejide.position, abejide.position) < 1f) {
				boca.enabled = true;
				boca.GetComponent<Conversas> ().MudarRostoMostrar (rostoMostrarExuVelho);
				if (dadosDaFase.PegarIndioma ()) {
					fala = "Muito bem, meu rapaz. Tu mataste a Quimera, as súcubus e se livrara das caveiras.";
				} else {
					fala = "Ingles";
				}
				boca.GetComponent<Conversas> ().nome.text = "Exu Velho:";
				boca.GetComponent<Text> ().text = fala;
				boca.GetComponent<Conversas> ().MudarTempoLimparTexto (6.5f);
				boca.GetComponent<Conversas> ().conversas.enabled = true;
				temporizador = 0;
				maxTemporizador = 6.5f;
				abejide.GetComponentInChildren<Animator> ().SetFloat ("Velocidade", 0);
				ato++;
			}
			break;
		case 3:
			abejideSeta.LookAt ((new Vector3 (transform.position.x, abejideSeta.position.y, transform.position.z)));
			abejide.rotation = Quaternion.Lerp (abejide.rotation, abejideSeta.rotation, 3 * Time.deltaTime);

			temporizador += Time.deltaTime;
			if (temporizador > maxTemporizador) {
				if (dadosDaFase.PegarIndioma ()) {
					fala = "No entanto... tudo que é bom dura pouco. Não penso mais em torturá-lo. Agora vou te matar, e que se danem meus colegas.";
				} else {
					fala = "Ingles";
				}
				boca.GetComponent<Text> ().text = fala;
				boca.GetComponent<Conversas> ().MudarTempoLimparTexto (8f);
				boca.GetComponent<Conversas> ().conversas.enabled = true;
				temporizador = 0f;
				maxTemporizador = 8f;
				ato++;
			}
			break;
		case 4:
			temporizador += Time.deltaTime;
			if (temporizador > maxTemporizador) {
				if (dadosDaFase.PegarIndioma ()) {
					fala = "Pois tu já és uma ameaça de alto nível. O meu reino será o seu túmulo.";
				} else {
					fala = "Ingles";
				}
				boca.GetComponent<Text> ().text = fala;
				boca.GetComponent<Conversas> ().MudarTempoLimparTexto (6.5f);
				boca.GetComponent<Conversas> ().conversas.enabled = true;
				temporizador = 0f;
				maxTemporizador = 6.5f;
				ato++;
			}
			break;
		case 5:
			temporizador += Time.deltaTime;
			if (temporizador > maxTemporizador) {
				boca.GetComponent<Conversas> ().nome.text = "Abejide:";
				boca.GetComponent<Conversas> ().MudarRostoMostrar (rostoMostrarAbejide);
				if (dadosDaFase.PegarIndioma ()) {
					fala = "Não... Não vai não.\nTeu reino é que será tua tumba, monstruosidade!";
				} else {
					fala = "Ingles";
				}
				boca.GetComponent<Text> ().text = fala;
				boca.GetComponent<Conversas> ().MudarTempoLimparTexto (6.5f);
				boca.GetComponent<Conversas> ().conversas.enabled = true;
				temporizador = 0f;
				maxTemporizador = 6.5f;
				ato++;
			}
			break;
		case 6:
			temporizador += Time.deltaTime;
			if (temporizador > maxTemporizador) {
				boca.GetComponent<Conversas> ().nome.text = "Exu Velho:";
				boca.GetComponent<Conversas> ().MudarRostoMostrar (rostoMostrarExuVelho);
				if (dadosDaFase.PegarIndioma ()) {
					fala = "Muito bem então... Que duelemos!";
				} else {
					fala = "Ingles";
				}
				boca.GetComponent<Text> ().text = fala;
				boca.GetComponent<Conversas> ().MudarTempoLimparTexto (4.5f);
				boca.GetComponent<Conversas> ().conversas.enabled = true;
				temporizador = 0f;
				maxTemporizador = 4.5f;
				ato++;
			}
			break;
		case 7:
			AcabouCutScene ();
			break;
		}
	}

	void AcabouCutScene () {
		for (int i = 0; i < inimigosNormais.Length; i++) {
			inimigosNormais [i].rodandoCutscene = false;
		}

		boca.enabled = false;
		boca.GetComponent<Conversas> ().conversas.enabled = false;

		abejide.GetComponent<Abejide> ().AtivarCodigoLula ();
		GetComponent<Boss> ().AtivarCodigo ();

		Destroy (GetComponent<SeteEncruzilhadasAto03> ());
	}
}
