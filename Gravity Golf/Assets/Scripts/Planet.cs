using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

[ExecuteInEditMode]
public class Planet : MonoBehaviour {
	
	private Rigidbody ObjToPull;

	public bool DisableIfClose;
	
	float Origpf;
	
	public enum Modes {Spherical, Directional, Mesh};
	public Modes Mode;

	public Vector3 Direction;

	public float Range;
	public float Rangez;
	public float Rangey;
	
	public bool AddRandomDir;

	public float pullForce;
	
	public bool isSelec;
	
	public List<Rigidbody> RigLis = new List<Rigidbody>();
	public List<Rigidbody> RigLisPrev = new List<Rigidbody>();
	
	public List<Rigidbody> AllObjs = new List<Rigidbody>();

	void Start () {
		
		Origpf = pullForce;
		isSelec = true;
	}
	
	private void Update() {
		
		if (Application.isPlaying) {
			
			RaycastHit[] cast = Physics.SphereCastAll(transform.position, ((Range + (transform.localScale.x * 10)) * 2), transform.forward, 0);
			AllObjs.Clear();
			foreach (RaycastHit raycastHit in cast) {
				AllObjs.Add (raycastHit.collider.GetComponent<Rigidbody>());
			}
			
			for (int i = 0; i < AllObjs.Count; i++) {
				if (AllObjs[i] != null) {
					ObjToPull = AllObjs[i];
					float num = Vector3.Distance(ObjToPull.gameObject.transform.position, base.transform.position);
					float range = Range;
					
					Vector3 localScale = base.transform.localScale;
					
					if (((num < range + (localScale.x * 10)) && Mode != Modes.Directional) || (Mode == Modes.Directional && Mathf.Abs(transform.InverseTransformPoint(ObjToPull.transform.position).x) < Range && Mathf.Abs(transform.InverseTransformPoint(ObjToPull.transform.position).z) < Rangez && Mathf.Abs(transform.InverseTransformPoint(ObjToPull.transform.position).y) < Rangey)) {
						
						
						RigLis.Add (ObjToPull);
								
						bool slow = true;
						
						foreach (Rigidbody Rb in RigLisPrev) {
							if (Rb == ObjToPull ||ObjToPull.name == "GBC(Clone)") {
								slow = false;
							}
						}
						
						if (slow && tag != "BlackHole" && tag != "WhiteHole") {
							if (ObjToPull.GetComponent<GolfHit>()) {
								ObjToPull.GetComponent<GolfHit>().Slow();
							}
						}
								
						GameObject gameObject2 = new GameObject();
						
						switch (Mode) {
							case Modes.Spherical:
								gameObject2.transform.position = base.transform.position;
								break;
							case Modes.Directional:
								gameObject2.transform.position = ObjToPull.gameObject.transform.position + Direction;
								break;
							case Modes.Mesh:
								gameObject2.transform.position = ObjToPull.gameObject.transform.position;
								gameObject2.transform.LookAt(base.transform.position);
								
								RaycastHit Normals;
								
								if (Physics.Raycast(gameObject2.transform.position, gameObject2.transform.forward, out Normals, (Range + (transform.localScale.x * 10)) * 2)) {
									gameObject2.transform.LookAt (Normals.normal);
								} else {
									gameObject2.transform.position = base.transform.position;
								}
								break;
							 default:
								gameObject2.transform.position = base.transform.position;
								break;
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
						
						if (num4 < ((num5 + (localScale3.x * 10)) * 2) && Mode != Modes.Directional) {
							
							GameObject gameObject = new GameObject();
						
							switch (Mode) {
								case Modes.Spherical:
									gameObject.transform.position = base.transform.position;
									break;
								case Modes.Directional:
									gameObject.transform.position = ObjToPull.gameObject.transform.position + Direction;
									break;
								case Modes.Mesh:
									gameObject.transform.position = ObjToPull.gameObject.transform.position;
									gameObject.transform.LookAt(base.transform.position);
									
									RaycastHit Normals;
									
									if (Physics.Raycast(gameObject.transform.position, gameObject.transform.forward, out Normals, (Range + (transform.localScale.x * 10)) * 2)) {
										gameObject.transform.LookAt (Normals.point);
										gameObject.transform.Translate (0, 0, 1);
									}
									
									gameObject.transform.position = base.transform.position;
									break;
								 default:
									gameObject.transform.position = base.transform.position;
									break;
							}
						
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
		
		if (Mode != Modes.Directional) {
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
		
		if (Hit.GetComponent<GolfHit>() && gameObject.layer == LayerMask.NameToLayer("Water")) {
			Hit.GetComponent<GolfHit>().inWater = true;
		}
	}
	
	void OnTriggerExit (Collider Hit) {
		
		if (Hit.GetComponent<GolfHit>() && gameObject.layer == LayerMask.NameToLayer("Water")) {
			Hit.GetComponent<GolfHit>().inWater = false;
		} else if (Hit.GetComponent<GolfHit>()) {
			GameObject.Find("LavaBurn").GetComponent<ParticleSystem>().Play();
		}
	}
}
