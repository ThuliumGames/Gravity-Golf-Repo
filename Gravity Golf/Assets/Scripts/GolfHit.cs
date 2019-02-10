using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GolfHit : MonoBehaviour
{
	public Transform Direction;

	public GameObject Arrow;

	public Rigidbody RB;

	public float PowerMultiplier;

	public int Strokes;

	public Text Hits;

	public Image Pow;

	public GameObject ControlsPage;

	public GameObject[] ObjOff;

	public GameObject[] ObjOn;

	private float Power;

	private Vector3 LastPlace = new Vector3(0f, 0.25f, 0f);

	public bool OnGround = true;

	private float T;

	private bool Shake;

	private float Magni;

	private bool CanDamage;

	private float T2;

	public AudioClip[] HitSounds;

	public GameObject RWing;

	private void Start()
	{
		int num = Random.Range(0, 100);
		if (num == 50)
		{
			GameObject gameObject = RWing = Object.Instantiate(RWing, RWing.transform.position, RWing.transform.rotation);
			Invoke("KillRWing", 25f);
		}
	}

	private void Update()
	{
		if (base.name != "GBC(Clone)")
		{
			if (Vector3.Distance(base.transform.position, LastPlace) <= 1f)
			{
				CanDamage = true;
			}
			if (GameObject.Find("BossPlanet") == null)
			{
				Hits.text = string.Empty + Strokes;
			}
			else
			{
				Hits.text = string.Empty + Object.FindObjectOfType<Boss1_AI>().PlayerHealth;
				Strokes = 10;
			}
			Pow.fillAmount = Power / 25f;
			if (Input.GetKeyDown(KeyCode.T))
			{
				if (GameObject.Find(ControlsPage.name) != null)
				{
					ControlsPage.SetActive(value: false);
				}
				else
				{
					ControlsPage.SetActive(value: true);
				}
			}
			if (Input.GetKeyDown(KeyCode.Backspace))
			{
				Application.LoadLevel(Application.loadedLevel);
			}
			if (Input.GetButton("Fire1"))
			{
				GameObject[] objOff = ObjOff;
				foreach (GameObject gameObject in objOff)
				{
					gameObject.SetActive(value: false);
				}
				GameObject[] objOn = ObjOn;
				foreach (GameObject gameObject2 in objOn)
				{
					gameObject2.SetActive(value: true);
				}
			}
			else
			{
				GameObject[] objOff2 = ObjOff;
				foreach (GameObject gameObject3 in objOff2)
				{
					gameObject3.SetActive(value: true);
				}
				GameObject[] objOn2 = ObjOn;
				foreach (GameObject gameObject4 in objOn2)
				{
					gameObject4.SetActive(value: false);
				}
			}
			if (Input.GetButton("Fire1"))
			{
				if (RB.velocity.magnitude < 1f)
				{
					RB.velocity = Vector3.zero;
					Power -= Input.GetAxis("Mouse Y");
					Power = Mathf.Clamp(Power, 0f, 25f);
					T2 += Time.deltaTime;
				}
				else
				{
					Power = 0f;
				}
			}
			else
			{
				if (Power > 0f)
				{
					if (T2 < 0.3f)
					{
						Power = 25f;
					}
					Strokes++;
					GameObject.Find(base.name + "Hit").GetComponent<AudioSource>().Play();
					LastPlace = base.transform.position;
					RB.velocity = Direction.up * Power * PowerMultiplier / 60f;
					ShakeCamera(0.15f, 0.25f);
				}
				Power = 0f;
				T2 = 0f;
			}
			if (Power > 0.1f)
			{
				if (GameObject.Find("GBC(Clone)") == null)
				{
					GameObject gameObject5 = Object.Instantiate(Arrow, base.transform.position, base.transform.rotation);
					gameObject5.GetComponent<Rigidbody>().velocity = Direction.up * Power * PowerMultiplier / 60f;
					gameObject5.GetComponent<TimedKill>().Invoke("Kill", 1f);
				}
				else if (GameObject.Find("GBC(Clone)").GetComponent<Rigidbody>().velocity.magnitude < 0.5f || Mathf.Abs(Input.GetAxis("Mouse X")) > 0.5f || Mathf.Abs(Input.GetAxis("Mouse Y")) > 0.5f || Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0.125f)
				{
					GameObject.Find("GBC(Clone)").GetComponent<TimedKill>().Kill();
				}
			}
			else if (GameObject.Find("GBC(Clone)") != null)
			{
				Object.Destroy(GameObject.Find("GBC(Clone)"));
			}
			if (Shake)
			{
				Direction.GetComponentInParent<CameraControl>().gameObject.GetComponentInChildren<Camera>().transform.localEulerAngles = Random.insideUnitSphere * Magni;
			}
			else
			{
				Direction.GetComponentInParent<CameraControl>().gameObject.GetComponentInChildren<Camera>().transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			}
		}
		if ((GameObject.Find("BossPlanet") == null || !Input.GetKey(KeyCode.D)) && (!OnGround || Direction.GetComponentInParent<CameraControl>().LookObj.name != "Desert"))
		{
			if (!OnGround)
			{
				RB.drag = 0.025f;
			}
			else
			{
				RB.drag = 0.05f;
			}
			if (RB.velocity.magnitude < 2f)
			{
				RB.drag = 1f;
			}
		}
		else
		{
			RB.drag = 4f;
		}
		if (base.name != "GBC(Clone)")
		{
			if (Input.GetKeyDown(KeyCode.R))
			{
				if (Strokes <= 1)
				{
					Application.LoadLevel(Application.loadedLevel);
				}
				else
				{
					Die();
				}
			}
			T -= Time.deltaTime;
		}
		OnGround = false;
	}

	private void OnCollisionStay(Collision Hit)
	{
		if ((bool)Hit.gameObject.GetComponentInChildren<Renderer>())
		{
			if (Hit.gameObject.GetComponentInChildren<Renderer>().materials[0].name == "Planet (Instance)")
			{
				if (GetComponentInChildren<AudioSource>().clip != HitSounds[0])
				{
					GetComponentInChildren<AudioSource>().clip = HitSounds[0];
					T = 0f;
				}
			}
			else if (Hit.gameObject.GetComponentInChildren<Renderer>().materials[0].name == "Rock (Instance)")
			{
				if (GetComponentInChildren<AudioSource>().clip != HitSounds[1])
				{
					GetComponentInChildren<AudioSource>().clip = HitSounds[1];
					T = 0f;
				}
			}
			else if (GetComponentInChildren<AudioSource>().clip != HitSounds[2])
			{
				GetComponentInChildren<AudioSource>().clip = HitSounds[2];
				T = 0f;
			}
		}
		if (base.name != "GBC(Clone)" && RB.velocity.magnitude > 0.1f && T <= 0f)
		{
			T = 0.25f;
			GetComponentInChildren<AudioSource>().volume = Mathf.Clamp01(RB.velocity.magnitude / 20f);
			GetComponentInChildren<AudioSource>().Play();
			GetComponentInChildren<ParticleSystem>().Play();
		}
		OnGround = true;
	}

	private void OnCollisionEnter(Collision Hit)
	{
		ShakeCamera(0.2f, RB.velocity.magnitude / 20f);
		T = 0f;
	}

	private void OnCollisionExit(Collision Hit)
	{
		OnGround = false;
	}

	private void ShakeCamera(float Dur, float Mag)
	{
		if (!Shake)
		{
			Magni = Mag;
			Shake = true;
			Invoke("StopShake", Dur);
		}
	}

	private void StopShake()
	{
		Shake = false;
	}

	public void Die()
	{
		RB.velocity = Vector3.zero;
		RB.angularVelocity = Vector3.zero;
		if (GameObject.Find("BossPlanet") == null)
		{
			if (Vector3.Distance(base.transform.position, LastPlace) > 1f)
			{
				if (CanDamage)
				{
					Strokes++;
				}
				CanDamage = false;
			}
			else
			{
				CanDamage = true;
			}
		}
		else
		{
			if (Vector3.Distance(base.transform.position, new Vector3(0f, 25.5f, 0f)) > 3f)
			{
				if (CanDamage)
				{
					Object.FindObjectOfType<Boss1_AI>().PlayerHealth--;
				}
				CanDamage = false;
			}
			else
			{
				CanDamage = true;
			}
			LastPlace = new Vector3(0f, 25.5f, 0f);
		}
		base.transform.position = LastPlace;
		GameObject.Find("Cam").GetComponent<CameraControl>().VertAng = 10f;
	}

	private void KillRWing()
	{
		Object.Destroy(RWing);
	}
}