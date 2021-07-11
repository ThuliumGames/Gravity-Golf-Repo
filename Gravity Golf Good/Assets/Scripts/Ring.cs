using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour {
	
	bool hasEntered = false;
	public Material newMat;
	
	void OnTriggerEnter (Collider other) {
		if (!hasEntered) {
			if (other.gameObject.tag == "Player") {
				hasEntered = true;
				Camera.main.GetComponent<Animator>().Play("Points");
				Global.score+=5;
				GetComponent<Renderer>().material = newMat;
				GetComponent<AudioSource>().Play();
			}
		}
	}
}
