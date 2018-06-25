using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MostrarPortao : MonoBehaviour {

	public Transform cameraPrincipal;
	public Transform cameraLugar;

	// Update is called once per frame
	void Update () {
		cameraPrincipal.GetComponent<seguirJogador> ().seta.LookAt (transform.position);
		cameraPrincipal.rotation = Quaternion.Lerp (cameraPrincipal.rotation, cameraPrincipal.GetComponent<seguirJogador> ().seta.rotation, 2 * Time.deltaTime);
		cameraPrincipal.position = Vector3.Lerp (cameraPrincipal.position, cameraLugar.position, 2 * Time.deltaTime);
	}


	public void FocarCameraNoAlvo () {
		cameraPrincipal.GetComponent<seguirJogador> ().enabled = false;
		GetComponent<MostrarPortao> ().enabled = true;
		StartCoroutine ("TempoMostrandoObjeto");
	}

	IEnumerator TempoMostrandoObjeto () {
		yield return new WaitForSeconds (3);
		cameraPrincipal.GetComponent<seguirJogador> ().AtivarComLula ();
		Destroy (GetComponent<MostrarPortao> ());
	}
}
