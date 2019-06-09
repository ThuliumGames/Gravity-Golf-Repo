using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GolfHit : MonoBehaviour {
	public Transform Direction;

	public GameObject Arrow;

	public Rigidbody RB;

	public float PowerMultiplier;

	public int Strokes;

	public Text Hits;

	public Image Pow;
	public Image AngGraph;

	public GameObject ControlsPage;

	public GameObject[] ObjOff;

	public GameObject[] ObjOn;

	public float Power;

	private Vector3 LastPlace = new Vector3(0f, 0.25f, 0f);

	public bool OnGround = true;
	public bool PublicOnGround;
	public bool inWater;

	private float T;

	private bool Shake;

	private float Magni;

	private bool CanDamage;

	private float T2;

	public AudioClip[] HitSounds;
	
	public GameObject HoleIOConfetti;
	
	bool PuttingMode;
	
	float CanFF;
	
	public static bool hideControls;
	
	public Image PMGlow;
	
	bool SlowDown;

	private void Update() {
		
		ControlsPage.SetActive(!hideControls);
		
		if (base.name != "GBC(Clone)") {
			
			if (RB.velocity.magnitude < 1f) {
				CanFF = 0;
				if (Input.GetKeyDown(KeyCode.P)) {
					PuttingMode = !PuttingMode;
				}
			} else {
				CanFF += Time.deltaTime;
			}
			
			if (PuttingMode) {
				PMGlow.enabled = true;
			} else {
				PMGlow.enabled = false;
			}
			
			if (!Direction.GetComponentInParent<CameraControl>().isDying) {
				bool CanKill = true;
				foreach (Portal P in GameObject.FindObjectsOfType<Portal>()) {
					if (Vector3.Distance (transform.position+RB.velocity, P.transform.position) <= Vector3.Distance (transform.position, P.transform.position)) {
						CanKill = false;
					}
				}
				foreach (Planet P in GameObject.FindObjectsOfType<Planet>()) {
					if (Vector3.Distance (transform.position+RB.velocity, P.transform.position) <= Vector3.Distance (transform.position, P.transform.position) || Vector3.Distance (transform.position, P.transform.position) <= (P.Range+P.transform.localScale.x)*4) {
						CanKill = false;
					}
				}
				
				if (CanKill) {
					Invoke ("Die", 2);
					Direction.GetComponentInParent<CameraControl>().IsDying();
				}
			}
				
			if (Input.GetKey (KeyCode.Tab) && CanFF >= 8) {
				Time.timeScale = Mathf.Lerp(Time.timeScale, 10, 5*Time.deltaTime);
			} else {
				Time.timeScale = 1;
			}
			
			if (Vector3.Distance(base.transform.position, LastPlace) <= 1f) {
				CanDamage = true;
			}
			if (GameObject.Find("BossPlanet") == null) {
				Hits.text = string.Empty + Strokes;
			} else {
				Hits.text = string.Empty + Object.FindObjectOfType<Boss1_AI>().PlayerHealth;
				Strokes = 10;
			}
			if (PuttingMode) {
				Pow.fillAmount = Power / 21.375f;
			} else {
				Pow.fillAmount = Power / 100f;
			}
			if (Input.GetKeyDown(KeyCode.T)) {
				hideControls = !hideControls;
			}
			if (Input.GetKeyDown(KeyCode.Backspace)) {
				Application.LoadLevel(Application.loadedLevel);
			}
			if (Input.GetButton("Fire1")) {
				GameObject[] objOff = ObjOff;
				foreach (GameObject gameObject in objOff) {
					gameObject.SetActive(value: false);
				}
				GameObject[] objOn = ObjOn;
				foreach (GameObject gameObject2 in objOn) {
					gameObject2.SetActive(value: true);
				}
			} else {
				GameObject[] objOff2 = ObjOff;
				foreach (GameObject gameObject3 in objOff2) {
					gameObject3.SetActive(value: true);
				}
				GameObject[] objOn2 = ObjOn;
				foreach (GameObject gameObject4 in objOn2) {
					gameObject4.SetActive(value: false);
				}
			}
			if (Input.GetButton("Fire1")) {
				if (RB.velocity.magnitude < 1f) {
					RB.velocity = Vector3.zero;
					Power -= Input.GetAxis("Mouse Y")*((Power/25)+0.1f);
					AngGraph.transform.localPosition = new Vector3 (0, GameObject.FindObjectOfType<CameraControl>().HitAngle, 0);
					if (PuttingMode) {
						Power = Mathf.Clamp(Power, 0f, 21.375f);
					} else {
						Power = Mathf.Clamp(Power, 0f, 100f);
					}
					T2 += Time.deltaTime;
				} else {
					Power = 0f;
				}
			} else {
				if (Power > 0f) {
					if (T2 < 0.3f) {
						Power = 25f;
					}
					RB.isKinematic = false;
					Strokes++;
					GameObject.Find(base.name + "Hit").GetComponent<AudioSource>().Play();
					LastPlace = base.transform.position;
					RB.velocity = Direction.forward * Power * PowerMultiplier / 60f;
					ShakeCamera(0.15f, 0.25f);
					GameObject.FindObjectOfType<CameraControl>().HitAngle = 0;
				}
				Power = 0f;
				T2 = 0f;
			}
			if (Power > 0.1f) {
				RB.isKinematic = true;
				if (GameObject.Find("GBC(Clone)") == null) {
					GameObject gameObject5 = Object.Instantiate(Arrow, base.transform.position, base.transform.rotation);
					gameObject5.GetComponent<Rigidbody>().velocity = Direction.forward * Power * PowerMultiplier / 60f;
					gameObject5.GetComponent<TimedKill>().Invoke("Kill", 1f);
				} else if (GameObject.Find("GBC(Clone)").GetComponent<Rigidbody>().velocity.magnitude < 0.5f || Mathf.Abs(Input.GetAxis("Mouse X")) > 0.5f || Mathf.Abs(Input.GetAxis("Mouse Y")) > 0.5f || Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0.125f) {
					GameObject.Find("GBC(Clone)").GetComponent<TimedKill>().Kill();
				}
			} else if (GameObject.Find("GBC(Clone)") != null) {
				Object.Destroy(GameObject.Find("GBC(Clone)"));
			}
			if (Shake) {
				Camera.main.GetComponentInParent<Collider>().transform.localEulerAngles = Random.insideUnitSphere * Magni * 2;
			} else {
				Camera.main.GetComponentInParent<Collider>().transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			}
			if (SlowDown) {
				RB.drag = 4;
			} else {
				if (((GameObject.Find("BossPlanet") == null || !Input.GetKey(KeyCode.D)) && (!OnGround || Direction.GetComponentInParent<CameraControl>().LookObj.name != "Desert")) && !inWater) {
					if (!OnGround) {
						RB.drag = 0.025f;
					} else {
						RB.drag = 0.05f;
					}
					if (RB.velocity.magnitude < 2f) {
						RB.drag = 1f;
					}
				} else {
					if (!inWater) {
						RB.drag = 4f;
					} else {
						RB.drag = 6f;
					}
				}
			}
			
		} else {
			if (SlowDown) {
				RB.drag = 4;
			} else {
				if (((GameObject.Find("BossPlanet") == null || !Input.GetKey(KeyCode.D)) && (!OnGround || GameObject.Find("Golf Ball").GetComponent<GolfHit>().Direction.GetComponentInParent<CameraControl>().LookObj.name != "Desert")) && !inWater) {
					if (!OnGround) {
						RB.drag = 0.025f;
					} else {
						RB.drag = 0.05f;
					}
					if (RB.velocity.magnitude < 2f) {
						RB.drag = 1f;
					}
				} else {
					if (!inWater) {
						RB.drag = 4f;
					} else {
						RB.drag = 6f;
					}
				}
			}
		}
		if (base.name != "GBC(Clone)") {
			if (Input.GetKeyDown(KeyCode.R)) {
				if (Strokes <= 1) {
					Application.LoadLevel(Application.loadedLevel);
				} else {
					Die();
				}
			}
			T -= Time.deltaTime;
		}
		OnGround = false;
	}

	private void OnCollisionStay(Collision Hit) {
		if ((bool)Hit.gameObject.GetComponentInChildren<Renderer>()) {
			if (Hit.gameObject.GetComponentInChildren<Renderer>().materials[0].name == "Planet (Instance)") {
				if (GetComponentInChildren<AudioSource>().clip != HitSounds[0]) {
					GetComponentInChildren<AudioSource>().clip = HitSounds[0];
					T = 0f;
				}
			} else if (Hit.gameObject.GetComponentInChildren<Renderer>().materials[0].name == "Metal (Instance)" || Hit.gameObject.GetComponentInChildren<Renderer>().materials[0].name == "Rock (Instance)") {
				if (GetComponentInChildren<AudioSource>().clip != HitSounds[1]) {
					GetComponentInChildren<AudioSource>().clip = HitSounds[1];
					T = 0f;
				}
			} else if (GetComponentInChildren<AudioSource>().clip != HitSounds[2]) {
				GetComponentInChildren<AudioSource>().clip = HitSounds[2];
				T = 0f;
			}
		}
		if (base.name != "GBC(Clone)" && RB.velocity.magnitude > 0.1f && T <= 0f) {
			T = 0.25f;
			GetComponentInChildren<AudioSource>().volume = Mathf.Clamp01(RB.velocity.magnitude / 20f);
			GetComponentInChildren<AudioSource>().Play();
			GetComponentInChildren<ParticleSystem>().Play();
		}
		OnGround = true;
		PublicOnGround = true;
	}

	private void OnCollisionEnter(Collision Hit) {
		ShakeCamera(0.2f, RB.velocity.magnitude / 20f);
		T = 0f;
	}
	
	public void Slow () {
		SlowDown = true;
		Invoke ("UnSlow", 0.25f);
	}
	
	void UnSlow () {
		SlowDown = false;
	}

	private void OnCollisionExit(Collision Hit) {
		OnGround = false;
		PublicOnGround = false;
	}

	private void ShakeCamera(float Dur, float Mag) {
		if (!Shake) {
			Magni = Mag;
			Shake = true;
			Invoke("StopShake", Dur);
		}
	}

	private void StopShake() {
		Shake = false;
	}

	public void Die() {
		RB.velocity = Vector3.zero;
		RB.angularVelocity = Vector3.zero;
		if (GameObject.Find("BossPlanet") == null) {
			if (Vector3.Distance(base.transform.position, LastPlace) > 1f) {
				if (CanDamage) {
					Strokes++;
				}
				CanDamage = false;
			} else {
				CanDamage = true;
			}
		} else {
			if (Vector3.Distance(base.transform.position, new Vector3(0f, 25.5f, 0f)) > 3f) {
				if (CanDamage) {
					Object.FindObjectOfType<Boss1_AI>().PlayerHealth--;
				}
				CanDamage = false;
			} else {
				CanDamage = true;
			}
			LastPlace = new Vector3(0f, 100.75f, 0f);
		}
		base.transform.position = LastPlace;
		GameObject.FindObjectOfType<CameraControl>().VertAng = 10f;
	}
}