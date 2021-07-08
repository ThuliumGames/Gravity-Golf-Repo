using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FolowCam : MonoBehaviour {
	
	public int mode;
	
	void Update () {
		switch (mode) {
			case 0:
				transform.LookAt(Camera.main.transform.position);
				if (SceneManager.GetActiveScene().name == "Title Screen") {
					transform.localScale = transform.parent.parent.parent.localScale;
					transform.GetChild(0).localScale = transform.parent.parent.parent.localScale;
				}
				break;
			case 1:
				if (GameObject.Find("Hole")) {
					Vector2 v2 = Camera.main.WorldToScreenPoint(GameObject.Find("Hole").transform.position+GameObject.Find("Hole").transform.up);
					Vector2 v2up = Camera.main.WorldToScreenPoint(GameObject.Find("Hole").transform.position+(GameObject.Find("Hole").transform.up*2));
					
					GetComponent<RectTransform>().eulerAngles = new Vector3 (0, 0, Mathf.Atan2(v2.x-v2up.x, v2up.y-v2.y)*Mathf.Rad2Deg);
					
					if (Vector3.Dot(Camera.main.transform.forward, GameObject.Find("Hole").transform.position-Camera.main.transform.position) > 0) {
						GetComponent<RectTransform>().position = new Vector2 (Mathf.Clamp(v2.x, 0, Screen.width), Mathf.Clamp(v2.y, 0, Screen.height));
					}
					
					Physics.clothGravity = (GameObject.Find("Hole").transform.forward*UnityEngine.Random.Range(-10.0f, 0.0f))+(GameObject.Find("Hole").transform.up*UnityEngine.Random.Range(-3.0f, 3.0f));
				}
				break;
			case 2:
				transform.position = Camera.main.transform.position;
				break;
			default:
				break;
		}
	}
}
