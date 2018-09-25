using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class SeteEncruzilhadasAto04 : MonoBehaviour {


	public Transform rostoMostrarExuVelho;
	public Transform rostoMostrarAbejide;
	private DadosDaFase dadosDaFase;

	public Conversas boca;

	private int ato = 0;

	private float temporizador;
	private float maxTemporizador;

	private string fala;
	// Use this for initialization
	void Awake () {
		dadosDaFase = FindObjectOfType<DadosDaFase> ();
	}

	// Update is called once per frame
	void Update () {
		switch (ato) {
		case 0:
			InimigosNormais[] inimigosNormais = FindObjectsOfType <InimigosNormais> ();
			for (int i = 0; i < inimigosNormais.Length; i++) {
				inimigosNormais[i].enabled = false;
			}

			MiniBoss[] miniBoss = FindObjectsOfType <MiniBoss> ();
			for (int i = 0; i < miniBoss.Length; i++) {
				miniBoss[i].enabled = false;
			}

			Abejide abejide = FindObjectOfType <Abejide> ();
			abejide.enabled = false;
			abejide.GetComponentInChildren<Animator> ().SetFloat ("Velocidade", 0);

			boca.enabled = true;
			boca.GetComponent<Conversas> ().MudarRostoMostrar (rostoMostrarExuVelho);
			if (dadosDaFase.PegarIndioma ()) {
				fala = "Maldito moleque!, você pagara por isso!";
			} else {
				fala = "Ingles";
			}
			boca.GetComponent<Conversas> ().nome.text = "Exu Velho:";
			boca.GetComponent<Text> ().text = fala;
			boca.GetComponent<Conversas> ().MudarTempoLimparTexto (6.5f);
			boca.GetComponent<Conversas> ().conversas.enabled = true;
			temporizador = 0;
			maxTemporizador = 4f;
			ato++;
			break;
		case 1:
			temporizador += Time.deltaTime;
			if (temporizador > maxTemporizador) {
				boca.GetComponent<Conversas> ().nome.text = "Abejide:";
				boca.GetComponent<Conversas> ().MudarRostoMostrar (rostoMostrarAbejide);
				if (dadosDaFase.PegarIndioma ()) {
					fala = "É melhor que tu fiques por bem, meu velho, já causou poblemas demais.";
				} else {
					fala = "Ingles";
				}
				boca.GetComponent<Text> ().text = fala;
				boca.GetComponent<Conversas> ().MudarTempoLimparTexto (8f);
				boca.GetComponent<Conversas> ().conversas.enabled = true;
				temporizador = 0f;
				maxTemporizador = 5.5f;
				ato++;
			}
			break;
		case 2:
			temporizador += Time.deltaTime;
			if (temporizador > maxTemporizador) {
				boca.GetComponent<Conversas> ().nome.text = "Exu Velho:";
				boca.GetComponent<Conversas> ().MudarRostoMostrar (rostoMostrarExuVelho);
				if (dadosDaFase.PegarIndioma ()) {
					fala = "Filho de uma...";
				} else {
					fala = "Ingles";
				}
				boca.GetComponent<Text> ().text = fala;
				boca.GetComponent<Conversas> ().MudarTempoLimparTexto (8f);
				boca.GetComponent<Conversas> ().conversas.enabled = true;
				temporizador = 0f;
				maxTemporizador = 2f;
				ato++;
			}
			break;
		case 3:
			temporizador += Time.deltaTime;
			if (temporizador > maxTemporizador) {
				FindObjectOfType <Conversas> ().conversas.enabled = false;
				temporizador = 0f;
				maxTemporizador = 1f;
				ato++;
			}
			break;
		case 4:
			temporizador += Time.deltaTime;
			if (temporizador > maxTemporizador) {
				AcabouCutscene ();
			}
			break;
		}
	}

	private void AcabouCutscene () {
		Destroy (GetComponent <SeteEncruzilhadasAto04> ());
		FindObjectOfType<LevelManager> ().MostrarTelaFinal ();
	}
}
