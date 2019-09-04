using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBossAI : MonoBehaviour {
	
	public bool Head;
	public GameObject OTI;
	public GameObject OOTI;
	public GameObject Ex;
	public Transform Go;
	public Vector3 LocalOffset;
	public float Speed = 1;
	public static int PlayerHealth = 10;
	
	public bool doAnger;
	public bool doFakeAnger;
	
	Rigidbody RB;
	SnakeBossAI TheHead;
	
	float T;
	Color C;
	public int Life = 5;
	
	public Transform Win;
	
	public bool isMini;
	
	void Start () {
		
		PlayerHealth = 10;
		
		if (name == "Head") {
			Go = GameObject.Find("Golf Ball").transform;
		}
		RB = GetComponent<Rigidbody>();
		foreach (SnakeBossAI SBAI in GameObject.FindObjectsOfType<SnakeBossAI>()) {
			if (SBAI.Head) {
				TheHead = SBAI;
			}
		}
		C = GetComponent<Renderer>().materials[0].GetColor ("_BaseColor");
	}
	
	void Update () {
		if (Life <= 0) {
			Win.transform.position = transform.position + (transform.forward*0.5f*17.5f);
			if (Time.frameCount%15 == 0) {
				foreach (SnakeBossAI SBAI in GameObject.FindObjectsOfType<SnakeBossAI>()) {
					if (SBAI != this) {
						Instantiate(Ex, SBAI.transform.position, transform.rotation);
						Destroy (SBAI.gameObject);
						break;
					}
				}
			}
			transform.LookAt (Go.position+(((Go.right*LocalOffset.x)/Go.localScale.x) + ((Go.up*LocalOffset.y)/Go.localScale.x) + ((Go.forward*LocalOffset.z)/Go.localScale.x)), GameObject.FindObjectOfType<CameraControl>().transform.up);
			RB.velocity = transform.forward * Speed * Time.deltaTime;
		} else {
			if (isMini && name == "Head") {
				if (Time.frameCount%60 == 0) {
					GameObject G1 = Instantiate(OTI, transform.position + transform.forward*10, transform.rotation);
					G1.GetComponent<Rigidbody>().velocity = new Vector3 (0, 0, 2000);
				}
			}
			
			if (GameObject.FindObjectOfType<CameraControl>()) {
				transform.LookAt (Go.position+(((Go.right*LocalOffset.x)/Go.localScale.x) + ((Go.up*LocalOffset.y)/Go.localScale.x) + ((Go.forward*LocalOffset.z)/Go.localScale.x)), GameObject.FindObjectOfType<CameraControl>().transform.up);
			} else {
				transform.LookAt (Go.position+(((Go.right*LocalOffset.x)/Go.localScale.x) + ((Go.up*LocalOffset.y)/Go.localScale.x) + ((Go.forward*LocalOffset.z)/Go.localScale.x)));
			}
			if (doAnger) {
				if (T == 0) {
					GetComponentInChildren<AudioSource>().Play();
				}
				RB.velocity = Vector3.zero;
				T += Time.deltaTime;
				GetComponent<Renderer>().materials[0].SetColor ("_BaseColor", Color.red);
				if (doFakeAnger) {
					if (T > 5) {
						GameObject G1 = Instantiate(OTI, transform.position + transform.forward*10, transform.rotation);
						G1.GetComponent<Rigidbody>().velocity = new Vector3 (0, 0, 1000);
						doAnger = false;
					}
				} else {
					if (T < 10) {
						if (Time.frameCount%30 == 0) {
							GameObject G1 = Instantiate(OTI, transform.position + transform.forward*10, transform.rotation);
							G1.GetComponent<Rigidbody>().velocity = new Vector3 (0, 0, 2000);
						}
					} else {
						doAnger = false;
						if (Life <= 2) {
							GameObject G1 = Instantiate(OOTI, transform.position + transform.right*10, transform.rotation);
							GameObject G2 = Instantiate(OOTI, transform.position + transform.right*-10, transform.rotation);
						}
					}
				}
			} else {
				T = 0;
				GetComponent<Renderer>().materials[0].SetColor ("_BaseColor", C);
				if (TheHead == this) {
					TheHead.doAnger = false;
					TheHead.doFakeAnger = false;
				}
				if (!Head) {
					RB.velocity = transform.forward * Speed * (Vector3.Distance (transform.position, Go.position+(((Go.right*LocalOffset.x)/Go.localScale.x) + ((Go.up*LocalOffset.y)/Go.localScale.x) + ((Go.forward*LocalOffset.z)/Go.localScale.x))) / 10) * Time.deltaTime;
				} else {
					RB.velocity = transform.forward * Speed * Time.deltaTime;
				}
				if (TheHead == this) {
					if (Time.frameCount%60 == 0) {
						int R = Random.Range(0, 5);
						if (R == 1) {
							TheHead.doAnger = true;
							TheHead.doFakeAnger = true;
						}
					}
				}
			}
		}
	}
	
	public void Hit () {
		if (isMini) {
			Destroy(transform.parent.gameObject);
		} else {
			TheHead.GetComponentInChildren<AudioSource>().Play();
			TheHead.Life--;
			Invoke ("DoAng", 2);
		}
	}
	
	void DoAng() {
		TheHead.doAnger = true;
		TheHead.doFakeAnger = false;
	}
}
