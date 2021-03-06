﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Balls : MonoBehaviour {
	
	Rigidbody rb;
	Gravity gt;
	public Transform vis;
	
	float rot;
	
	float shotForce;
	float shotAng;
	
	public float maxShotForce;
	
	public GameObject shotTrail;
	Transform prevTrail;
	
	public Image shotGauge;
	public Transform shotAngler;
	public GameObject[] ControlWords;
	bool canShoot = true;
	
	public AudioSource hitBall;
	public AudioSource shotPower;
	
	public GameObject GrassObj;
	
	Vector3 shotPos;
	
	bool onGrass;
	bool onGrassThisFrame;
	public PhysicMaterial[] PH;
	
	bool canDo;
	
	void Start () {
		rb = GetComponent<Rigidbody>();
		gt = GetComponent<Gravity>();
	}
	
	void Update () {
		
		if ((!Global.touchingHole || rb.velocity.magnitude > 0.1f) && !Global.Paused) {
			if ((transform.position-shotPos).magnitude > 1) {
				if (Input.GetButtonDown("M")) {
					Mulligan();
				}
			}
			ControlWords[0].SetActive(true);
			ControlWords[2].SetActive(true);
			ControlWords[1].SetActive(false);
			ControlWords[3].SetActive(false);
			shotPower.volume = 0;
			if (((!Input.GetButton("Fire1") && !Global.shotLocked) || (Input.GetButtonUp("Fire1") && Global.shotLocked && canDo)) && shotForce > 0) {
				canDo = false;
				shotPos = transform.position;
				rb.AddForce(((CamControl.camForward*Mathf.Cos(shotAng))+(CamControl.camUp*Mathf.Sin(shotAng)))*shotForce);
				shotForce = 0;
				shotAng = 0;
				Global.strokes++;
				vis.localScale = Vector3.one*2;
				hitBall.Play();
			} else if (shotForce > 0) {
				if (Input.GetButtonDown("Fire2")) {
					Global.shotLocked = !Global.shotLocked;
				}
				
				if (Global.shotLocked) {
					if (Input.GetButtonDown("Cam")) {
						Global.freeCam = true;
					}
					if (Input.GetButtonUp("Fire1")) {
						canDo = true;
					}
				} else {
					canDo = false;
				}
				
				shotPower.volume = 0.025f;
				shotPower.pitch = shotForce/maxShotForce;
				ControlWords[1].SetActive(true);
				ControlWords[3].SetActive(true);
				ControlWords[0].SetActive(false);
				ControlWords[2].SetActive(false);
				rb.velocity = Vector3.zero;
				shotGauge.fillAmount = ((shotForce/maxShotForce)/(1.0f/0.22f))+0.39f;
				shotAngler.localPosition = new Vector3 (-45, ((shotAng/(Mathf.PI/2))*102)-51, 0);
				if (Time.frameCount%2==0) {
					Gravity G = Instantiate(shotTrail, transform.position, transform.rotation).GetComponent<Gravity>();
					if (Global.difficulty == 1) {
						G.GetComponent<Trail>().ball = this;
						G.currentGrav = gt.currentGrav;
						G.gravVector = gt.gravVector;
						G.onLinePlanet = gt.onLinePlanet;
						G.linkNum = gt.linkNum;
						/*if (prevTrail != null) {
							G.GetComponent<Trail>().follow = prevTrail.GetComponent<Trail>();
						}*/
						//prevTrail = G.transform;
						G.GetComponent<Rigidbody>().AddForce(((CamControl.camForward*Mathf.Cos(shotAng))+(CamControl.camUp*Mathf.Sin(shotAng)))*shotForce);
					} else {
						Transform o = G.transform;
						Destroy(G);
						o.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
						o.position = transform.position+(((CamControl.camForward*Mathf.Cos(shotAng))+(CamControl.camUp*Mathf.Sin(shotAng)))*(shotForce/2500.0f));
					}
				}
			} else {
				if (Input.GetButtonDown("Cam")) {
					Global.freeCam = true;
				}
				canDo = false;
				Global.shotLocked = false;
				if (Input.GetButton("Fire1")) {
					ControlWords[1].SetActive(true);
					ControlWords[3].SetActive(true);
					ControlWords[0].SetActive(false);
					ControlWords[2].SetActive(false);
				}
				shotGauge.fillAmount = Mathf.Lerp(shotGauge.fillAmount, 0.39f, 5*Time.deltaTime);
				shotAngler.localPosition = new Vector3 (-45, Mathf.Lerp(shotAngler.localPosition.y, -51, 10*Time.deltaTime), 0);
				shotAng = 0;
			}
		}
	}
	
	void FixedUpdate () {
		if (!Global.touchingHole || rb.velocity.magnitude > 0.1f) {
			vis.localScale = Vector3.Lerp(vis.localScale, Vector3.one, 0.5f);
			rb.AddForce(gt.gravVector*(Physics.gravity.magnitude*gt.gravMulti));
			
			if (rb.velocity.magnitude > 0.1f) {
				vis.LookAt(transform.position+rb.velocity);
				rot += rb.velocity.magnitude;
				vis.Rotate(rot, 0, 0);
				canShoot = false;
			} else {
				if (!canShoot) {
					Global.didAGood = true;
					canShoot = true;
				}
				if (Input.GetButton("Fire1")) {
					if (!Global.shotLocked) {
						shotForce = Mathf.Clamp (shotForce-((Input.GetAxis("Mouse Y")+(Input.GetAxis("Triggers")*20))*20), 0, maxShotForce);
						shotAng = Mathf.Clamp(shotAng-(Input.GetAxis("Mouse ScrollWheel")+(Input.GetAxis("LStick Y")*0.0625f)), 0, Mathf.PI/2);
					}
				}
			}
		}
		
		if (onGrass) {
			GetComponent<Collider>().material = PH[0];
		} else {
			GetComponent<Collider>().material = PH[1];
		}
		
		onGrassThisFrame = false;
		
	}
	
	void OnTriggerEnter (Collider other) {
		if (other.gameObject.layer == LayerMask.NameToLayer("Hole")) {
			gameObject.layer = LayerMask.NameToLayer("HoleBall");
			rb.velocity /= 2;
			Global.touchingHole = true;
		} else if (other.gameObject.layer == LayerMask.NameToLayer("Death")) {
			Mulligan();
		}
	}
	
	void Mulligan () {
		Global.strokes++;
		rb.velocity = Vector3.zero;
		transform.position = shotPos;
	}
	
	void OnTriggerExit (Collider other) {
		if (other.gameObject.layer == LayerMask.NameToLayer("Hole")) {
			gameObject.layer = LayerMask.NameToLayer("Ball");
			Global.touchingHole = false;
		}
	}
	
	void OnCollisionStay (Collision other) {
		RaycastHit H;
		if (other.gameObject.GetComponent<MeshFilter>()) {
			if (Physics.Raycast(other.contacts[0].point+(other.contacts[0].normal*0.01f), -other.contacts[0].normal, out H, Mathf.Infinity)) {
				int theIndex = -1;
				for (int x = 0; x < other.gameObject.GetComponent<MeshFilter>().mesh.subMeshCount; x++) {
					int[] tri = other.gameObject.GetComponent<MeshFilter>().mesh.GetTriangles(x);
					for (int y = 0; y < tri.Length; y++) {
						if (H.triangleIndex == tri[y]) {
							theIndex = x;
							break;
						}
					}
				}
				
				if (theIndex != -1) {
					if (other.gameObject.GetComponent<Renderer>().materials[theIndex].name.Contains("ass")) {
						onGrass = true;
						onGrassThisFrame = true;
						/*GameObject G = Instantiate(GrassObj);
						G.transform.position = other.contacts[0].point;
						G.transform.LookAt(G.transform.position+other.contacts[0].normal);*/
					} else {
						if (!onGrassThisFrame) {
							onGrass = false;
						}
					}
				} else if (other.gameObject.GetComponent<Renderer>().materials.Length == 1) {
					if (other.gameObject.GetComponent<Renderer>().materials[0].name.Contains("ass")) {
						onGrass = true;
						onGrassThisFrame = true;
						/*GameObject G = Instantiate(GrassObj);
						G.transform.position = other.contacts[0].point;
						G.transform.LookAt(G.transform.position+other.contacts[0].normal);*/
					} else {
						if (!onGrassThisFrame) {
							onGrass = false;
						}
					}
				}
			}
		}
	}
}
