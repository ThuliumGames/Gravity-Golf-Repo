using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour
{
	public Transform Ball;

	public Transform BallRot;

	public Transform Cam;

	public Transform LookObj;

	public Transform PointToLook;

	public float MouseSpeed;

	public float Ang;

	public float VertAng = 10f;

	private float HitAngle;

	private float CamDist = 2f;

	public LayerMask LM;

	private bool NewPlanet;

	private Vector3 PrevAng;

	private bool ChangeAng;

	private void Start()
	{
		VertAng = 10f;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		float num = 1000000f;
		Planet[] array = Object.FindObjectsOfType<Planet>();
		foreach (Planet planet in array)
		{
			if (Vector3.Distance(base.transform.position, planet.gameObject.transform.position) < num)
			{
				num = Vector3.Distance(base.transform.position, planet.gameObject.transform.position);
				LookObj = planet.gameObject.transform;
			}
		}
		PointToLook.position = LookObj.position;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Cursor.lockState = CursorLockMode.None;
		}
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}
		if (!Object.FindObjectOfType<Win>().EndThing)
		{
			Transform lookObj = LookObj;
			float num = 1000000f;
			Planet[] array = Object.FindObjectsOfType<Planet>();
			foreach (Planet planet in array)
			{
				if (Vector3.Distance(base.transform.position, planet.gameObject.transform.position) < num)
				{
					num = Vector3.Distance(base.transform.position, planet.gameObject.transform.position);
					LookObj = planet.gameObject.transform;
				}
			}
			if (num > 500f)
			{
				Ball.gameObject.GetComponent<GolfHit>().Die();
			}
			if (lookObj != LookObj)
			{
				NewPlanet = true;
			}
			Vector3 position = base.transform.position;
			base.transform.position = Ball.transform.position;
			if (LookObj.gameObject.layer != LayerMask.NameToLayer("Death"))
			{
				if (NewPlanet)
				{
					NewPlanet = false;
					ChangeAng = true;
				}
				if (LookObj.gameObject.GetComponent<Planet>().isDirectional)
				{
					PointToLook.position = Vector3.Lerp(PointToLook.position, base.transform.position + LookObj.gameObject.GetComponent<Planet>().Direction, 1000f * Time.deltaTime);
				}
				else
				{
					PointToLook.position = Vector3.Lerp(PointToLook.position, LookObj.position, 10f * Time.deltaTime);
				}
			}
			if (ChangeAng)
			{
				VertAng = Mathf.Lerp(VertAng, 90f, 10f * Time.deltaTime);
				if (VertAng > 89f)
				{
					ChangeAng = false;
				}
			}
			PointToLook.LookAt(base.transform.position);
			base.transform.eulerAngles = Vector3.zero;
			base.transform.up = PointToLook.forward;
			Transform transform = GameObject.Find("ShadowSphere").transform;
			Vector3 position2 = LookObj.position;
			Vector3 forward = PointToLook.forward;
			Vector3 localScale = LookObj.localScale;
			transform.position = position2 + forward * localScale.x;
			GameObject.Find("ShadowSphere").transform.up = PointToLook.forward;
			Ang += Input.GetAxis("Mouse X") * MouseSpeed * Time.deltaTime;
			if (!Input.GetButton("Fire1"))
			{
				if (!ChangeAng)
				{
					VertAng -= Input.GetAxis("Mouse Y") * MouseSpeed * Time.deltaTime;
				}
				CamDist -= Input.GetAxis("Mouse ScrollWheel") * MouseSpeed * 1f * Time.deltaTime;
				Invoke("RHA", Time.deltaTime * 5f);
			}
			else
			{
				HitAngle += Input.GetAxis("Mouse ScrollWheel") * MouseSpeed * 10f * Time.deltaTime;
			}
			HitAngle = Mathf.Clamp(HitAngle, -45f, 0f);
			CamDist = Mathf.Clamp(CamDist, 2f, 20f);
			BallRot.eulerAngles = base.transform.eulerAngles;
			BallRot.Rotate(90f + HitAngle, 0f, 0f);
			VertAng = Mathf.Clamp(VertAng, -90f, 90f);
			base.transform.RotateAround(base.transform.position, base.transform.up, Ang);
			base.transform.RotateAround(base.transform.position, base.transform.right, VertAng);
			BallRot.RotateAround(base.transform.position, base.transform.right, 0f - VertAng);
			GameObject gameObject = new GameObject();
			gameObject.transform.position = base.transform.position;
			gameObject.transform.LookAt(Cam.transform.position);
			Ball.GetComponentInChildren<TrailRenderer>().enabled = true;
			RaycastHit hitInfo;
			if (Physics.Raycast(base.transform.position, gameObject.transform.forward, out hitInfo, CamDist, LM))
			{
				Cam.localPosition = new Vector3(0f, 0f, 0f - hitInfo.distance);
				if (hitInfo.distance < 0.5f)
				{
					if (hitInfo.distance < 0.375f)
					{
						Ball.GetComponentInChildren<TrailRenderer>().enabled = false;
					}
					if (hitInfo.distance < 0.19f)
					{
						Ball.GetComponent<MeshRenderer>().materials[0].color = new Color(1f, 1f, 1f, 0f);
					}
					else
					{
						Ball.GetComponent<MeshRenderer>().materials[0].color = new Color(1f, 1f, 1f, Mathf.Clamp01(hitInfo.distance * 2f) - 0.375f);
					}
					Ball.GetComponent<MeshRenderer>().material.SetFloat("_Mode", 2f);
					Ball.GetComponent<MeshRenderer>().material.SetInt("_SrcBlend", 5);
					Ball.GetComponent<MeshRenderer>().material.SetInt("_DstBlend", 10);
					Ball.GetComponent<MeshRenderer>().material.SetInt("_ZWrite", 0);
					Ball.GetComponent<MeshRenderer>().material.DisableKeyword("_ALPHATEST_ON");
					Ball.GetComponent<MeshRenderer>().material.EnableKeyword("_ALPHABLEND_ON");
					Ball.GetComponent<MeshRenderer>().material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
					Ball.GetComponent<MeshRenderer>().material.renderQueue = 3000;
				}
				else
				{
					Ball.GetComponent<MeshRenderer>().material.SetFloat("_Mode", 0f);
					Ball.GetComponent<MeshRenderer>().material.SetInt("_SrcBlend", 5);
					Ball.GetComponent<MeshRenderer>().material.SetInt("_DstBlend", 10);
					Ball.GetComponent<MeshRenderer>().material.SetInt("_ZWrite", 1);
					Ball.GetComponent<MeshRenderer>().material.DisableKeyword("_ALPHATEST_ON");
					Ball.GetComponent<MeshRenderer>().material.DisableKeyword("_ALPHABLEND_ON");
					Ball.GetComponent<MeshRenderer>().material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
					Ball.GetComponent<MeshRenderer>().material.renderQueue = 2000;
				}
			}
			else
			{
				Cam.localPosition = new Vector3(0f, 0f, 0f - CamDist + 0.1f);
				Ball.GetComponent<MeshRenderer>().material.SetFloat("_Mode", 0f);
				Ball.GetComponent<MeshRenderer>().material.SetInt("_SrcBlend", 5);
				Ball.GetComponent<MeshRenderer>().material.SetInt("_DstBlend", 10);
				Ball.GetComponent<MeshRenderer>().material.SetInt("_ZWrite", 1);
				Ball.GetComponent<MeshRenderer>().material.DisableKeyword("_ALPHATEST_ON");
				Ball.GetComponent<MeshRenderer>().material.DisableKeyword("_ALPHABLEND_ON");
				Ball.GetComponent<MeshRenderer>().material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
				Ball.GetComponent<MeshRenderer>().material.renderQueue = 2000;
			}
			Object.Destroy(gameObject);
		}
		else
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			if (!Object.FindObjectOfType<Win>().EndThing2)
			{
				Cam.position = Vector3.Lerp(Cam.position, GameObject.Find("EndGamePos").transform.position, 0.1f * Time.deltaTime);
				Cam.LookAt(base.transform.position);
			}
			else
			{
				Cam.position = Vector3.Lerp(Cam.position, GameObject.Find("EndGamePos2").transform.position, 5f * Time.deltaTime);
				Cam.LookAt(GameObject.Find("EndGamePos2").GetComponentInParent<Planet>().transform.position);
			}
		}
	}

	private void RHA()
	{
		HitAngle = 5f;
	}
}