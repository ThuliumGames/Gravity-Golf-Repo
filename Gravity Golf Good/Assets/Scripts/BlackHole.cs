using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour {

	Vector3 vel;
	Vector3 dir;
	
	void OnTriggerStay (Collider other) {
		if (other.GetComponent<Gravity>()) {
			Gravity G = other.GetComponent<Gravity>();
			if (G.waitTime < (1-(Vector3.Distance(transform.position, G.transform.position)/100.0f))*180) {
				if (G.gravityUser) {
					if (!G.inBH) {
						dir = G.GetComponent<Rigidbody>().velocity.normalized;
					}
					G.inBH = true;
					transform.LookAt (G.transform.position);
					if (!G.phase2) {
						G.transform.LookAt(transform.position);
						G.GetComponent<Rigidbody>().AddForce((G.transform.forward+dir).normalized*30);
						if (Mathf.Abs(Vector3.Dot(G.GetComponent<Rigidbody>().velocity.normalized, transform.forward.normalized)) < 0.1f) {
							vel = G.GetComponent<Rigidbody>().velocity;
							G.transform.LookAt(G.GetComponent<Rigidbody>().velocity+G.transform.position);
							G.phase2 = true;
						}
					} else {
						transform.RotateAround (transform.position, G.transform.forward, -90);
						G.GetComponent<Rigidbody>().velocity = Vector3.zero;
						G.transform.RotateAround (transform.position, transform.forward, (vel.magnitude*2)/(Vector3.Distance(transform.position, G.transform.position)*2));
						G.waitTime += (vel.magnitude*2)/Vector3.Distance(transform.position, G.transform.position);
					}
				}
			} else {
				G.GetComponent<Rigidbody>().velocity = G.transform.forward*vel.magnitude;
			}
		}
	}
	
	void OnTriggerExit (Collider other) {
		if (other.GetComponent<Gravity>()) {
			Gravity G = other.GetComponent<Gravity>();
			if (G.gravityUser) {
				G.inBH = false;
				G.phase2 = false;
				G.waitTime = 0;
			}
		}
	}
}