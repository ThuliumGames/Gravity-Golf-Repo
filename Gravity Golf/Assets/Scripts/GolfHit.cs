﻿using System.Collections;
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
	public Image PrevAngGraph;

	public GameObject ControlsPage;

	public GameObject[] ObjOff;

	public GameObject[] ObjOn;

	public float Power;

	private Vector3 LastPlace = new Vector3(0f, 0.75f, 0f);

	public bool OnGround = true;
	public bool PublicOnGround;
	public bool inWater;
	
	private bool Shake;

	private float Magni;

	private bool CanDamage;

	public AudioClip[] HitSounds;
	public AudioClip[] LandSounds;
	
	public GameObject HoleIOConfetti;
	
	public bool PuttingMode;
	
	float CanFF;
	
	public static bool hideControls;
	
	public Image PMGlow;
	public GameObject FFCont;
	
	bool SlowDown;
	
	public AudioSource PowerSource;
	public AudioSource LandSource;
	
	bool Swi;
	
	public bool Paused;
	public GameObject PauseObj;
	
	bool DragMode;
	
	int fs;
	float SpawnDelay;
	
	public static int GoodInARow;
	public static int GoodKeepTrack;

	private void Update() {
		if (GoodInARow != 0) {
			GoodKeepTrack = GoodInARow;
		} else {
			if (GoodKeepTrack >= 3) {
				SceneManager.LoadScene("Bonus Game");
			}
		}
		
		if (Input.GetButtonDown("Fire3")) {
			DragMode = !DragMode;
		}
		if (RB.velocity.magnitude < 0.1f) {
			DragMode = false;
		}
		
		if (base.name != "GBC(Clone)") {
			if (Input.GetButtonDown("Cancel")) {
				Paused = !Paused;
			}
			if (Paused) {
				PauseObj.SetActive (true);
			} else {
				PauseObj.SetActive (false);
			}
		}
			
		if (!Paused) {
			if (PowerSource != null) {
				if (Power > 1) {
					PowerSource.pitch = Mathf.Lerp (PowerSource.pitch, 10000/Mathf.Pow(-Power+200, 2), 10*Time.deltaTime);
					PowerSource.volume = Mathf.Lerp (PowerSource.volume, ((-(Power/100)+3f)/10)-0.075f, 10*Time.deltaTime);
				} else {
					PowerSource.pitch = 0;
					PowerSource.volume = 0;
				}
			}
			
			if (base.name != "GBC(Clone)") {
				
				ControlsPage.SetActive(!hideControls);
				
				if (Input.GetAxis ("Fire7") > 0.1f) {
					hideControls = false;
					ControlsPage.transform.localPosition = new Vector3 (192, 222, 0);
					ControlsPage.transform.localScale = new Vector3 (0.9f, 0.9f, 1);
				} else {
					ControlsPage.transform.localPosition = new Vector3 (800, -125, 0);
					ControlsPage.transform.localScale = new Vector3 (0.5f, 0.5f, 1);
				}
				
				if (inWater) {
					GameObject.Find("WaterSound").GetComponent<AudioSource>().volume = RB.velocity.magnitude;
				} else {
					GameObject.Find("WaterSound").GetComponent<AudioSource>().volume = 0;
				}
				
				
				if (Input.GetButtonDown("Fire3") && GameObject.Find("BossPlanet") == null) {
					PuttingMode = !PuttingMode;
				}
					
				if (RB.velocity.magnitude < 1f) {
					CanFF = 0;
				} else {
					CanFF += Time.deltaTime;
				}
				
				if (CanFF >= 8) {
					FFCont.gameObject.SetActive(true);
				} else {
					FFCont.gameObject.SetActive(false);
				}
				
				if (PuttingMode) {
					PMGlow.enabled = true;
				} else {
					PMGlow.enabled = false;
				}
				
				if (!Direction.GetComponentInParent<CameraControl>().isDying) {
					bool CanKill = true;
					foreach (Portal P in GameObject.FindObjectsOfType<Portal>()) {
						if (Vector3.Distance (transform.position+RB.velocity.normalized, P.transform.position) <= Vector3.Distance (transform.position, P.transform.position)) {
							CanKill = false;
						}
					}
					foreach (Planet P in GameObject.FindObjectsOfType<Planet>()) {
						if (Vector3.Distance (transform.position+RB.velocity.normalized, P.transform.position) <= Vector3.Distance (transform.position, P.transform.position) || Vector3.Distance (transform.position, P.transform.position) <= ((P.Range + (P.transform.localScale.x * 10)) * 2)) {
							CanKill = false;
						}
					}
					
					if (CanKill) {
						Invoke ("Die", 2);
						Direction.GetComponentInParent<CameraControl>().IsDying();
					}
				}
				if (Input.GetButton ("Fire4") && CanFF >= 8) {
					Time.timeScale = Mathf.Lerp(Time.timeScale, 10, 5*Time.deltaTime);
				} else {
					Time.timeScale = 1;
				}
				
				if (Vector3.Distance(base.transform.position, LastPlace) <= 1f) {
					CanDamage = true;
				}
				if (GameObject.Find("BossPlanet") == null) {
					Hits.text = "" + Strokes;
				} else {
					Hits.text = string.Empty + SnakeBossAI.PlayerHealth;
					Strokes = GameObject.FindObjectOfType<Win>().Par - (SnakeBossAI.PlayerHealth-5);
				}
				if (PuttingMode) {
					Pow.fillAmount = Power / 21.375f;
				} else {
					Pow.fillAmount = Power / 100f;
				}
				if (Input.GetAxis("Fire7") < -0.1f) {
					if (Swi) {
						Swi = false;
						hideControls = !hideControls;
					}
				} else {
					Swi = true;
				}
				if (Input.GetButtonDown("Fire6")) {
					PlayerPrefs.SetInt("NumResetTemp", PlayerPrefs.GetInt("NumResetTemp", 0)+1);
					Application.LoadLevel(Application.loadedLevel);
				}
				if (GameObject.FindObjectOfType<CameraControl>().isShooting) {
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
				if (GameObject.FindObjectOfType<CameraControl>().isShooting) {
					if (RB.velocity.magnitude < 1f) {
						RB.velocity = Vector3.zero;
						Power -= (Input.GetAxis("Vertical2")*2);
						AngGraph.transform.localPosition = new Vector3 (0, GameObject.FindObjectOfType<CameraControl>().HitAngle, 0);
						PrevAngGraph.transform.localPosition = new Vector3 (0, CameraControl.PrevHitAngle, 0);
						if (PuttingMode) {
							Power = Mathf.Clamp(Power, 0f, 21.375f);
						} else {
							Power = Mathf.Clamp(Power, 0f, 100f);
						}
					} else {
						Power = 0f;
						GameObject.FindObjectOfType<CameraControl>().isShooting = false;
					}
				} else {
					if (Power > 0f) {
						RB.isKinematic = false;
						Strokes++;
						GameObject.Find(base.name + "Hit").GetComponent<AudioSource>().clip = HitSounds[3];
						GameObject.Find(base.name + "Hit").GetComponent<AudioSource>().Play();
						LastPlace = base.transform.position;
						RB.velocity = Direction.forward * Power * PowerMultiplier / 60f;
						ShakeCamera(0.15f, 0.25f);
						CameraControl.PrevHitAngle = GameObject.FindObjectOfType<CameraControl>().HitAngle;
						GameObject.FindObjectOfType<CameraControl>().HitAngle = 0;
					}
					Power = 0f;
				}
				if (Power > 0.1f) {
					RB.isKinematic = true;
					if (GameObject.Find("GBC(Clone)") == null) {
						if (SpawnDelay > 0.25f) {
						GameObject Go = Instantiate(Arrow);
							Go.transform.position = transform.position;
							Go.GetComponent<Rigidbody>().velocity = Direction.forward * Power * PowerMultiplier / 60f;
							Go.GetComponent<TimedKill>().Invoke ("Kill", 1);
						} else {
							SpawnDelay += Time.deltaTime;
						}
					} else {
						SpawnDelay = 0;
					}
				} else {
					SpawnDelay = 0;
					if (GameObject.Find("GBC(Clone)") != null) {
						Destroy (GameObject.Find("GBC(Clone)"));
					}
				}
				if (Shake) {
					Camera.main.GetComponentInParent<Collider>().transform.localEulerAngles = Random.insideUnitSphere * Magni * 2;
				} else {
					Camera.main.GetComponentInParent<Collider>().transform.localEulerAngles = new Vector3(0f, 0f, 0f);
				}
				if (SlowDown) {
					RB.drag = 4;
				} else {
					if (((GameObject.Find("BossPlanet") == null || !DragMode) && (!OnGround || Direction.GetComponentInParent<CameraControl>().LookObj.name != "Desert")) && !inWater) {
						if (!OnGround) {
							GetComponentInChildren<AudioSource>().volume = Mathf.Lerp (GetComponentInChildren<AudioSource>().volume, 0, 10*Time.deltaTime);
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
							if (RB.drag != 6) {
								GameObject.Find("WaterHit").GetComponent<AudioSource>().Play();
							}
							RB.drag = 6f;
						}
					}
				}
				
			} else {
				if (SlowDown) {
					RB.drag = 4;
				} else {
					if (((GameObject.Find("BossPlanet") == null || !DragMode) && (!OnGround || GameObject.Find("Golf Ball").GetComponent<GolfHit>().Direction.GetComponentInParent<CameraControl>().LookObj.name != "Desert")) && !inWater) {
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
				if (Input.GetButtonDown("Fire5")) {
					if (Strokes <= 1) {
						PlayerPrefs.SetInt("NumResetTemp", PlayerPrefs.GetInt("NumResetTemp", 0)+1);
						Application.LoadLevel(Application.loadedLevel);
					} else {
						Die();
					}
				}
			}
			OnGround = false;
		} else {
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			Time.timeScale = 0;
		}
	}

	private void OnCollisionStay(Collision Hit) {
		if ((bool)Hit.gameObject.GetComponentInChildren<Renderer>()) {
			if (Hit.gameObject.GetComponentInChildren<Renderer>().materials[0].name == "Planet (Instance)" || Hit.gameObject.GetComponentInChildren<Renderer>().materials[0].name == "Tree (Instance)") {
				if (GetComponentInChildren<AudioSource>().clip != HitSounds[0]) {
					GetComponentInChildren<AudioSource>().clip = HitSounds[0];
				}
			} else if (Hit.gameObject.GetComponentInChildren<Renderer>().materials[0].name == "Metal (Instance)" || Hit.gameObject.GetComponentInChildren<Renderer>().materials[0].name == "Rock (Instance)") {
				if (GetComponentInChildren<AudioSource>().clip != HitSounds[1]) {
					GetComponentInChildren<AudioSource>().clip = HitSounds[1];
				}
			} else if (GetComponentInChildren<AudioSource>().clip != HitSounds[2]) {
				GetComponentInChildren<AudioSource>().clip = HitSounds[2];
			}
		}
		if (base.name != "GBC(Clone)" && RB.velocity.magnitude > 0.1f) {
			GetComponentInChildren<AudioSource>().volume = Mathf.Clamp01(RB.velocity.magnitude / 20f);
			if (!GetComponentInChildren<AudioSource>().isPlaying) {
				GetComponentInChildren<AudioSource>().Play();
			}
			GetComponentInChildren<ParticleSystem>().Play();
		}
		OnGround = true;
		PublicOnGround = true;
	}

	private void OnCollisionEnter(Collision Hit) {
		if ((bool)Hit.gameObject.GetComponentInChildren<Renderer>()) {
			if (Hit.gameObject.GetComponentInChildren<Renderer>().materials[0].name == "Planet (Instance)" || Hit.gameObject.GetComponentInChildren<Renderer>().materials[0].name == "Tree (Instance)") {
				if (LandSource.clip != LandSounds[0]) {
					LandSource.clip = LandSounds[0];
				}
			} else if (Hit.gameObject.GetComponentInChildren<Renderer>().materials[0].name == "Metal (Instance)" || Hit.gameObject.GetComponentInChildren<Renderer>().materials[0].name == "Rock (Instance)") {
				if (LandSource.clip != LandSounds[1]) {
					LandSource.clip = LandSounds[1];
				}
			} else if (LandSource.clip != LandSounds[2]) {
				LandSource.clip = LandSounds[2];
			}
		}
		LandSource.Play();
		ShakeCamera(0.2f, RB.velocity.magnitude / 20f);
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
		PlayerPrefs.SetInt("NumOBTemp", PlayerPrefs.GetInt("NumOBTemp", 0)+1);
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
					SnakeBossAI.PlayerHealth--;
				}
				CanDamage = false;
			} else {
				CanDamage = true;
			}
			LastPlace = new Vector3(0f, 0.75f, 0f);
		}
		base.transform.position = LastPlace;
		GameObject.FindObjectOfType<CameraControl>().VertAng = 10f;
	}
}