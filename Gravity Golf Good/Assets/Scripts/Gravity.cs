using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour {
	
	public int linkNum = 0;
	
	public bool gravityUser;
	
	public int priority;
	
	public Gravity currentGrav;
	Gravity prevGrav;
	
	public enum GravityType {
		Box,
		Sphere,
		Line,
		Capsule
	};
	public GravityType gravityType;
	
	public GameObject onLinePlanet;
	public LayerMask lm;
	
	public Vector3 gravVector = new Vector3 (0, 0, 0);
	
	[HideInInspector]
	public bool inBH;
	[HideInInspector]
	public bool inWorld;
	[HideInInspector]
	public float waitTime;
	[HideInInspector]
	public bool phase2 = false;
	[HideInInspector]
	public float gravMulti = 1;
	
	public AudioSource bgNoise;
	void Start () {
		prevGrav = currentGrav;
	}
	
	void Update () {
		if (gravityUser) {
			if (!inBH && inWorld) {
				RaycastHit H;
				Physics.gravity = new Vector3 (0, -40, 0);
				if (Physics.Raycast(transform.position, gravVector, out H, Mathf.Infinity, lm)) {
					gravMulti = Mathf.Clamp((50.0f-H.distance)/50.0f, 0.125f, 1.0f);
				} else {
					gravMulti = 0.125f;
				}
				
				if (bgNoise != null) {
					bgNoise.volume = Mathf.Lerp(bgNoise.volume, gravMulti, Time.deltaTime);
				}
				
				if (onLinePlanet != null) {
					RaycastHit hit;
					
					if (Physics.Raycast(transform.position, gravVector, out hit, Mathf.Infinity, lm)) {
						if (hit.collider.gameObject == onLinePlanet && Vector3.Dot(gravVector, -hit.normal) > 0.5f) {
							gravVector = -hit.normal;
						}
					}
				}
			
				if (prevGrav != currentGrav) {
					if (prevGrav != null && currentGrav != null) {
						if (prevGrav.linkNum != currentGrav.linkNum) {
							GetComponent<Rigidbody>().velocity = Vector3.ClampMagnitude(GetComponent<Rigidbody>().velocity, GetComponent<Rigidbody>().velocity.magnitude/1.5f);
						}
					}
					prevGrav = currentGrav;
				}
			} else {
				Physics.gravity = Vector3.zero;
			}
		}
	}
	
	void OnTriggerExit (Collider other) {
		if (!gravityUser) {
			if (other.GetComponent<Gravity>()) {
				Gravity G = other.GetComponent<Gravity>();
				if (G.gravityUser) {
					G.currentGrav = null;
					G.onLinePlanet = null;
				}
			}
		} else {
			if (other.gameObject.name == "World") {
				inWorld = false;
			}
		}
	}
	
	void OnTriggerStay (Collider other) {
		if (!gravityUser) {
			if (other.GetComponent<Gravity>()) {
				Gravity G = other.GetComponent<Gravity>();
				if (G.gravityUser) {
					if (G.currentGrav == this) {
						switch (gravityType) {
							
							case GravityType.Box:
								
								G.gravVector = -transform.up;
								
								break;
							case GravityType.Sphere:
								
								G.gravVector = (transform.position-other.transform.position).normalized;
								
								break;
							case GravityType.Line:
									
									if (G.onLinePlanet == null || linkNum != G.linkNum) {
										G.gravVector = -transform.up;
									}
									G.onLinePlanet = onLinePlanet;
								
								break;
							case GravityType.Capsule:
									
									float d = Vector3.Distance(transform.position, onLinePlanet.transform.position);
									
									onLinePlanet.transform.LookAt(transform.position);
									
									float lerpAmount = onLinePlanet.transform.InverseTransformPoint(other.transform.position).z/d;
									
									Vector3 result = Vector3.Lerp(onLinePlanet.transform.position, transform.position, lerpAmount);
									
									G.gravVector = (result-other.transform.position).normalized;
								
								break;
							default:
								break;
						}
						
						G.linkNum = linkNum;
					} else {
						if (G.currentGrav == null || priority > G.currentGrav.priority) {
							G.currentGrav = this;
						}
					}
				}
			}
		} else {
			if (other.gameObject.name == "World") {
				inWorld = true;
			}
		}
	}
}
