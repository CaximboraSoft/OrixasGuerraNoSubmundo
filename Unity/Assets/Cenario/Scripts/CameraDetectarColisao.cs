using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDetectarColisao : MonoBehaviour {

	public Transform alvo;
	//public Transform partidaRaio;
	private Vector3 partidaRaio;
	public RaycastHit hitPoint = new RaycastHit();

	public float distanciaZ;
	public float distanciaY;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		partidaRaio.Set (transform.position.x, transform.position.y + distanciaY, transform.position.z - distanciaZ);

		if (Physics.Linecast (partidaRaio, alvo.position, out hitPoint)) {
			Debug.DrawLine (partidaRaio, alvo.position, Color.yellow);
		}
	}
}
