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
				
				if (!(rigidbody != GetComponent<Rigidbody>())) {
					continue;
				}
				
				ObjToPull = rigidbody;
				
				float num = Vector3.Distance(ObjToPull.gameObject.transform.position, base.transform.position);
				float range = Range;
				
				Vector3 localScale = base.transform.localScale;
				
				if (!(num < range + localScale.x / 2f)) {
				
					float num4 = Vector3.Distance(ObjToPull.gameObject.transform.position, base.transform.position);
					float num5 = Range * 4f;
					Vector3 localScale3 = base.transform.localScale;
					
					if (num4 < num5 + localScale3.x / 2f && !isDirectional) {
						
						GameObject gameObject = new GameObject();
						gameObject.transform.position = base.transform.position;
						gameObject.transform.LookAt(ObjToPull.gameObject.transform.position);
						ObjToPull.AddForce(-gameObject.transform.forward * (pullForce / 2f) * Time.deltaTime);
						Object.Destroy(gameObject);
						
					} else {
						
						if (DisableIfClose) {
							
							pullForce = Origpf;
							
						}
					}
					
					continue;
				}
				
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
				
				if (ObjToPull.gameObject.layer == LayerMask.NameToLayer("Boss")) {
					
					ObjToPull.AddForce(-gameObject2.transform.forward * (pullForce * 1E+07f) * Time.deltaTime);
				
				} else {
					
					ObjToPull.AddForce(-gameObject2.transform.forward * pullForce * Time.deltaTime);
					
					if (DisableIfClose) {
						
						pullForce = 0;
						
					}
				}
				
				Object.Destroy(gameObject2);
				
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

	private void OnDrawGizmos() {
		
		if (isSelec) {
			
			Gizmos.color = Color.yellow;
			Vector3 position = base.transform.position;
			float range = Range;
			Vector3 localScale = base.transform.localScale;
			Gizmos.DrawWireSphere(position, range + localScale.x / 2f);
			Gizmos.color = new Color(0.5f, 0.5f, 0f);
			Vector3 position2 = base.transform.position;
			float num = Range * 4f;
			Vector3 localScale2 = base.transform.localScale;
			Gizmos.DrawWireSphere(position2, num + localScale2.x / 2f);
			
		}
	}
	
	private void OnDrawGizmosSelected() {
		
		if (!isSelec) {	
		
			Gizmos.color = Color.yellow;
			Vector3 position = base.transform.position;
			float range = Range;
			Vector3 localScale = base.transform.localScale;
			Gizmos.DrawWireSphere(position, range + localScale.x / 2f);
			Gizmos.color = new Color(0.5f, 0.5f, 0f);
			Vector3 position2 = base.transform.position;
			float num = Range * 4f;
			Vector3 localScale2 = base.transform.localScale;
			Gizmos.DrawWireSphere(position2, num + localScale2.x / 2f);
			
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
