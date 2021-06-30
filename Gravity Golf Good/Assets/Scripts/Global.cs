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
	
	public AudioSource holePlop;
	public AudioSource winSound;
	
	public GameObject GameUI;
	public GameObject Controls;
	public Image ControlsTogg;
	public Sprite[] cToSps;
	public GameObject[] defSel;
	public GameObject PauseUI;
	public GameObject WinUI;
	
	Volume volume;
	DepthOfField DOF;
	
	void Start () {
		
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
		if (SceneManager.GetActiveScene().name != "Title Screen") {
			if (touchingHole) {
				if (!holePlop.isPlaying && !winSound.isPlaying) {
					holePlop.volume = 0;
					holePlop.Play();
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
						Cursor.visible = false;
						Cursor.lockState = CursorLockMode.Locked;
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
			
			Controls.SetActive(showControls);
			
		} else {
			Time.timeScale = 1;
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}
	}
	
	void StartSong () {
		if (touchingHole) {
			winSound.Play();
			inHole = true;
		}
	}
	
	void DoPlay () {
		Paused = false;
	}
	
	void DoNext () {
		
	}
	
	void DoControl () {
		showControls = !showControls;
	}
	
	void DoQuit () {
		SceneManager.LoadScene("Title Screen");
	}
}
