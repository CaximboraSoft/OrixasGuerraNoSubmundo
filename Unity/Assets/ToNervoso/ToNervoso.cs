using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToNervoso : MonoBehaviour {

	public Transform olhor;
	public Transform ondeEstou;

	public Transform olho;
	public Transform olharParaEsseLixo;

	public float anguloChegar;
	public float anguloQueEstou;

	public bool sentidoHorario = false;
	public bool chegouNoAngulo = false;

	public float angulo;

	// Use this for initialization
	void Start () {
		olho.LookAt (new Vector3(olharParaEsseLixo.position.x, olho.position.y, olharParaEsseLixo.position.z));

		anguloChegar = olho.eulerAngles.y;
		anguloQueEstou = transform.eulerAngles.y;

		anguloChegar -= anguloQueEstou;

		olhor.eulerAngles = new Vector3 (olhor.eulerAngles.x, anguloChegar, olhor.eulerAngles.z);
		ondeEstou.eulerAngles = new Vector3 (ondeEstou.eulerAngles.x, 0, ondeEstou.eulerAngles.z);

		if (anguloChegar < 0) {
			anguloChegar += 360;
		}

		sentidoHorario = true;
		if (anguloChegar > 180) {
			sentidoHorario = false;
		}

		angulo = 0;
	}

	// Update is called once per frame
	void Update () {
		if (!chegouNoAngulo) {
			if (sentidoHorario) {
				angulo -= 40 * Time.deltaTime;

				if (angulo + anguloChegar < 0) {
					chegouNoAngulo = true;
				}
			} else {
				angulo += 40 * Time.deltaTime;

				if (angulo + anguloChegar > 360) {
					chegouNoAngulo = true;
				}
			}
		}

		olhor.eulerAngles = new Vector3 (olhor.eulerAngles.x, anguloChegar + angulo, olhor.eulerAngles.z);
		transform.eulerAngles = new Vector3 (transform.eulerAngles.x, anguloQueEstou - angulo, transform.eulerAngles.z);
	}
}
