using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbejideMagia : MonoBehaviour {

	public ParticleSystem parcicula;

	public float massa;
	public float addNewton;
	public float maxNewton;
	private float aceleracao;
	private float newton;
	public float maxDistancia;

	private float inicialX;
	private float inicialZ;

	private float distancia;
	private float distanciaX;
	private float distanciaZ;

	// Use this for initialization
	void Start () {
		newton = 0;

		inicialX = transform.position.x;
		inicialZ = transform.position.z;
	}
	
	// Update is called once per frame
	void Update () {
		distanciaX = transform.position.x - inicialX;
		distanciaZ = transform.position.z - inicialZ;
		distancia = Mathf.Sqrt(Mathf.Pow(distanciaX, 2) + Mathf.Pow(distanciaZ, 2));

		if (distancia > maxDistancia) {
			Destroy(gameObject);
		}

		if (newton < maxNewton) {
			newton += addNewton;

			if (newton > maxNewton) {
				newton = maxNewton;
			}
		}

		aceleracao = (newton / massa);
		transform.Translate (0, 0, aceleracao * Time.deltaTime);
	}
}
