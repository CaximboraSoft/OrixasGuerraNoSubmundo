using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbejideChao : MonoBehaviour {

	public bool bateuNoChao;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter (Collider other) {
		bateuNoChao = true;
	}
}
