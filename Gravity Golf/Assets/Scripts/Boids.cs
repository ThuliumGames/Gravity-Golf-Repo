using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boids : MonoBehaviour {
	
	public LayerMask LM;
	
	public float FrdThreshold = 0.5f;
	public float SphericalThreshold = 1;
	public float VisualThreshold = 5;
	
	public float TurnAwaySpeed;
	public float TurnCenterSpeed;
	public float TurnToSpeed;
	public float TurnTargetSpeed;
	
	public float MoveSpeed;
	
	public Boids[] BoidGroup;
	
	public bool isMoving;
	public Boids MasterBoid;
	
	public Transform Bird;
	public Transform BirdVis;
	public Transform BirdVisGrounded;
	public Transform BirdLook;
	public Transform BT1;
	public Transform BT2;
	
	public float LandingTime;
	float T;
	
	void Start () {
		int i = 0;
		foreach (Boids B in GameObject.FindObjectsOfType<Boids>()) {
			float TmpDst = Vector3.Distance(transform.position, B.transform.position);
			if (TmpDst < 100) {
				if (i > 0) {
					System.Array.Resize(ref BoidGroup, BoidGroup.Length+1);
				}
				BoidGroup[BoidGroup.Length-1] = B;
				i++;
			}
		}
	}
	
	void Update () {
		
		BirdLook.LookAt(transform.parent.position);
		if (Vector3.Distance (Bird.forward, transform.forward) > Vector3.Distance (BT1.forward, transform.forward)) {
			Bird.Rotate (0, 15, 0);
		} else if (Vector3.Distance (Bird.forward, transform.forward) > Vector3.Distance (BT2.forward, transform.forward)) {
			Bird.Rotate (0, -15, 0);
		}
		
		if (isMoving) {
			T += Time.deltaTime;
			if (MasterBoid == this) {
				GetComponent<Rigidbody>().velocity = transform.forward * MoveSpeed;
			} else {
				Vector3 TurnAway = new Vector3 (0, 0, 0);
				Vector3 FlockCenter = new Vector3 (0, 0, 0);
				Vector3 FlockDirection = new Vector3 (0, 0, 0);
				int TurnAm = 0;
				int CenterAm = 0;
				float Dist = Mathf.Infinity;
				Boids ClosB = null;
				
				foreach (Boids B in BoidGroup) {
					if (B != this.GetComponent<Boids>()) {
						
						float TmpDst = Vector3.Distance(transform.position, B.transform.position);
						if (transform.InverseTransformPoint(B.transform.position).z > FrdThreshold && TmpDst < VisualThreshold) {
							if (TmpDst < SphericalThreshold) {
								TurnAway += B.transform.position;
								TurnAm++;
							}
							
							FlockCenter += B.transform.position;
							FlockDirection += B.transform.eulerAngles;
							CenterAm++;
							
							if (TmpDst < Dist) {
								ClosB = B;
								Dist = TmpDst;
							}
						}
					}
				}
				
				if (T < LandingTime) {
					if (TurnAm > 0) {
						TurnAway /= TurnAm;
						
						Quaternion targetRotation = Quaternion.LookRotation(transform.position - TurnAway, Vector3.up);
						transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, TurnAwaySpeed * Time.deltaTime);    
					}
					
					RaycastHit H;
					Vector3 HitPos;
					if (Physics.BoxCast(transform.position, Vector3.one*0.5f, transform.forward, out H, transform.rotation, SphericalThreshold, LM)) {
						HitPos = H.point;
						Quaternion targetRotation2 = Quaternion.LookRotation(transform.position - HitPos, Vector3.up);
						transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation2, TurnAwaySpeed * Time.deltaTime);
					}
					
					if (CenterAm > 0) {
						FlockCenter /= CenterAm;
						FlockDirection /= CenterAm;
						
						Quaternion targetRotation = Quaternion.LookRotation(FlockCenter - transform.position, Vector3.up);
						transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, TurnCenterSpeed * Time.deltaTime);
						
						transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler (FlockDirection), TurnToSpeed * Time.deltaTime);  
					}
				}
				
				Quaternion targetRotation1 = Quaternion.LookRotation(transform.parent.position - transform.position, Vector3.up);
				transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation1, TurnTargetSpeed * Time.deltaTime);  
				
				if (T > LandingTime) {
					GetComponent<Rigidbody>().velocity = transform.forward * (0.5f * MoveSpeed);
				} else {
					GetComponent<Rigidbody>().velocity = transform.forward * MoveSpeed;
				}
			}
		} else {
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			T = 0;
			if (Vector3.Distance(transform.position, GameObject.Find("Golf Ball").transform.position) < 5 && GameObject.Find("Golf Ball").GetComponent<Rigidbody>().velocity.magnitude > 1) {
				isMoving = true;
				transform.LookAt(transform.parent.position);
				transform.Rotate(180, 0, 0);
			}
		}
	}
	
	void OnCollisionEnter () {
		isMoving = false;
	}
	
	void OnCollisionStay () {
		BirdVis.gameObject.SetActive(false);
		BirdVisGrounded.gameObject.SetActive(true);
	}
	
	void OnCollisionExit () {
		isMoving = true;
		BirdVis.gameObject.SetActive(true);
		BirdVisGrounded.gameObject.SetActive(false);
	}
}
