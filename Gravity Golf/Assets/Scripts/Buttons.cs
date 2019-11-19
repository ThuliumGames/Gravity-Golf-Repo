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
	
	public string KeyToPress;
	public string KeyNotToPress;

	public GameObject ObjToEnable;
	public GameObject ObjToDisable;

	public Shadow[] Ss;

	public bool ColorChanger;

	public Color DefColor = new Color(0.5f, 0.5f, 0.5f);

	public Color HighColor = new Color(1f, 1f, 1f);
	
	public bool isAc;
	public bool wasAc;
	public static bool CanMove;
	public static float ReMove;
	public Buttons U;
	public Buttons D;
	public Buttons L;
	public Buttons R;
	public Buttons A;
	public Buttons DA;
	public Image I;
	
	public bool notPC;
	
	public bool PermanAc;
	
	public bool ArrowOnLeft;
	
	void Start () {
		Cam = Camera.main;
	}
	
	private void Update() {
		
		if (new Vector2 (Input.GetAxisRaw("Vertical1"), Input.GetAxisRaw("Horizontal1")).magnitude <= 0.25f || ReMove > 0.375f) {
			if (ReMove <= 0.25f) {
				ReMove = 0f;
			} else {
				ReMove = 0.125f;
			}
			CanMove = true;
		}
		
		if (notPC) {
			if (isAc && (U != null || D != null || L != null || R != null)) {
				
				if (new Vector2 (Input.GetAxisRaw("Vertical1"), Input.GetAxisRaw("Horizontal1")).magnitude > 0.25f) {
					ReMove += Time.deltaTime;
				}
				
				if (CanMove) {
					if (Input.GetAxisRaw("Vertical1") > 0.25f) {
						if (U != null) {
							U.isAc = true;
							isAc = false;
						}
						CanMove = false;
					} else if (Input.GetAxisRaw("Vertical1") < -0.25f) {
						if (D != null) {
							D.isAc = true;
							isAc = false;
						}
						CanMove = false;
					} else if (Input.GetAxisRaw("Horizontal1") > 0.25f) {
						if (R != null) {
							R.isAc = true;
							isAc = false;
						}
						CanMove = false;
					} else if (Input.GetAxisRaw("Horizontal1") < -0.25f) {
						if (L != null) {
							L.isAc = true;
							isAc = false;
						}
						CanMove = false;
					}
				}
				I.transform.position = Vector3.Lerp (I.transform.position, transform.position + new Vector3 (-2+(Mathf.Sin(Time.frameCount/8)/2), 0, 0), 5*Time.deltaTime);
			}
		} else {
			
			if (wasAc) {
				if (ArrowOnLeft) {
					I.transform.localPosition = Vector3.Lerp (I.transform.localPosition, transform.localPosition + new Vector3 ((-GetComponent<RectTransform>().sizeDelta.x/2), 0, 0) + new Vector3 ((Mathf.Sin(Time.frameCount/8)/2), 0, 0), 5*Time.deltaTime);
				} else {
					I.transform.position = Vector3.Lerp (I.transform.position, transform.position + new Vector3 ((Mathf.Sin(Time.frameCount/8)/2), 0, 0), 5*Time.deltaTime);
				}
			}
			
			if (!PermanAc) {
				if ((Input.mousePosition.x-(Screen.width/2) > transform.localPosition.x - (GetComponent<RectTransform>().sizeDelta.x/2) && Input.mousePosition.x-(Screen.width/2) < transform.localPosition.x + (GetComponent<RectTransform>().sizeDelta.x/2)) &&
					(Input.mousePosition.y-(Screen.height/2) > transform.localPosition.y - (GetComponent<RectTransform>().sizeDelta.y/2) && Input.mousePosition.y-(Screen.height/2) < transform.localPosition.y + (GetComponent<RectTransform>().sizeDelta.y/2))) {
						isAc = true;
						foreach (Buttons B in GameObject.FindObjectsOfType<Buttons>()) {
							B.wasAc = false;
						}
						wasAc = true;
				} else {
					isAc = false;
				}
			} else {
				isAc = true;
			}
		}
		
		if (KeyNotToPress != "") {
			if (Input.GetButtonDown(KeyNotToPress) && isAc) {
				DA.isAc = true;
				isAc = false;
				if (ObjToDisable != null) {
					ObjToDisable.SetActive (false);
				}
			}
		}
		
		if (isAc) {
			
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
				if (GetComponent<Outline>()) {
					GetComponent<Outline>().enabled = true;
				}
			}
			
			if (Input.GetButtonDown(KeyToPress)) {
				
				if (LevelName == "Quit") {
					
					Application.Quit();
					
				} else if (LevelName == "Ena") {
					ObjToEnable.SetActive(!ObjToEnable.activeSelf);
					
				} else if (LevelName == "LoadPre") {
					
					SceneManager.LoadScene(PlayerPrefs.GetString("SaveLevel", "Scenes/World 1/Level 1-1"));
					
				} else if (LevelName == "EndGame") {
					
					PlayerPrefs.SetString("SaveLevel", SceneManager.GetActiveScene().path);
					SceneManager.LoadScene("Intro");
				
				} else if (LevelName == "EndWorld") {
					
					PlayerPrefs.SetString("SaveLevel", "Scenes/World 1/Level 1-1");
					SceneManager.LoadScene("Intro");
				
				} else if (LevelName == "PlayMusic") {
					GetComponent<MusicPicker>().TaskOnClick();
				} else if (GetComponent<Text>()) {
					if (GetComponent<Text>().text == "Check For Updates") {
						Application.OpenURL ("https://gamejolt.com/games/GravityGolfBeta/416803");
					} else {
						if (LevelName.Contains("Level")) {
							
							if (SceneManager.GetActiveScene().name == "L Select") {
								PlayerPrefs.SetInt("ScoreTemp", 0);
								PlayerPrefs.SetInt("NumOBTemp", 0);
								PlayerPrefs.SetInt("NumResetTemp", 0);
							}
							
							if (LevelName.Contains("l 1")) {
								print (PlayerPrefs.GetString("SaveLevel", "Scenes/World 1/Level 1-1"));
								if (PlayerPrefs.GetString("SaveLevel", "Scenes/World 1/Level 1-1").Contains("d 1")) {
									SceneManager.LoadScene(PlayerPrefs.GetString("SaveLevel", "Scenes/World 1/Level 1-1"));
								} else {
									SceneManager.LoadScene("Scenes/World 1/" + LevelName);
								}
							} else if (LevelName.Contains("l 2")) {
								if (PlayerPrefs.GetString("SaveLevel", "Scenes/World 1/Level 1-1").Contains("d 2")) {
									SceneManager.LoadScene(PlayerPrefs.GetString("SaveLevel", "Scenes/World 1/Level 1-1"));
								} else {
									SceneManager.LoadScene("Scenes/World 2/" + LevelName);
								}
							} else if (LevelName.Contains("l 3")) {
								if (PlayerPrefs.GetString("SaveLevel", "Scenes/World 1/Level 1-1").Contains("d 3")) {
									SceneManager.LoadScene(PlayerPrefs.GetString("SaveLevel", "Scenes/World 1/Level 1-1"));
								} else {
									SceneManager.LoadScene("Scenes/World 3/" + LevelName);
								}
							} else if (LevelName.Contains("l 4")) {
								if (PlayerPrefs.GetString("SaveLevel", "Scenes/World 1/Level 1-1").Contains("d 4")) {
									SceneManager.LoadScene(PlayerPrefs.GetString("SaveLevel", "Scenes/World 1/Level 1-1"));
								} else {
									SceneManager.LoadScene("Scenes/World 4/" + LevelName);
								}
							} else if (LevelName.Contains("l 5")) {
								if (PlayerPrefs.GetString("SaveLevel", "Scenes/World 1/Level 1-1").Contains("d 5")) {
									SceneManager.LoadScene(PlayerPrefs.GetString("SaveLevel", "Scenes/World 1/Level 1-1"));
								} else {
									SceneManager.LoadScene("Scenes/World 51/" + LevelName);
								}
							}
						} else {
							SceneManager.LoadScene(LevelName);
						}
					}
				} else {
					if (LevelName.Contains("Level")) {
						if (LevelName.Contains("l 1")) {
							SceneManager.LoadScene("Scenes/World 1/" + LevelName);
						} else if (LevelName.Contains("l 2")) {
							SceneManager.LoadScene("Scenes/World 2/" + LevelName);
						} else if (LevelName.Contains("l 3")) {
							SceneManager.LoadScene("Scenes/World 3/" + LevelName);
						} else if (LevelName.Contains("l 4")) {
							SceneManager.LoadScene("Scenes/World 4/" + LevelName);
						} else if (LevelName.Contains("l 5")) {
							SceneManager.LoadScene("Scenes/World 51/" + LevelName);
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
			if (GetComponent<Outline>()) {
				GetComponent<Outline>().enabled = false;
			}
		}
	}
}