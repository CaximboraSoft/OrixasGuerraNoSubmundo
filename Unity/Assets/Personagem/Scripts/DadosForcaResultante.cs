using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadosForcaResultante : MonoBehaviour {

	public float massa;
	public float armaMassa;
	public float escudoMassa;

	public float addNewtonAndando;
	public float subNewtonAndando;
	public float maxNewtonAndando;
	public float maxNewtonCorrendo;

	public float addNewtonRodando;
	public float maxNewtonRodando;

	public  float PegarMassaTotal () {
		return (massa + armaMassa + escudoMassa);
	}
}
