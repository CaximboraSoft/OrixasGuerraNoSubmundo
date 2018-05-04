using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentacaoTeste : MonoBehaviour {

	Rigidbody corpo;

//	Vector3 movimento;

	public float forca = 0;

	// Use this for initialization
	void Start () {
		corpo = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		//movimento = new Vector3 (0, 0, 1 * Time.deltaTime);

		if (Input.GetKey(KeyCode.W)) {
			corpo.AddForce (transform.forward * forca);
		}
	}
}
