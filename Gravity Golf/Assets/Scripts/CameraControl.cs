using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour {
	
	public Transform ParentControl;
	public Transform BallParent;
	
	public Transform Ball;

	public Transform BallRot;

	public Transform Cam;
	public Transform CamFixer;
	
	public Transform LookObj;
	public Transform LookPos;
	private Transform LookObjPrev;

	public float MouseSpeed;

	public float Ang;

	public float VertAng = 10f;

	private float HitAngle;

	public float CamDist = 2f;

	public LayerMask LM;

	public bool NewPlanet;
	public bool MovingCam;
	
	float FixSpeed;
	
	private void Start() {
		VertAng = 10f;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		
		float num = Mathf.Infinity;
		Planet[] array = Object.FindObjectsOfType<Planet>();
		foreach (Planet planet in array) {
			if (Vector3.Distance(base.transform.position, planet.gameObject.transform.position) < num && planet.tag != "BlackHole") {
				num = Vector3.Distance(base.transform.position, planet.gameObject.transform.position);
				LookObj = planet.gameObject.transform;
			}
		}
		LookObjPrev = LookObj;
	}

	private void Update() {
		
		if (!Input.GetMouseButton(0)) {
			CamDist -= Input.GetAxis ("Mouse ScrollWheel")*10;
		} else {
			HitAngle += Input.GetAxis ("Mouse ScrollWheel")*10;
		}
		CamDist = Mathf.Clamp (CamDist, 3, 25);
		HitAngle = Mathf.Clamp (HitAngle, -80, 0);
		//Find Planet
		float num = Mathf.Infinity;
		Planet[] array = Object.FindObjectsOfType<Planet>();
		foreach (Planet planet in array) {
			if (Vector3.Distance(Ball.transform.position, planet.gameObject.transform.position) < num && planet.tag != "BlackHole" && (Vector3.Distance(Ball.transform.position, planet.gameObject.transform.position) >= Vector3.Distance(Ball.transform.position+Ball.GetComponent<Rigidbody>().velocity, planet.gameObject.transform.position) || planet.gameObject.transform == LookObjPrev)) {
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
				Invoke ("StopCam", 1.5f);
			}
			MovingCam = true;
		}
		
		LookPos.position = LookObj.position;
		
		
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Cursor.lockState = CursorLockMode.None;
		}
		
		if (Ball != null) {
			BallParent.position = Vector3.Lerp (BallParent.position, Ball.position, 15*Time.deltaTime);
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
				Ang += Input.GetAxis("Mouse X");
				if (!Input.GetMouseButton (0)) {
					VertAng -= Input.GetAxis("Mouse Y");
				}
			} else {
				FixSpeed += Time.deltaTime*1;
				CamFixer.GetComponent<Camera>().depth = 1;
				if (Vector3.Distance (CamFixer.position, Cam.position) > 1f || NewPlanet) {
					CamFixer.position = Vector3.Lerp (CamFixer.position, Cam.position, FixSpeed*0.5f*Vector3.Distance (CamFixer.position, Cam.position)*Time.deltaTime);
					CamFixer.rotation = Quaternion.RotateTowards (CamFixer.rotation, Cam.rotation, FixSpeed*360f*Time.deltaTime);
				} else {
					MovingCam = false;
				}
			}
			
			if (!NewPlanet) {
				if (!LookObj.GetComponent<Planet>().isDirectional) {
					ParentControl.position = LookPos.position;
					ParentControl.Rotate (transform.InverseTransformPoint (Ball.position).z/Vector3.Distance(Ball.position, LookPos.position), 0, -transform.InverseTransformPoint (Ball.position).x/Vector3.Distance(Ball.position, LookPos.position));
					ParentControl.position += (transform.up*Vector3.Distance(Ball.position, LookPos.position));
				} else {
					ParentControl.position = Ball.position;
					ParentControl.LookAt(ParentControl.position + LookObj.GetComponent<Planet>().Direction);
					ParentControl.Rotate (-90, 0, 0);
				}
				VertAng = Mathf.Clamp (VertAng, -90, 90);
				transform.eulerAngles = ParentControl.eulerAngles;
				transform.position = Vector3.Lerp (transform.position, ParentControl.position, 25*Time.deltaTime);
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
			
			//Ball Rendering
			BallParent.GetComponentInChildren<TrailRenderer>().enabled = true;
			RaycastHit hitInfo;
			if (Physics.Raycast(Ball.transform.position, -Cam.transform.forward, out hitInfo, CamDist, LM)) {
				Cam.Translate(0f, 0f, -hitInfo.distance + 0.1f);
				if (hitInfo.distance < 0.5f) {
					if (hitInfo.distance < 0.375f) {
						BallParent.GetComponentInChildren<TrailRenderer>().enabled = false;
					}
					if (hitInfo.distance < 0.19f) {
						BallParent.GetComponent<MeshRenderer>().materials[0].color = new Color(1f, 1f, 1f, 0f);
					} else {
						BallParent.GetComponent<MeshRenderer>().materials[0].color = new Color(1f, 1f, 1f, Mathf.Clamp01(hitInfo.distance * 2f) - 0.375f);
					}
					BallParent.GetComponent<MeshRenderer>().material.SetFloat("_Mode", 2f);
					BallParent.GetComponent<MeshRenderer>().material.SetInt("_SrcBlend", 5);
					BallParent.GetComponent<MeshRenderer>().material.SetInt("_DstBlend", 10);
					BallParent.GetComponent<MeshRenderer>().material.SetInt("_ZWrite", 0);
					BallParent.GetComponent<MeshRenderer>().material.DisableKeyword("_ALPHATEST_ON");
					BallParent.GetComponent<MeshRenderer>().material.EnableKeyword("_ALPHABLEND_ON");
					BallParent.GetComponent<MeshRenderer>().material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
					BallParent.GetComponent<MeshRenderer>().material.renderQueue = 3000;
				} else {
					BallParent.GetComponent<MeshRenderer>().material.SetFloat("_Mode", 0f);
					BallParent.GetComponent<MeshRenderer>().material.SetInt("_SrcBlend", 5);
					BallParent.GetComponent<MeshRenderer>().material.SetInt("_DstBlend", 10);
					BallParent.GetComponent<MeshRenderer>().material.SetInt("_ZWrite", 1);
					BallParent.GetComponent<MeshRenderer>().material.DisableKeyword("_ALPHATEST_ON");
					BallParent.GetComponent<MeshRenderer>().material.DisableKeyword("_ALPHABLEND_ON");
					BallParent.GetComponent<MeshRenderer>().material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
					BallParent.GetComponent<MeshRenderer>().material.renderQueue = 2000;
				}
			} else {
				
				Cam.Translate(0f, 0f, 0f - CamDist + 0.1f);
				
				BallParent.GetComponent<MeshRenderer>().material.SetFloat("_Mode", 0f);
				BallParent.GetComponent<MeshRenderer>().material.SetInt("_SrcBlend", 5);
				BallParent.GetComponent<MeshRenderer>().material.SetInt("_DstBlend", 10);
				BallParent.GetComponent<MeshRenderer>().material.SetInt("_ZWrite", 1);
				BallParent.GetComponent<MeshRenderer>().material.DisableKeyword("_ALPHATEST_ON");
				BallParent.GetComponent<MeshRenderer>().material.DisableKeyword("_ALPHABLEND_ON");
				BallParent.GetComponent<MeshRenderer>().material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
				BallParent.GetComponent<MeshRenderer>().material.renderQueue = 2000;
			}
			
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
	}

	private void RHA() {
		HitAngle = 5f;
	}
	
	void StopCam () {
		MovingCam = false;
	}
}