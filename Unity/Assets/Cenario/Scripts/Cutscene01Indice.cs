using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene01Indice : MonoBehaviour {

	public GameObject[] atores;

	public bool acabouAtuacao;

	public int ato;
	private int linha;

	/*
	 * qualTipoDeAtuacao:
	 *     1 Muda a posicao
	 *     2 Muda a animação
	 *     3 Muda a rotação
	 */

	// Use this for initialization
	void Start () {
		ato = 1;

		acabouAtuacao = true;
	}
	
	// Update is called once per frame
	void Update () {

		if (acabouAtuacao) {
			acabouAtuacao = false;

			switch (ato) {
			case 1:
				IniciarAtuacao (1); //Posição
				break;
			case 2:
				IniciarAtuacao (3); //Posição
				break;
			case 3:
				IniciarAtuacao (1); //Posição
				break;
			case 4:
				IniciarAtuacao (1); //Posição
				break;
			default:
				for (linha = 0; linha < atores.Length; linha++) {
					atores [linha].GetComponent<CutscenePersonagens> ().enabled = false;
				}

				atores [0].GetComponent<AbejideAtaque> ().enabled = true;
				atores [0].GetComponent<AbejideAndando> ().enabled = true;
				GetComponent<Cutscene01Indice> ().enabled = false;
				break;
			}

			ato++;
		}

		acabouAtuacao = true;
		for (linha = 0; linha < atores.Length; linha++) {
			if (!atores[linha].GetComponent<CutscenePersonagens>().PegarAcabouDeAtuar()) {
				acabouAtuacao = false;
				break;
			}
		}
	}

	private void IniciarAtuacao (int qualTipoDeAtuacao) {
		for (linha = 0; linha < atores.Length; linha++) {
			atores [linha].GetComponent<CutscenePersonagens> ().MudarParametros (qualTipoDeAtuacao);
		}

		//Passa a massa do Abejide
		atores [0].GetComponent<CutscenePersonagens> ().MudarMassa (atores [0].GetComponent<AbejideAtaque>().PegarMassaTotal ());
		atores [1].GetComponent<CutscenePersonagens> ().MudarMassa (atores [1].GetComponent<DadosForcaResultante>().PegarMassaTotal ());
	}
}
