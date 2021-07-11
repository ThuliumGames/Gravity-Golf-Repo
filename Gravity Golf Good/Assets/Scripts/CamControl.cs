using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour {
	
	public static Vector3 camForward;
	public static Vector3 camUp;
	public Transform ObjToFollow;
	float camDist = 10;
	float preCamDist = 10;
	
	public Transform cam;
	
	float ang = 15;
	float preAng = 15;
	
	public LayerMask lm;
	
	Gravity G;
	
	public Transform freeCamCam;
	
	Vector3 preLoc;
	Vector3 preRot;
	
	void Start () {
		
		if (ObjToFollow.GetComponent<Gravity>()) {
			G = ObjToFollow.GetComponent<Gravity>();
		}
	}
	
	void Update () {
		
		preLoc = cam.position;
		preRot = cam.eulerAngles;
		preCamDist = camDist;
		preAng = ang;
		
		transform.position = ObjToFollow.position+G.gravVector;
		transform.Rotate ((transform.InverseTransformPoint (ObjToFollow.position).z/Vector3.Distance(ObjToFollow.position, ObjToFollow.position+G.gravVector))*Mathf.PI, 0, (-transform.InverseTransformPoint (ObjToFollow.position).x/Vector3.Distance(ObjToFollow.position, ObjToFollow.position+G.gravVector))*Mathf.PI);
		//transform.position += (transform.up*Vector3.Distance(ObjToFollow.position, ObjToFollow.position+G.gravVector));
		transform.position = ObjToFollow.position;
		cam.position = transform.position;
		cam.localEulerAngles = new Vector3(0, cam.localEulerAngles.y, 0);
		
		if (!Global.freeCam) {
			if (!Global.touchingHole) {
				cam.Rotate(0, (Input.GetAxis("Mouse X")+(Input.GetAxis("LStick X")*3))*Time.deltaTime*60, 0);
				if (!Input.GetButton("Fire1") || Global.shotLocked) {
					ang = Mathf.Clamp(ang-(Input.GetAxis("Mouse Y")+(Input.GetAxis("LStick Y")*3))*Time.deltaTime*60, -89, 89);
					camDist = Mathf.Clamp(camDist-(Input.GetAxis("Mouse ScrollWheel")+Input.GetAxis("Triggers"))*Time.deltaTime*240, 3, 40);
					if (!Global.shotLocked) {
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
			
			cam.position += (cam.forward*-camDist);
			
			RaycastHit h;
			Physics.Raycast(preLoc, (cam.position-preLoc).normalized, out h, (cam.position-preLoc).magnitude, lm);
			if (h.collider != null) {
				Physics.Raycast(transform.position, -cam.forward, out h, camDist, lm);
				if (h.point != Vector3.zero) {
					cam.position = h.point;
					cam.position += (cam.forward*1f);
				} else {
					cam.position += (cam.forward*-camDist);
				}
			}
			
			if (freeCamCam.GetComponent<Camera>().depth > -2) {
				freeCamCam.GetComponent<Camera>().depth = Mathf.Lerp(freeCamCam.GetComponent<Camera>().depth, -3, Time.deltaTime*5);
				freeCamCam.position = Vector3.Lerp(freeCamCam.position, cam.position, Time.deltaTime*5);
				//freeCamCam.eulerAngles = Vector3.Lerp(freeCamCam.eulerAngles, cam.eulerAngles, Time.deltaTime*5);
			} else {
				freeCamCam.GetComponent<Camera>().depth = -2;
				freeCamCam.position = cam.position;
				freeCamCam.eulerAngles = cam.eulerAngles;
			}
			
		} else {
			if (Input.GetButton("Fire1")) {
				Global.freeCam = false;
			}
			freeCamCam.GetComponent<Camera>().depth = 1;
			freeCamCam.Translate((Input.GetAxis("Controller LX")*2)-(Input.GetAxis("Mouse X")*System.Convert.ToInt32(Input.GetButton("MiddleClick"))), -Input.GetAxis("Mouse Y")*System.Convert.ToInt32(Input.GetButton("MiddleClick")), (Input.GetAxis("Controller LY")*2)+(Input.GetAxis("Mouse ScrollWheel")*20));
			freeCamCam.Rotate((Input.GetAxis("Controller RY")*7)-(Input.GetAxis("Mouse Y")*System.Convert.ToInt32(Input.GetButton("RightClick"))*4.5f), (Input.GetAxis("Controller RX")*7)+(Input.GetAxis("Mouse X")*System.Convert.ToInt32(Input.GetButton("RightClick"))*System.Convert.ToInt32(!Input.GetButton("Controller ZMod"))*4.5f)+(Input.GetAxis("Controller RotZKey")*System.Convert.ToInt32(!Input.GetButton("Controller ZMod"))*7), -(Input.GetAxis("Controller RotZ")*7)-(Input.GetAxis("Controller RotZKey")*System.Convert.ToInt32(Input.GetButton("Controller ZMod"))*7)-(Input.GetAxis("Mouse X")*System.Convert.ToInt32(Input.GetButton("RightClick"))*System.Convert.ToInt32(Input.GetButton("Controller ZMod"))*4.5f));
		}
	}
}