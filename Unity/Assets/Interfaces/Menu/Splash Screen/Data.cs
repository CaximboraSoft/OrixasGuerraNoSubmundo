using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour {

	public static int portugues = 1;
	private GameObject[] datas;

	void Awake () {
		datas = GameObject.FindGameObjectsWithTag ("Data");

		if (datas.Length >= 2) {
			Destroy (datas [0]);
		}

		DontDestroyOnLoad (transform.gameObject);
	}

	void Start () {
		if (PlayerPrefs.HasKey ("Indioma")) {
			portugues = PlayerPrefs.GetInt ("Indioma");
		} else {
			PlayerPrefs.SetInt ("Indioma", portugues);
		}
	}
}
