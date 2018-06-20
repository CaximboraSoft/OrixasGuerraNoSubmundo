using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeixarTransparente : MonoBehaviour {

	public Renderer renderMaterial = new Renderer ();
	public Material materialTransparente;
	public Material[] materialOriginal;

	public bool deixarTransparente;

	// Use this for initialization
	void Awake () {
		deixarTransparente = false;

		materialTransparente = Resources.Load ("Materiais/Transparente", typeof(Material)) as Material;

		renderMaterial = GetComponent<Renderer> ();

		materialOriginal = GetComponent<Renderer> ().materials;
	}
	
	// Update is called once per frame
	void Update () {
		for (int linha = 0; linha < renderMaterial.materials.Length; linha++) {
			if (deixarTransparente) {
				CarrecarMaterialTransparente (false);
			} else {
				CarrecarMaterialTransparente (true);
				GetComponent<DeixarTransparente> ().enabled = false;
			}
		}
	}

	private void CarrecarMaterialTransparente (bool original) {
		Material[] materialTemporario;
		materialTemporario = new Material[materialOriginal.Length];

		if (!original) {
			for (int i = 0; i < materialTemporario.Length; i++) {
				materialTemporario [i] = materialTransparente;
			}
		} else {
			for (int i = 0; i < materialTemporario.Length; i++) {
				materialTemporario [i] = materialOriginal[i];
			}
		}

		GetComponent<Renderer> ().materials = materialTemporario;
	}
}
