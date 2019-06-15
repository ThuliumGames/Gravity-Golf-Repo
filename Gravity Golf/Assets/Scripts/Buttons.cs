using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
	public Camera Cam;

	public string LevelName;
	
	public KeyCode KeyToPress;
	public KeyCode KeyNotToPress;

	public GameObject ObjToEnable;
	public GameObject ObjToDisable;

	public Shadow[] Ss;

	public bool ColorChanger;

	public Color DefColor = new Color(0.5f, 0.5f, 0.5f);

	public Color HighColor = new Color(1f, 1f, 1f);

	private bool ButtonPressed;
	
	public bool isAc;
	public static bool CanMove;
	public static float ReMove;
	public Buttons U;
	public Buttons D;
	public Buttons L;
	public Buttons R;
	public Buttons A;
	public Buttons DA;
	public Image I;
	public bool AlwaysDisable;
	public static bool SomethingPressed;
	public static int FramePressed;
	
	void Start () {
		Cam = Camera.main;
	}
	
	private void Update() {
		if (Time.frameCount != FramePressed) {
			SomethingPressed = false;
		}
		
		if (new Vector2 (Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal")).magnitude <= 0.25f || ReMove > 0.375f) {
			if (ReMove <= 0.25f) {
				ReMove = 0f;
			} else {
				ReMove = 0.125f;
			}
			CanMove = true;
		}
		
		if (new Vector2 (Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal")).magnitude > 0.25f) {
			ReMove += Time.deltaTime/GameObject.FindObjectsOfType<Buttons>().Length;
		}
		
		if (isAc) {
			if (CanMove) {
				if (Input.GetAxisRaw("Vertical") > 0.5f) {
					if (U != null) {
						U.isAc = true;
						isAc = false;
					}
					CanMove = false;
				}
				if (Input.GetAxisRaw("Vertical") < -0.5f) {
					if (D != null) {
						D.isAc = true;
						isAc = false;
					}
					CanMove = false;
				}
				
				if (Input.GetAxisRaw("Horizontal") > 0.5f) {
					if (R != null) {
						R.isAc = true;
						isAc = false;
					}
					CanMove = false;
				}
				if (Input.GetAxisRaw("Horizontal") < -0.5f) {
					if (L != null) {
						L.isAc = true;
						isAc = false;
					}
					CanMove = false;
				}
			}
			I.transform.position = Vector3.Lerp (I.transform.position, transform.position + new Vector3 (-2+(Mathf.Sin(Time.frameCount/8)/2), 0, 0), 5*Time.deltaTime);
		}
		
		if (KeyNotToPress != null) {
			if (Input.GetKeyDown(KeyNotToPress) && isAc) {
				DA.isAc = true;
				isAc = false;
				if (ObjToDisable != null) {
					ObjToDisable.SetActive (false);
				}
			}
		}
		
		Camera cam = Cam;
		Vector3 mousePosition = Input.mousePosition;
		float x = mousePosition.x;
		Vector3 mousePosition2 = Input.mousePosition;
		RaycastHit hitInfo;
		bool TurnOff = true;
		if (Physics.Raycast(cam.ScreenPointToRay(new Vector3(x, mousePosition2.y, 0f)), out hitInfo, float.PositiveInfinity) || ButtonPressed || (KeyToPress != null && isAc)) {
			if (KeyToPress != null || hitInfo.collider.gameObject == base.gameObject || ButtonPressed) {
				if (hitInfo.collider != null) {
					if (hitInfo.collider.gameObject == base.gameObject) {
						TurnOff = false;
						if (Input.GetMouseButtonDown(0)) {
							SomethingPressed = true;
							FramePressed = Time.frameCount;
						}
						if (ColorChanger) {
							Text[] componentsInChildren = GetComponentsInChildren<Text>();
							foreach (Text text in componentsInChildren) {
								GetComponent<Text>().color = HighColor;
								text.color = HighColor;
							}
							Shadow[] ss = Ss;
							foreach (Shadow shadow in ss) {
								GetComponent<Shadow>().enabled = true;
								shadow.enabled = true;
							}
						}
					}
				}
				if ((Input.GetKeyDown(KeyToPress) && isAc) || ButtonPressed || (Input.GetButtonDown("Fire1") && hitInfo.collider.gameObject == base.gameObject)) {
					
					ButtonPressed = true;
					
					if (LevelName == "Quit") {
						
						Application.Quit();
						
					} else if (LevelName == "Ena") {
						
						if (GameObject.Find(ObjToEnable.name) == null) {
							ObjToEnable.SetActive(value: true);
						} else {
							ObjToEnable.SetActive(value: false);
						}
						
					} else if (GetComponent<Text>()) {
						if (GetComponent<Text>().text == "Check For Updates") {
							Application.OpenURL ("https://gamejolt.com/games/GravityGolfBeta/416803");
						} else {
							if (LevelName.Contains("Level")) {
								if (LevelName.Contains("1") && !LevelName.Contains("2-1") && !LevelName.Contains("3-1")) {
									SceneManager.LoadScene("Scenes/World 1/" + LevelName);
								} else if (LevelName.Contains("2") && !LevelName.Contains("1-2") && !LevelName.Contains("3-2")) {
									SceneManager.LoadScene("Scenes/World 2/" + LevelName);
								} else if (LevelName.Contains("1") && !LevelName.Contains("1-3") && !LevelName.Contains("2-3")) {
									SceneManager.LoadScene("Scenes/World 3/" + LevelName);
								}
							} else {
								SceneManager.LoadScene(LevelName);
							}
						}
					} else {
						if (LevelName.Contains("Level")) {
							if (LevelName.Contains("1") && !LevelName.Contains("2-1") && !LevelName.Contains("3-1")) {
								SceneManager.LoadScene("Scenes/World 1/" + LevelName);
							} else if (LevelName.Contains("2") && !LevelName.Contains("1-2") && !LevelName.Contains("3-2")) {
								SceneManager.LoadScene("Scenes/World 2/" + LevelName);
							} else if (LevelName.Contains("1") && !LevelName.Contains("1-3") && !LevelName.Contains("2-3")) {
								SceneManager.LoadScene("Scenes/World 3/" + LevelName);
							}
						} else {
							SceneManager.LoadScene(LevelName);
						}
					}
					
					if (A != null) {
						A.isAc = true;
						isAc = false;
					}
					
				}
			} else if (ColorChanger) {
				Text[] componentsInChildren2 = GetComponentsInChildren<Text>();
				foreach (Text text2 in componentsInChildren2)
				{
					GetComponentInChildren<Text>().color = DefColor;
					text2.color = DefColor;
				}
				Shadow[] ss2 = Ss;
				foreach (Shadow shadow2 in ss2)
				{
					GetComponent<Shadow>().enabled = false;
					shadow2.enabled = false;
				}
			}
		}
		if (ColorChanger && TurnOff) {
			Text[] componentsInChildren3 = GetComponentsInChildren<Text>();
			foreach (Text text3 in componentsInChildren3) {
				GetComponent<Text>().color = DefColor;
				text3.color = DefColor;
			}
			Shadow[] ss3 = Ss;
			foreach (Shadow shadow3 in ss3) {
				GetComponent<Shadow>().enabled = false;
				shadow3.enabled = false;
			}
		}
	}
	
	void LateUpdate () {
		if (ObjToEnable != null) {
			if ((!SomethingPressed || AlwaysDisable) && !ButtonPressed) {
				if (Input.GetMouseButtonDown(0)) {
					ObjToEnable.SetActive(false);
				}
			}
		}
		ButtonPressed = false;
	}
}