using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Esqueleto : MonoBehaviour {

	public Transform abejide;

	public float distancia;
	private float distanciaX;
	private float distanciaZ;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		distanciaX = abejide.position.x - transform.position.x;
		distanciaZ = abejide.position.z - transform.position.z;
		distancia = Mathf.Sqrt(Mathf.Pow(distanciaX, 2) + Mathf.Pow(distanciaZ, 2));

		transform.LookAt (new Vector3(abejide.position.x, transform.position.y, abejide.position.z));
	}
}
