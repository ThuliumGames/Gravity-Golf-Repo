using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour {
	
	public Transform launchSpot;
	public GameObject obj;
	
	void Update () {
		GetComponent<Rigidbody>().velocity = transform.forward*Input.GetAxis("Vertical1")*50;
		transform.Rotate (0, Input.GetAxis("Horizontal1")*Time.deltaTime*50, 0);
		if (Input.GetButtonDown("Fire1")) {
			Instantiate (obj, launchSpot.position, launchSpot.rotation);
		}
	}
}
