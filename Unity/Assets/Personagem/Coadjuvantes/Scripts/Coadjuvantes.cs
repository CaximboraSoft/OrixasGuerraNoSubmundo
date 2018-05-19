using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coadjuvantes : MonoBehaviour {

	public int ato;

	// Use this for initialization
	void Start () {
		Animator temp = GetComponent<Animator> ();
		temp.SetInteger ("Ato", ato);
		temp.SetTrigger ("MudarAto");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
