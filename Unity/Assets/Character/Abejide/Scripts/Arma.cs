using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arma : MonoBehaviour {

	public float dano = 0;
	public float peso = 0;
	//0 nenhum elemento | 1 água | 2 fogo | 3 terra
	public int elemento = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public float PegarDano() {
		return dano;
	}

	public float PegarPeso() {
		return peso;
	}

	public int PegarElemento() {
		return elemento;
	}
}
