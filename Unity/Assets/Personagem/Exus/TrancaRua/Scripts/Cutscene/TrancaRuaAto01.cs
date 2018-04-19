using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrancaRuaAto01 : MonoBehaviour {

	public GameObject terreno;
	public Animator atorAnimator;
	public RuntimeAnimatorController controllerInicial;
	public RuntimeAnimatorController controllerCutscene;

	private Transform objeto;
	public string posicoesNome;
	public float[] rotacoes;

	private float tempoEspera;
	private float maxTempoEspera;

	public int ato;
	private int indicePosicoes;
	private int indiceRotacoes;

	// Use this for initialization
	void Start () {
		atorAnimator.runtimeAnimatorController = controllerCutscene as RuntimeAnimatorController;

		ato = 0;
		indicePosicoes = 0;
		indiceRotacoes = 0;
		tempoEspera = 10;
		maxTempoEspera = 0;

		objeto = GameObject.Find(posicoesNome + " (" + indicePosicoes.ToString () + ")").GetComponent<Transform> ();
		transform.position = objeto.position;
		transform.eulerAngles = new Vector3 (transform.eulerAngles.x, rotacoes[0], transform.eulerAngles.z);
	}
	
	// Update is called once per frame
	void Update () {
		atorAnimator.SetFloat ("NewtonAndando", GetComponent<MetodosDaCutscene> ().PegarNewtonAndando ());

		if (!GetComponent<MetodosDaCutscene> ().PegarEstaAtuando () && tempoEspera > maxTempoEspera) {

			switch (ato) {
			case 0:
				if (terreno.GetComponent<DadosDaFase>().atores[0].GetComponent<AbejideAto01>().PegarAto () > 1) {
					ato++;
				}
				break;
			case 1:
				tempoEspera = 0;
				maxTempoEspera = 2;
				ato++;
				break;
			case 2:
				MudarPosicao (true, false, false, false);
				ato++;
				break;
			case 3:
				tempoEspera = 0;
				maxTempoEspera = 2;
				ato++;
				break;
			case 4:
				MudarRotacao (false, true);
				ato++;
				break;
			case 5:
				MudarPosicao (false, false, true, false);
				ato++;
				break;
			case 6:
				MudarPosicao (false, false, false, false);
				ato++;
				break;
			case 7:
				MudarRotacao (false, false);
				ato++;
				break;
			case 8:
				tempoEspera = 0;
				maxTempoEspera = 3;
				atorAnimator.SetInteger ("Ato", ato);
				atorAnimator.SetTrigger ("MudarAnimacao");
				ato++;
				break;
			case 9:
				if (terreno.GetComponent<DadosDaFase>().atores[2].GetComponent<SeteEncruzilhadasAto01>().PegarAto () > 2) {
					ato++;
				}
				break;
			case 10:
				MudarRotacao (true, true);
				ato++;
				break;
			case 11:
				MudarPosicao (true, false, false, false);
				ato++;
				break;
			case 12:
				MudarRotacao (true, false);
				ato++;
				break;
			default:
				//ato--;
				/*
				GetComponent<Ato01> ().enabled = false;
				GetComponent<MetodosDaCutscene> ().enabled = false;
				GetComponent<AbejideAtaque> ().enabled = true;
				GetComponent<AbejideAndando> ().enabled = true;*/
				break;
			}
		} else {
			tempoEspera += Time.deltaTime;
		}
	}

	private void MoverNoEixoY(float distancia, bool cima, bool correndo) {
		GetComponent<MetodosDaCutscene> ().ComecarAtuacaoMoverNoEixoY (distancia, cima, correndo);
	}

	private void ApontarParaObjeto (bool sentidoHorario, bool manterNewtonRotacao) {
		GetComponent<MetodosDaCutscene> ().ComecarAtuacaoOlharParaObjeto (objeto, sentidoHorario, manterNewtonRotacao);
	}

	private void MudarRotacao (bool sentidoHorario, bool manterNewtonRotacao) {
		indiceRotacoes++;
		GetComponent<MetodosDaCutscene> ().ComecarAtuacaoRotacao (rotacoes[indiceRotacoes], sentidoHorario, manterNewtonRotacao);
	}

	private void MudarPosicao (bool sentidoHorario, bool correndo, bool manterNewtonAndando, bool manterNewtonRodando) {
		indicePosicoes++;
		objeto = GameObject.Find(posicoesNome + " (" + indicePosicoes.ToString () + ")").GetComponent<Transform> ();
		GetComponent<MetodosDaCutscene> ().ComecarAtuacaoPosicao (objeto, sentidoHorario, correndo, manterNewtonAndando, manterNewtonRodando);
	}

	public int PegarAto () {
		return ato;
	}
}
