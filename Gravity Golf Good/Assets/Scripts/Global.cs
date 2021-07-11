using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class Global : MonoBehaviour {
	
	public static bool Paused;
	public static bool inHole;
	public static bool touchingHole;
	public static bool showControls = true;
	public static bool didAGood;
	
	public static int strokes;
	public static int score;
	public int Par;
	
	public AudioSource holePlop;
	public AudioSource winSound;
	
	public GameObject GameUI;
	public GameObject[] Controls;
	public Image ControlsTogg;
	public Sprite[] cToSps;
	public GameObject[] defSel;
	public GameObject PauseUI;
	public GameObject WinUI;
	public Image[] lockedGraph;
	
	public Text strokesText;
	public Text parText;
	public Text[] EndText;
	
	public static bool shotLocked;
	
	string[] terms = {
		"hole in one !",
		"albatross",
		"eagle",
		"birdie",
		"par",
		"bogey",
		"double-",
		"tripple-",
		"quadruple-",
		"quintuple-",
		"sextuple-",
		"septuple-",
		"octuple-",
		"nonuple-",
		"decuple-",
		"undecuple-",
		"duodecuple-",
		"tredecuple-",
		"quattuordecuple-",
		"quindecuple-",
		"sexdecuple-",
		"septendecuple-",
		"octodecuple-",
		"novemdecuple-",
		"like a billion-"
	};
	
	Volume volume;
	DepthOfField DOF;
	
	public static int inputMethod;
	public static int difficulty = 1;
	public static bool freeCam;
	
	void Start () {
		
		shotLocked = false;
		
		strokes = 0;
		score = 0;
		
		if (SceneManager.GetActiveScene().name == "Title Screen") {
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		} else {
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}
		
		Paused = false;
		inHole = false;
		touchingHole = false;
		
		volume = gameObject.GetComponentInChildren<Volume>();
		DepthOfField tmp;

		if(volume.profile.TryGet<DepthOfField>(out tmp)) {
			DOF = tmp;
		}
	}
	
	void Update () {
		
		if (Mathf.Abs(Input.GetAxis("Only Mouse")) > 0) {
			inputMethod = 0;
		} else if (Mathf.Abs(Input.GetAxis("Only Controller")) > 0) {
			inputMethod = 1;
		}
		
		if (SceneManager.GetActiveScene().name != "Title Screen") {
			
			foreach (Image im in lockedGraph) {
				im.color = new Color (200.0f/360.0f, 1, 1, Mathf.Lerp(im.color.a, 0.2f*System.Convert.ToInt32(shotLocked), Time.deltaTime*10));
			}
			
			if (Input.GetButtonDown("R")) {
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
			// Stroke Text
			parText.text = "<size=40>Par:</size>\n"+Par;
			strokesText.text = "<size=40>Strokes:</size>\n"+strokes;
			int p = strokes-Par;
			EndText[0].text = "";
			if (p >= 0) {
				EndText[0].text = "+";
			}
			EndText[0].text += ""+p+"\n<size=25>"+score+"pts</size>";
			if (strokes > 1) {
				EndText[1].text = "";
				if (Mathf.Clamp(p+4, 1, 5) != p+4) {
					EndText[1].text = terms[Mathf.Clamp(Mathf.Abs(Mathf.Clamp(p+4, 1, 5) - (p+4))+5, 6, 24)].ToUpper();
				}
				EndText[1].text += terms[Mathf.Clamp(p+4, 1, 5)].ToUpper();
			} else {
				EndText[1].text = "HOLE IN ONE !";
			}
			//
			
			if (touchingHole) {
				if (!holePlop.isPlaying && !winSound.isPlaying) {
					holePlop.volume = 0;
					holePlop.Play();
					Invoke ("StartAnim", 0.833f);
					Invoke ("StartSong", 2);
				}
				
				//Cursor
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
				//
				
				if(DOF != null) {
					DOF.focalLength.value = Mathf.Lerp(DOF.focalLength.value, 40, 0.1f);
				}
				
				//UIs
				if (inHole) {
					WinUI.SetActive(true);
					if (GameUI.activeSelf) {
						EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(defSel[0]);
						EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(defSel[1]);
					}
					GameUI.SetActive(false);
					PauseUI.SetActive(false);
				} else {
					GameUI.SetActive(!Paused);
					PauseUI.SetActive(Paused);
					WinUI.SetActive(false);
				}
				//
				
				Paused = false;
			} else {
				
				if (Input.GetButtonDown("Start")) {
					Paused = !Paused;
				}
				
				if (holePlop.isPlaying) {
					holePlop.Stop();
				}
				if(DOF != null) {
					if (Paused) {
						//Cursor
						Cursor.visible = true;
						Cursor.lockState = CursorLockMode.None;
						//
						DOF.focalLength.value = Mathf.Lerp(DOF.focalLength.value, 40, 0.1f);
					} else {
						//Cursor
						if (!freeCam) {
							Cursor.visible = false;
							Cursor.lockState = CursorLockMode.Locked;
						} else {
							Cursor.visible = true;
							Cursor.lockState = CursorLockMode.None;
						}
						//
						DOF.focalLength.value = Mathf.Lerp(DOF.focalLength.value, 20, 0.1f);
					}
				}
				
				//UIs
				PauseUI.SetActive(Paused);
				if (Paused && GameUI.activeSelf) {
					EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(defSel[1]);
					EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(defSel[0]);
				}
				GameUI.SetActive(!Paused);
				WinUI.SetActive(false);
				//
			}
			
			if (Paused) {
				Time.timeScale = 0;
				if (showControls) {
					ControlsTogg.sprite = cToSps[0];
				} else {
					ControlsTogg.sprite = cToSps[1];
				}
			} else {
				Time.timeScale = 1;
			}
			
			holePlop.volume = Mathf.Clamp(holePlop.volume+0.075f, 0, 1);
			
			if (showControls) {
				if (inputMethod == 0) {
					Controls[0].SetActive(true);
					Controls[1].SetActive(false);
				} else {
					Controls[1].SetActive(true);
					Controls[0].SetActive(false);
				}
			} else {
				foreach (GameObject G in Controls) {
				G.SetActive(false);
			}
			}
			
		} else {
			if (Input.GetButtonDown("Quit")) {
				Application.Quit();
			}
			
			Time.timeScale = 1;
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}
	}
	
	void StartSong () {
		if (touchingHole) {
			winSound.Play();
		}
	}
	
	void StartAnim () {
		if (touchingHole) {
			score += (int)Mathf.Clamp(((Par-strokes)*10)+50, 0, Mathf.Infinity);
			inHole = true;
		}
	}
	
	void DoPlay () {
		Paused = false;
	}
	
	void DoNext () {
		char[] C = SceneManager.GetActiveScene().name.ToCharArray();
		string N = "";
		for (int i = 6; i < C.Length; i++) {
			N += C[i];
		}
		int L = int.Parse(N)+1;
		SceneManager.LoadScene("Level " + L);
	}
	
	void DoControl () {
		showControls = !showControls;
	}
	
	void DoQuit () {
		SceneManager.LoadScene("Title Screen");
	}
}
