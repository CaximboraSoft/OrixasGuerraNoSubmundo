using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColocarDentro : MonoBehaviour {

	public Transform objeto;

	// Use this for initialization
	void Start () {
		if (objeto != null) {
			objeto.SetParent (transform);
			objeto.localPosition = Vector3.zero;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
