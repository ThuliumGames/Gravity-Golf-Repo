using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
 using UnityEngine.Rendering.PostProcessing;

public class CameraControl : MonoBehaviour {
	
	public Transform ParentControl;
	
	public Transform Ball;

	public Transform BallRot;

	public Transform Cam;
	public Transform CamFixer;
	public GameObject OB;
	
	public Transform LookObj;
	public Transform LookPos;
	private Transform LookObjPrev;

	public float MouseSpeed;

	public float Ang;

	public float VertAng = 10f;

	public float HitAngle;
	public static float PrevHitAngle;

	public float CamDist = 2f;

	public LayerMask LM;

	public bool NewPlanet;
	public bool MovingCam;
	
	float FixSpeed;
	
	public bool isDying;
	
	public bool isShooting;
	public float T1;
	public float T2;
	public float T3;
	
	public Transform lostBall;
	public Transform Goal;
	public Transform GoalIm;
	
	PostProcessVolume PPP;
	DepthOfField DOF;
	
	private void Start() {
		
		PPP = Cam.GetComponent<PostProcessVolume>();
		
		PPP.profile.TryGetSettings(out DOF);
		
		if (GameObject.Find("BossPlanet") == null) {
			Goal = GameObject.FindObjectOfType<Win>().transform;
		}
		
		if (VertAng != 70) {
			VertAng = 10f;
		}
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		
		float num = Mathf.Infinity;
		Planet[] array = Object.FindObjectsOfType<Planet>();
		foreach (Planet planet in array) {
			if (Vector3.Distance(base.transform.position, planet.gameObject.transform.position) < num && planet.tag != "BlackHole" && planet.tag != "WhiteHole") {
				num = Vector3.Distance(base.transform.position, planet.gameObject.transform.position);
				LookObj = planet.gameObject.transform;
			}
		}
		LookObjPrev = LookObj;
		
		lostBall.gameObject.SetActive(false);
	}

	private void Update() {
		
		if (GameObject.Find("BossPlanet") == null) {
			if (Cam.InverseTransformPoint(Goal.position).z > 1) {
				GoalIm.gameObject.SetActive(true);
				GoalIm.localPosition = new Vector3 (Mathf.Clamp (Cam.InverseTransformPoint(Goal.position).x/Cam.InverseTransformPoint(Goal.position).z*(1920/2), -1820/2, 1820/2), Mathf.Clamp (Cam.InverseTransformPoint(Goal.position).y/Cam.InverseTransformPoint(Goal.position).z*(1920/2), -980/2, 980/2), 0);
			} else {
				GoalIm.gameObject.SetActive(false);
			}
		}
		
		if (!Ball.GetComponent<GolfHit>().Paused) {
			
			DOF.focusDistance.value = 10;
			
			if (isShooting) {
				T1 = 0;
				T2 += Time.deltaTime;
			} else {
				T1 += Time.deltaTime;
				T2 = 0;
			}
			
			/*if (Input.GetButton("Fire1")) {
				T3 += Time.deltaTime*4;
				if (isShooting) {
					if (T2 < 0.3f) {
						T2 = 0;
						if (Ball.GetComponent<GolfHit>().PuttingMode) {
							Ball.GetComponent<GolfHit>().Power = 21.375f*((Mathf.Sin(T3)/3)+0.675f);
						} else {
							Ball.GetComponent<GolfHit>().Power = 100f*((Mathf.Sin(T3)/3)+0.675f);
						}
					}
				} else {
					if (T1 < 0.3f) {
						T1 = 0;
						if (Ball.GetComponent<GolfHit>().PuttingMode) {
							Ball.GetComponent<GolfHit>().Power = 21.375f*((Mathf.Sin(T3)/3)+0.675f);
						} else {
							Ball.GetComponent<GolfHit>().Power = 100f*((Mathf.Sin(T3)/3)+0.675f);
						}
						isShooting = true;
					}
				}
			} else {
				T3 = 0;
			}*/
			
			if (Input.GetButton("Fire1")) {
				isShooting = true;
			} else {
				isShooting = false;
			}
			
			if (!isDying) {
				
				OB.SetActive(false);
					
				if (!isShooting) {
					CamDist -= Input.GetAxis ("Trigger and Scroll")*20;
				} else {
					HitAngle += Input.GetAxis ("Trigger and Scroll")*20;
				}
				CamDist = Mathf.Clamp (CamDist, 3, 25);
				HitAngle = Mathf.Clamp (HitAngle, -90, 90);
				//Find Planet
				float num = Mathf.Infinity;
				Planet[] array = Object.FindObjectsOfType<Planet>();
				foreach (Planet planet in array) {
					if (Vector3.Distance(Ball.transform.position, planet.gameObject.transform.position) < num && planet.tag != "BlackHole" && planet.tag != "WhiteHole" && (Vector3.Distance(Ball.transform.position, planet.gameObject.transform.position) >= Vector3.Distance(Ball.transform.position+Ball.GetComponent<Rigidbody>().velocity, planet.gameObject.transform.position) || planet.gameObject.transform == LookObjPrev)) {
						if (planet.gameObject.transform == LookObjPrev) {
							num = Vector3.Distance(Ball.transform.position, planet.gameObject.transform.position)/2;
						} else {
							num = Vector3.Distance(Ball.transform.position, planet.gameObject.transform.position);
						}
						LookObj = planet.gameObject.transform;
					}
				}
				if (LookObjPrev != LookObj) {
					NewPlanet = true;
					if (!MovingCam) {
						CamFixer.position = Cam.position;
						CamFixer.rotation = Cam.rotation;
						Invoke ("StopCam", 1f);
					}
					MovingCam = true;
				}
				
				LookPos.position = LookObj.position;
				
				
				if (Input.GetKeyDown(KeyCode.Escape)) {
					Cursor.lockState = CursorLockMode.None;
				}
				
				if (!Object.FindObjectOfType<Win>().EndThing) {
					if (Input.GetKeyDown(KeyCode.Mouse0)) {
						Cursor.visible = false;
						Cursor.lockState = CursorLockMode.Locked;
					}
					
					//Move
					if (!MovingCam) {
						FixSpeed = 0;
						CamFixer.GetComponent<Camera>().depth = -1;
						Ang += Input.GetAxis("Horizontal2")*2;
						if (!isShooting) {
							VertAng -= Input.GetAxis("Vertical2")*2;
						}
					} else {
						FixSpeed += Time.deltaTime*1;
						CamFixer.GetComponent<Camera>().depth = 1;
						if (Vector3.Distance (CamFixer.position, Cam.position) > 1f || NewPlanet) {
							CamFixer.position = Vector3.Lerp (CamFixer.position, Cam.position, FixSpeed*2.5f*Vector3.Distance (CamFixer.position, Cam.position)*Time.deltaTime);
							CamFixer.rotation = Quaternion.RotateTowards (CamFixer.rotation, Cam.rotation, FixSpeed*1800f*Time.deltaTime);
						} else {
							MovingCam = false;
						}
					}
					
					if (!NewPlanet) {
						if (!LookObj.GetComponent<Planet>().isDirectional) {
							ParentControl.position = LookPos.position;
							ParentControl.Rotate ((transform.InverseTransformPoint (Ball.position).z/Vector3.Distance(Ball.position, LookPos.position))*Mathf.PI, 0, (-transform.InverseTransformPoint (Ball.position).x/Vector3.Distance(Ball.position, LookPos.position))*Mathf.PI);
							ParentControl.position += (transform.up*Vector3.Distance(Ball.position, LookPos.position));
						} else {
							ParentControl.position = Ball.position;
							ParentControl.LookAt(ParentControl.position + LookObj.GetComponent<Planet>().Direction);
							ParentControl.Rotate (-90, 0, 0);
						}
						VertAng = Mathf.Clamp (VertAng, -90, 90);
						transform.eulerAngles = ParentControl.eulerAngles;
						transform.position = Vector3.Lerp (transform.position, ParentControl.position, 250*Time.deltaTime);
					} else {
						ParentControl.position = Ball.position;
						ParentControl.LookAt(LookPos.position);
						ParentControl.Rotate (-90, 0, 0);
						transform.eulerAngles = ParentControl.eulerAngles;
						transform.position = ParentControl.position;
						VertAng = 90;
					}
					Cam.localPosition = new Vector3(0f, 0f, 0f);
					Cam.localEulerAngles = Vector3.zero;
					Cam.Rotate (VertAng, Ang, 0);
					BallRot.localEulerAngles = new Vector3 (HitAngle, Cam.localEulerAngles.y, 0);
				} else {
					Cursor.visible = true;
					Cursor.lockState = CursorLockMode.None;
					if (!Object.FindObjectOfType<Win>().EndThing2) {
						Cam.position = Vector3.Lerp(Cam.position, GameObject.Find("EndGamePos").transform.position, 3f * Time.deltaTime);
						Cam.LookAt(Ball.transform.position);
					} else {
						Cam.position = Vector3.Lerp(Cam.position, GameObject.Find("EndGamePos2").transform.position, 5f * Time.deltaTime);
						Cam.LookAt(GameObject.Find("EndGamePos2").GetComponentInParent<Planet>().transform.position);
					}
				}
				
				NewPlanet = false;
				LookObjPrev = LookObj;
			
			} else {
				OB.SetActive(true);
				Cam.transform.LookAt (Ball.position);
			}
		} else {
			DOF.focusDistance.value = 0;
		}
	}
	
	void LateUpdate () {
		if (!Ball.GetComponent<GolfHit>().Paused) {
			if (!isDying && !Object.FindObjectOfType<Win>().EndThing) {
				//Ball Rendering
				Ball.GetComponentInChildren<TrailRenderer>().enabled = true;
				RaycastHit hitInfo;
				
				/*if (Physics.Raycast(Ball.transform.position, -Cam.transform.forward, out hitInfo, CamDist, LayerMask.NameToLayer("Everything"))) {
					
					if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer ("Tree") || hitInfo.collider.gameObject.layer == LayerMask.NameToLayer ("Branch")) {
						foreach (FixColor G in GameObject.FindObjectsOfType<FixColor>()) {
							if (G.gameObject.layer == LayerMask.NameToLayer ("Tree")) {
								MeshRenderer MR = G.GetComponent<MeshRenderer>();
								Color c = MR.materials[0].GetColor ("_BaseColor");
								MR.material.SetFloat("_Mode", 2f);
								MR.material.SetInt("_SrcBlend", 5);
								MR.material.SetInt("_DstBlend", 10);
								MR.material.SetInt("_ZWrite", 0);
								MR.material.DisableKeyword("_ALPHATEST_ON");
								MR.material.EnableKeyword("_ALPHABLEND_ON");
								MR.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
								MR.material.renderQueue = 3000;
								MR.materials[0].SetColor ("_BaseColor", new Color(c.r, c.g, c.b, 0.5f));
							}
						}
					}
				}*/
				
				
				if (Physics.Raycast(Ball.transform.position, -Cam.transform.forward, out hitInfo, CamDist, LM)) {
					Cam.Translate(0f, 0f, -hitInfo.distance + 0.1f);
					if (hitInfo.distance < 0.5f) {
						if (hitInfo.distance < 0.375f) {
							Ball.GetComponentInChildren<TrailRenderer>().enabled = false;
						}
						
						Ball.GetComponent<MeshRenderer>().material.SetFloat("_Mode", 2f);
						Ball.GetComponent<MeshRenderer>().material.SetInt("_SrcBlend", 5);
						Ball.GetComponent<MeshRenderer>().material.SetInt("_DstBlend", 10);
						Ball.GetComponent<MeshRenderer>().material.SetInt("_ZWrite", 0);
						Ball.GetComponent<MeshRenderer>().material.DisableKeyword("_ALPHATEST_ON");
						Ball.GetComponent<MeshRenderer>().material.EnableKeyword("_ALPHABLEND_ON");
						Ball.GetComponent<MeshRenderer>().material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
						Ball.GetComponent<MeshRenderer>().material.renderQueue = 3000;
						
						if (hitInfo.distance < 0.19f) {
							Ball.GetComponent<MeshRenderer>().materials[0].SetColor ("_BaseColor", new Color(1f, 1f, 1f, 0f));
						} else {
							Ball.GetComponent<MeshRenderer>().materials[0].SetColor ("_BaseColor", new Color(1f, 1f, 1f, Mathf.Clamp01(hitInfo.distance * 2f) - 0.375f));
						}
					} else {
						Ball.GetComponent<MeshRenderer>().materials[0].SetColor ("_BaseColor", new Color(1f, 1f, 1f, 1f));
					}
				} else {
					Cam.Translate(0f, 0f, 0f - CamDist + 0.1f);
					Ball.GetComponent<MeshRenderer>().materials[0].SetColor ("_BaseColor", new Color(1f, 1f, 1f, 1f));
				}
			}
		}
	}
	
	public void IsDying () {
		isDying = true;
		Invoke ("NotDying", 2);
	}
	
	void NotDying () {
		isDying = false;
	}

	private void RHA() {
		HitAngle = 5f;
	}
	
	void StopCam () {
		MovingCam = false;
	}
}