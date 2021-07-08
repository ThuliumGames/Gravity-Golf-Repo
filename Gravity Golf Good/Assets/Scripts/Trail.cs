using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour {
	
	public Balls ball;
	
	Rigidbody rb;
	Gravity gt;
	LineRenderer lr;
	
	public Trail follow;
	
	public PhysicMaterial[] PH;
	
	void Start () {
		rb = GetComponent<Rigidbody>();
		gt = GetComponent<Gravity>();
		//lr = GetComponentInChildren<LineRenderer>();
		
		Invoke("Dest", 2);
	}
	
	void Update () {
		if (!Input.GetButton("Fire1") && !ball.shooting) {
			Dest ();
		}
		
		/*if (follow != null) {
			lr.SetPosition(1, transform.InverseTransformPoint(follow.transform.position));
			if (Vector3.Distance (lr.GetPosition(0), lr.GetPosition(1)) > 4) {
				Dest ();
			}
		}*/
	}
	
	void FixedUpdate () {
		rb.AddForce(gt.gravVector*Physics.gravity.magnitude*gt.gravMulti);
	}
	
	public void Dest () {
		/*if (follow != null) {
			follow.Dest();
		}*/
		Destroy(gameObject);
	}
	
	void OnTriggerEnter (Collider other) {
		if (other.gameObject.layer == LayerMask.NameToLayer("Hole")) {
			gameObject.layer = LayerMask.NameToLayer("HoleBall");
		}
	}
	
	void OnTriggerExit (Collider other) {
		if (other.gameObject.layer == LayerMask.NameToLayer("Hole")) {
			gameObject.layer = LayerMask.NameToLayer("TrailBall");
		}
	}
}
