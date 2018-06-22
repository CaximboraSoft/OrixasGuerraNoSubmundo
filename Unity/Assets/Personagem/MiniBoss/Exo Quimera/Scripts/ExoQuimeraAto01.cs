using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ExoQuimeraAto01 : MonoBehaviour {

	public Transform abejideSeta;
	public Transform lugarDoAbejide;
	public Transform rostoMostrarExoQuimera;
	public Transform rostoMostrarAbejide;
	private Transform abejide;
	private InimigosNormais[] inimigosNormais;

	public Conversas boca;

	private float temporizador;
	private float maxTemporizador;

	private string fala;

	private int ato = 0;

	// Use this for initialization
	void Start () {
		abejide = GameObject.FindGameObjectWithTag ("Abejide").transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (ato > 0 && Input.GetKeyDown (KeyCode.Return)) {
			abejide.GetComponentInChildren<Animator> ().SetInteger ("IndiceGatilho", -1);
			abejide.GetComponentInChildren<Animator> ().SetTrigger ("Gatilho");
			AcabouCutScene ();
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
				abejide.GetComponentInChildren<Animator> ().SetInteger ("IndiceGatilho", 1);
				abejide.GetComponentInChildren<Animator> ().SetTrigger ("Gatilho");

				boca.enabled = true;
				boca.GetComponent<Conversas> ().MudarRostoMostrar (rostoMostrarExoQuimera);
				fala = "Oque o trouxe até aqui. garoto tolo?";
				boca.GetComponent<Conversas> ().nome.text = "Exo Quimera:";
				boca.GetComponent<Text> ().text = fala;
				boca.GetComponent<Conversas> ().MudarTempoLimparTexto (4.5f);
				boca.GetComponent<Conversas> ().conversas.enabled = true;
				temporizador = 0;
				maxTemporizador = 4.5f;
				abejide.GetComponentInChildren<Animator> ().SetFloat ("Velocidade", 0);
				ato++;
			}
			break;
		case 3:
			temporizador += Time.deltaTime;
			if (temporizador > maxTemporizador) {
				fala = "Mal consegue ficar de pé. Tudo o que você conheceu ou viveu para lutar já pereceu. Não há nada para ti, rapaz.";
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
				boca.GetComponent<Conversas> ().nome.text = "Abejide:";
				boca.GetComponent<Conversas> ().MudarRostoMostrar (rostoMostrarAbejide);
				fala = "Tudo o que eu passei até aqui... Você tem razão. Perdi tudo o que tinha.";
				boca.GetComponent<Text> ().text = fala;
				boca.GetComponent<Conversas> ().MudarTempoLimparTexto (4f);
				boca.GetComponent<Conversas> ().conversas.enabled = true;
				temporizador = 0f;
				maxTemporizador = 4f;
				ato++;
			}
			break;
		//Abejide finca a espada no chão e se levanta
		case 5:
			temporizador += Time.deltaTime;
			if (temporizador > maxTemporizador) {
				abejide.GetComponentInChildren<Animator> ().SetInteger ("IndiceGatilho", 2);
				abejide.GetComponentInChildren<Animator> ().SetTrigger ("Gatilho");
				temporizador = 0f;
				maxTemporizador = 2f;
				ato++;
			}
			break;
		case 6:
			abejideSeta.LookAt ((new Vector3 (transform.position.x, abejideSeta.position.y, transform.position.z)));
			abejide.rotation = Quaternion.Lerp (abejide.rotation, abejideSeta.rotation, 2 * Time.deltaTime);

			temporizador += Time.deltaTime;
			if (temporizador > maxTemporizador) {
				fala = "Estou sempre numa situação tão ruim como essa. No entanto..";
				boca.GetComponent<Text> ().text = fala;
				boca.GetComponent<Conversas> ().MudarTempoLimparTexto (5f);
				boca.GetComponent<Conversas> ().conversas.enabled = true;
				temporizador = 0f;
				maxTemporizador = 5f;
				ato++;
			}
			break;
		case 7:
			temporizador += Time.deltaTime;
			if (temporizador > maxTemporizador) {
				fala = "Vou continuar seguindo meu caminho. Eu tenho um propósito e irei cumpri-lo.";
				boca.GetComponent<Text> ().text = fala;
				boca.GetComponent<Conversas> ().MudarTempoLimparTexto (6f);
				boca.GetComponent<Conversas> ().conversas.enabled = true;
				temporizador = 0f;
				maxTemporizador = 6f;
				ato++;
			}
			break;
		case 8:
			temporizador += Time.deltaTime;
			if (temporizador > maxTemporizador) {
				fala = "Porque é a única coisa que me mantém vivo em meio a esse caos. Não importa quantos obstáculos surjam...";
				boca.GetComponent<Text> ().text = fala;
				boca.GetComponent<Conversas> ().MudarTempoLimparTexto (7f);
				boca.GetComponent<Conversas> ().conversas.enabled = true;
				temporizador = 0f;
				maxTemporizador = 7f;
				ato++;
			}
			break;
		case 9:
			temporizador += Time.deltaTime;
			if (temporizador > maxTemporizador) {
				fala = "É só isso que me resta em vida, algo que vocês monstros jamais entenderiam!! Vou matar todos!!!";
				boca.GetComponent<Text> ().text = fala;
				boca.GetComponent<Conversas> ().MudarTempoLimparTexto (6.5f);
				boca.GetComponent<Conversas> ().conversas.enabled = true;
				temporizador = 0f;
				maxTemporizador = 6.5f;
				ato++;
			}
			break;
		case 10:
			temporizador += Time.deltaTime;
			if (temporizador > maxTemporizador) {
				AcabouCutScene ();
			}
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
		GetComponent<MiniBoss> ().enabled = true;
		GetComponent<MiniBoss> ().viuAbejide = true;

		Destroy (GetComponent<ExoQuimeraAto01> ());
	}
}
