using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS : MonoBehaviour {
	public float frameCount = 0;
	public float deltaTimee = 0.0f;
	public float fps = 0.0f;
	public float updateRate = 4.0f; // VEZES POR SEGUNTO QUE O FPS IRA APARECER NA TELA
	public Font fonte;

	void Update (){
		frameCount++;
		deltaTimee += Time.deltaTime;

		if (deltaTimee > 1.0f/updateRate){
			fps = frameCount / deltaTimee ;
			frameCount = 0;
			deltaTimee -= 1.0f/updateRate;
		}
	}

	void OnGUI (){
		GUI.skin.font = fonte;
		GUI.Label (new Rect (2, 0, Screen.width / 6,  Screen.height / 12), "FPS: " + Mathf.RoundToInt(fps + 4));
	}
}