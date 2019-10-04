using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Planet : MonoBehaviour {
	
	private Rigidbody ObjToPull;

	public bool DisableIfClose;
	
	float Origpf;
	
	public bool isDirectional;

	public Vector3 Direction;

	public float Range;
	public float Rangez;
	public float Rangey;
	
	public bool AddRandomDir;

	public float pullForce;
	
	public static bool isSelec;
	
	public List<Rigidbody> RigLis = new List<Rigidbody>();
	public List<Rigidbody> RigLisPrev = new List<Rigidbody>();

	void Start () {
		
		Origpf = pullForce;
		
	}
	
	private void Update() {
		
		if (Application.isPlaying) {
			
			Rigidbody[] array = Object.FindObjectsOfType<Rigidbody>();
			
			foreach (Rigidbody rigidbody in array) {
				
				if (rigidbody == GetComponent<Rigidbody>()) {
					continue;
				}
				
				bool Wrong = false;
				
				if (GameObject.Find("Cannon") != null) {
					Wrong = true;
				}
				if (Wrong) {
					if (rigidbody == GameObject.Find("Cannon").GetComponent<Rigidbody>()) {
						continue;
					}
				}
				
				ObjToPull = rigidbody;
				
				float num = Vector3.Distance(ObjToPull.gameObject.transform.position, base.transform.position);
				float range = Range;
				
				Vector3 localScale = base.transform.localScale;
				
				if (((num < range + (localScale.x * 10)) && !isDirectional) || (isDirectional && Mathf.Abs(transform.InverseTransformPoint(ObjToPull.transform.position).x) < Range && Mathf.Abs(transform.InverseTransformPoint(ObjToPull.transform.position).z) < Rangez && Mathf.Abs(transform.InverseTransformPoint(ObjToPull.transform.position).y) < Rangey)) {
					
					
					RigLis.Add (rigidbody);
							
					bool slow = true;
					
					foreach (Rigidbody Rb in RigLisPrev) {
						if (Rb == rigidbody || rigidbody.name == "GBC(Clone)") {
							slow = false;
						}
					}
					
					if (slow && tag != "BlackHole") {
						if (rigidbody.GetComponent<GolfHit>()) {
							rigidbody.GetComponent<GolfHit>().Slow();
						}
					}
							
					GameObject gameObject2 = new GameObject();
					
					if (isDirectional) {
						
						gameObject2.transform.position = ObjToPull.gameObject.transform.position + Direction;
						
					} else {
						
						gameObject2.transform.position = base.transform.position;
						
					}
					
					gameObject2.transform.LookAt(ObjToPull.gameObject.transform.position);
					
					if (AddRandomDir) {
						
						Vector3 ITV = gameObject2.transform.InverseTransformPoint(ObjToPull.gameObject.transform.position + ObjToPull.velocity)/2;
						gameObject2.transform.LookAt(ObjToPull.gameObject.transform.position + (gameObject2.transform.right*ITV.x) + (gameObject2.transform.up*ITV.y));
					}
					
					if (ObjToPull.gameObject.layer == LayerMask.NameToLayer("Boss")) {
						
						ObjToPull.AddForce(-gameObject2.transform.forward * (pullForce * 1E+07f) * Time.deltaTime);
					
					} else {
						ObjToPull.AddForce(-gameObject2.transform.forward * (pullForce) * Time.deltaTime);
						
						if (DisableIfClose) {
							
							pullForce = 0;
							
						}
					}
					
					Object.Destroy(gameObject2);
				} else {
					float num4 = Vector3.Distance(ObjToPull.gameObject.transform.position, base.transform.position);
					float num5 = Range;
					Vector3 localScale3 = base.transform.localScale;
					
					if (num4 < ((num5 + (localScale3.x * 10)) * 2) && !isDirectional) {
						
						GameObject gameObject = new GameObject();
						gameObject.transform.position = base.transform.position;
						gameObject.transform.LookAt(ObjToPull.gameObject.transform.position);
					
						if (AddRandomDir) {
							
							Vector3 ITV = gameObject.transform.InverseTransformPoint(ObjToPull.gameObject.transform.position + ObjToPull.velocity)/2;
							gameObject.transform.LookAt(ObjToPull.gameObject.transform.position + (gameObject.transform.right*ITV.x) + (gameObject.transform.up*ITV.y));
						}
						ObjToPull.AddForce(-gameObject.transform.forward * (pullForce / 2f) * Time.deltaTime);
						Object.Destroy(gameObject);
						
					} else {
						
						if (DisableIfClose) {
							
							pullForce = Origpf;
							
						}
					}
				}
			}
			
		} else {
			
			Transform transform = base.transform;
			Vector3 localScale4 = base.transform.localScale;
			float x = localScale4.x;
			Vector3 localScale5 = base.transform.localScale;
			float x2 = localScale5.x;
			Vector3 localScale6 = base.transform.localScale;
			transform.localScale = new Vector3(x, x2, localScale6.x);
			
		}
		
		RigLisPrev.Clear();
		foreach (Rigidbody Rb in RigLis) {
			RigLisPrev.Add (Rb);
		}
		RigLis.Clear();
	}
	
	private void OnDrawGizmosSelected() {
		
		if (!isDirectional) {
			Gizmos.color = Color.yellow;
			Vector3 position = base.transform.position;
			float range = Range;
			Vector3 localScale = base.transform.localScale;
			Gizmos.DrawWireSphere(position, range + (localScale.x * 10));
			Gizmos.color = new Color(0.5f, 0.5f, 0f);
			Vector3 position2 = base.transform.position;
			Vector3 localScale2 = base.transform.localScale;
			Gizmos.DrawWireSphere(position2, (range + (localScale2.x * 10)) * 2);
		} else {
			Gizmos.color = Color.yellow;
			Vector3 position = base.transform.position;
			Vector3 localScale = base.transform.localScale;
			Gizmos.DrawWireCube(position, new Vector3(Range* 2, Rangey * 2, Rangez * 2) * transform.localScale.x);
		}
	}
	
	void OnTriggerEnter (Collider Hit) {
		
		if (Hit.GetComponent<GolfHit>()) {
			
			Hit.GetComponent<GolfHit>().inWater = true;
			
		}
	}
	
	void OnTriggerExit (Collider Hit) {
		
		if (Hit.GetComponent<GolfHit>()) {
			
			Hit.GetComponent<GolfHit>().inWater = false;
			
		}
	}
}
