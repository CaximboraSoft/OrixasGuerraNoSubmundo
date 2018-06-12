using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorDeixarTrasn : MonoBehaviour {

	private CameraDetectarColisao cameraDetectarColisaoScript;

	private bool deixarTransparente;

	// Use this for initialization
	void Start () {
		deixarTransparente = false;

		GameObject cameraTemp = GameObject.Find ("Main Camera");
		cameraDetectarColisaoScript = cameraTemp.GetComponent<CameraDetectarColisao> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (cameraDetectarColisaoScript.hitPoint.transform == transform) {
			if (!deixarTransparente) {
				DeixarTransparente[] corredor = GetComponentsInChildren<DeixarTransparente> ();
				for (int i = 0; i < corredor.Length; i++) {
					corredor [i].enabled = true;
					corredor [i].deixarTransparente = true;
				}

				deixarTransparente = true;
			}
		} else {
			if (deixarTransparente) {
				DeixarTransparente[] corredor = GetComponentsInChildren<DeixarTransparente> ();
				for (int i = 0; i < corredor.Length; i++) {
					corredor [i].deixarTransparente = false;
				}

				deixarTransparente = false;
			}
		}
	}
}
