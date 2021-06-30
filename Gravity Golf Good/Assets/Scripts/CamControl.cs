using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour {
	
	public static Vector3 camForward;
	public static Vector3 camUp;
	public Transform ObjToFollow;
	float camDist = 10;
	
	public Transform cam;
	
	float ang = 15;
	
	public LayerMask lm;
	
	Gravity G;
	
	void Start () {
		
		if (ObjToFollow.GetComponent<Gravity>()) {
			G = ObjToFollow.GetComponent<Gravity>();
		}
	}
	
	void Update () {
		transform.position = ObjToFollow.position+G.gravVector;
		transform.Rotate ((transform.InverseTransformPoint (ObjToFollow.position).z/Vector3.Distance(ObjToFollow.position, ObjToFollow.position+G.gravVector))*Mathf.PI, 0, (-transform.InverseTransformPoint (ObjToFollow.position).x/Vector3.Distance(ObjToFollow.position, ObjToFollow.position+G.gravVector))*Mathf.PI);
		transform.position += (transform.up*Vector3.Distance(ObjToFollow.position, ObjToFollow.position+G.gravVector));
		cam.position = transform.position;
		cam.localEulerAngles = new Vector3(0, cam.localEulerAngles.y, 0);
		if (!Global.touchingHole) {
			cam.Rotate(0, (Input.GetAxis("Mouse X")+(Input.GetAxis("LStick X")*3)+(Input.GetAxis("RStick X")*3))*Time.deltaTime*60, 0);
			if (!Input.GetButton("Fire1") || Input.GetButton("Fire2")) {
				ang = Mathf.Clamp(ang-(Input.GetAxis("Mouse Y")+(Input.GetAxis("LStick Y")*3)+(Input.GetAxis("RStick Y")*3))*Time.deltaTime*60, -89, 89);
				camDist = Mathf.Clamp(camDist-(Input.GetAxis("Mouse ScrollWheel")+Input.GetAxis("Triggers"))*Time.deltaTime*240, 3, 40);
				if (!Input.GetButton("Fire2") && !ObjToFollow.GetComponent<Balls>().shooting) {
					camForward = cam.forward;
					camUp = cam.up;
				}
			} else {
				camForward = cam.forward;
				camUp = cam.up;
			}
		} else {
			ang = Mathf.Clamp(ang+(5)*Time.deltaTime*60, -89, 89);
			camDist = Mathf.Clamp(camDist+(0.125f)*Time.deltaTime*240, 3, 40);
		}
		cam.Rotate (ang, 0, 0);
		RaycastHit h;
		Physics.Raycast(transform.position, -cam.forward, out h, camDist, lm);
		if (h.point != Vector3.zero) {
			cam.position = h.point;
			cam.position += (cam.forward*1f);
		} else {
			cam.position += (cam.forward*-camDist);
		}
	}
}