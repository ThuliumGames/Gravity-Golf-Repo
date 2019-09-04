using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedCam : MonoBehaviour {
	
	Vector3 Pos;
	Vector3 Rot;
	
	void Start () {
		Pos = transform.position;
		Rot = transform.eulerAngles;
	}
	
	void Update () {
		if (Input.GetButton("Fire9")) {
			GetComponent<Camera>().depth = 10;
			transform.position = Vector3.Lerp (transform.position, Pos, 10*Time.deltaTime);
		} else {
			transform.position = Vector3.Lerp (transform.position, Camera.main.transform.position, 10*Time.deltaTime);
			if (Vector3.Distance (transform.position, Camera.main.transform.position) < 0.1f) {
				GetComponent<Camera>().depth = -10;
			}
		}
	}
}
