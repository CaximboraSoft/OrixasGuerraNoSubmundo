using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeixarTransparente : MonoBehaviour {

	private CameraDetectarColisao cameraDetectarColisaoScript;
	private Renderer renderMaterial = new Renderer ();
	private Shader shaderOriginal;
	private Shader shaderTransparente;

	// Use this for initialization
	void Start () {
		GameObject cameraTemp = GameObject.Find ("Main Camera");
		cameraDetectarColisaoScript = cameraTemp.GetComponent<CameraDetectarColisao> ();

		renderMaterial = gameObject.GetComponent<Renderer> ();
		shaderOriginal = renderMaterial.material.shader;

		shaderTransparente = Shader.Find ("Custom/Tranparente");
	}
	
	// Update is called once per frame
	void Update () {
		for (int linha = 0; linha < renderMaterial.materials.Length; linha++) {
			if (cameraDetectarColisaoScript.hitPoint.transform == transform) {
				if (renderMaterial.materials[linha].color.a > 0.5f) {
					Color cor = renderMaterial.materials [linha].color;
					cor.a -= 0.5f;

					renderMaterial.materials [linha].color = cor;
					renderMaterial.materials[linha].shader = shaderTransparente;
				}
			} else if (renderMaterial.materials[linha].color.a < 1) {
				Color cor = renderMaterial.materials [linha].color;
				cor.a += 0.5f;

				renderMaterial.materials [linha].color = cor;
				renderMaterial.materials[linha].shader = shaderOriginal;
			}
		}
	}
}
