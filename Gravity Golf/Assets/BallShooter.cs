using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallShooter : MonoBehaviour {
	
	public GameObject GO;
	public GameObject GO2;
	
	void Update () {
		RaycastHit H;
		Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out H);
		transform.LookAt(H.point);
		
		if (Input.GetButtonDown("Fire1")) {
			GameObject G = Instantiate (GO, transform.position+(transform.forward*4f), Quaternion.identity);
			Instantiate (GO2, transform.position+(transform.forward*4f), transform.rotation);
			G.transform.forward = transform.forward;
		}
	}
}
