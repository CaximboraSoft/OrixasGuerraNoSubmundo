using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Esqueleto : MonoBehaviour {

	private GameObject abejide;

	public float distancia;
	private float distanciaX;
	private float distanciaZ;


	// Use this for initialization
	void Start () {
		abejide = GameObject.Find ("Abejide");
	}
	
	// Update is called once per frame
	void Update () {
		distanciaX = abejide.transform.position.x - transform.position.x;
		distanciaZ = abejide.transform.position.z - transform.position.z;
		distancia = Mathf.Sqrt(Mathf.Pow(distanciaX, 2) + Mathf.Pow(distanciaZ, 2));

		transform.LookAt (new Vector3(abejide.transform.position.x, transform.position.y, abejide.transform.position.z));
	}
}
