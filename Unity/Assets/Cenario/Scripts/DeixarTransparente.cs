using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeixarTransparente : MonoBehaviour {

	private Renderer renderMaterial = new Renderer ();
	private Shader shaderOriginal;
	private Shader shaderTransparente;

	public bool deixarTransparente;

	// Use this for initialization
	void Awake () {
		deixarTransparente = false;

		renderMaterial = gameObject.GetComponent<Renderer> ();
		shaderOriginal = renderMaterial.material.shader;

		shaderTransparente = Shader.Find ("Custom/Tranparente");

		for (int linha = 0; linha < renderMaterial.materials.Length; linha++) {
			Color cor = renderMaterial.materials [linha].color;
			cor.a = 0.5f;

			renderMaterial.materials [linha].color = cor;
		}
	}
	
	// Update is called once per frame
	void Update () {
		for (int linha = 0; linha < renderMaterial.materials.Length; linha++) {
			if (deixarTransparente) {
				renderMaterial.materials[linha].shader = shaderTransparente;
			} else {
				renderMaterial.materials[linha].shader = shaderOriginal;
				GetComponent<DeixarTransparente> ().enabled = false;
			}
		}
	}
}
