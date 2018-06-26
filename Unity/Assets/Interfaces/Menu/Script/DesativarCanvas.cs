using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesativarCanvas : MonoBehaviour {

	public bool desativar = false;

	// Use this for initialization
	void Start () {
		GetComponent<Canvas> ().enabled = !desativar;
	}
}
