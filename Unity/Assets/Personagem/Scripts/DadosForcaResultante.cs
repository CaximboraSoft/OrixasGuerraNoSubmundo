using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadosForcaResultante : MonoBehaviour {

	public float massa;
	public float armaMassa;
	public float escudoMassa;

	//força(newton) = massa(kg) * aceleração(m/s²)
	public float newtonAndando; 
	public float addNewtonAndando;
	public float subNewtonAndando;
	public float maxNewtonAndando;
	public float maxNewtonCorrendo;

	public float newtonRodando;
	public float addNewtonRodando;
	public float maxNewtonRodando;

	void Start () {
		newtonAndando = 0;
		newtonRodando = 0;

		GetComponent<Rigidbody> ().mass = PegarMassaTotal ();
	}

	public float PegarMassaTotal () {
		return (massa + armaMassa + escudoMassa);
	}

	/*--------------------- newtom andando ---------------------*/
	public void AddNewtonAndando () {
		newtonAndando += addNewtonAndando;

		if (newtonAndando > maxNewtonAndando) {

			newtonAndando -= subNewtonAndando;
			if (newtonAndando < maxNewtonAndando) {
				newtonAndando = maxNewtonAndando;
			}
		}
	}

	public void AddNewtonCorrendo () {
		newtonAndando += addNewtonAndando;

		if (newtonAndando > maxNewtonCorrendo + maxNewtonAndando) {
			newtonAndando = maxNewtonCorrendo + maxNewtonAndando;
		}
	}

	public void SubNewtonAndando (float vezes) {
		newtonAndando -= (subNewtonAndando * vezes);

		if (newtonAndando < 0) {
			newtonAndando = 0;
		}
	}

	public void MudarNewtonAndando (float valor) {
		newtonAndando = valor;
	}

	public float PegarNewtonAndando () {
		return newtonAndando;
	}

	public float PegarAceleracaoAndando () {
		return (newtonAndando / PegarMassaTotal ());
	}


	/*--------------------- newtom andando ---------------------*/
	public void AddNewtonRodando () {
		newtonRodando += addNewtonRodando;

		if (newtonRodando > maxNewtonRodando) {
			newtonRodando = maxNewtonRodando;
		}
	}

	public void MudarNewtonRodando (float valor) {
		newtonRodando = valor;
	}

	public float PegarNewtonRodando () {
		return newtonRodando;
	}

	public float PegarAceleracaoRodando () {
		return (((newtonRodando + newtonAndando) / PegarMassaTotal ()) * 45) * Time.deltaTime;
	}
}
