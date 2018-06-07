using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedraGigante : MonoBehaviour {

	public FazerPedrasVoar[] filhosFazerPedrasVoar;

	// Use this for initialization
	void Start () {
		filhosFazerPedrasVoar = GetComponentsInChildren<FazerPedrasVoar> ();
	}

	public void ExplodirPedras () {
		for (int linha = 0; linha < filhosFazerPedrasVoar.Length; linha++) {
			transform.SetParent (null);
			filhosFazerPedrasVoar [linha].AplicarForcaNasPedrsa ();
		}
	}
}
