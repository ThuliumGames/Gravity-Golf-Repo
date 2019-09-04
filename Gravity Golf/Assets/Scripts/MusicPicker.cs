using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicPicker : MonoBehaviour {
	
	AudioSource AS;
	public AudioClip clip;
	public Animator Anim;
	public Image Timeline;
	public Text SongName;
	public Text TotalTime;
	public Text CurrentTime;
	public Text Speed;
	
	public static bool Paused;
	bool canSwitch;
	bool canSpeed;
	bool canSlow;
	public static float pitch = 1;
	
	public KeyCode KC;
	
	void Start () {
		AS = GameObject.FindObjectOfType<AudioSource>();
		Paused = false;
		pitch = 1;
	}
	
	void Update () {
		
		Speed.text = pitch + "x";
		
		if (AS.clip == clip && AS.isPlaying) {
			
			if (Paused) {
				AS.pitch = AS.pitch = Mathf.Lerp (AS.pitch, 0, Time.deltaTime*10);
			}
			
			SongName.text = GetComponentInChildren<Text>().text;
			if ((int)(AS.clip.length - (((int)(AS.clip.length/60)) * 60)) <  10) {
				TotalTime.text = "0" + (int)(AS.clip.length/60) + ":0" + (int)(AS.clip.length - (((int)(AS.clip.length/60)) * 60));
			} else {
				TotalTime.text = "0" + (int)(AS.clip.length/60) + ":" + (int)(AS.clip.length - (((int)(AS.clip.length/60)) * 60));
			}
			if ((int)(AS.time - (((int)(AS.time/60)) * 60)) <  10) {
				CurrentTime.text = "0" + (int)(AS.time/60) + ":0" + (int)(AS.time - (((int)(AS.time/60)) * 60));
			} else {
				CurrentTime.text = "0" + (int)(AS.time/60) + ":" + (int)(AS.time - (((int)(AS.time/60)) * 60));
			}
			Timeline.rectTransform.sizeDelta = new Vector2 ((AS.time / AS.clip.length) * 1000f, 10);
		} else if (AS.clip == clip) {
			pitch = 1;
			TotalTime.text = "00:00";
			CurrentTime.text = "00:00";
			CurrentTime.text = "00:00";
			Timeline.rectTransform.sizeDelta = new Vector2 (0, 10);
			SongName.text = "";
		}
		
		if (Input.GetButtonDown("Fire2")) {
			pitch = 1;
			TotalTime.text = "00:00";
			CurrentTime.text = "00:00";
			CurrentTime.text = "00:00";
			Timeline.rectTransform.sizeDelta = new Vector2 (0, 10);
			SongName.text = "";
			AS.Stop();
		}
			
		if (GetComponent<Buttons>().wasAc) {
			
			if (Input.GetAxis("Fire7") > 0.1f) {
				if (canSwitch) {
					Paused = !Paused;
				}
				canSwitch = false;
			} else {
				canSwitch = true;
			}
			
			if (Input.GetAxis("Fire8") > 0.1f) {
				if (canSpeed) {
					canSpeed = false;
					pitch += pitch;
					if (pitch < 0) {
						pitch = 1;
					}
					if (pitch > 20) {
						pitch = 1;
					}
				}
			} else {
				canSpeed = true;
			}
			if (Input.GetAxis("Fire8") < -0.1f) {
				if (canSlow) {
					canSlow = false;
					pitch += pitch;
					if (pitch == 2) {
						pitch = -1;
					}
					if (pitch > 1) {
						pitch = 1;
					}
					if (pitch < -20) {
						pitch = -1;
					}
				}
			} else {
				canSlow = true;
			}
			if (!Paused) {
				AS.pitch = Mathf.Lerp (AS.pitch, pitch, Time.deltaTime*10);
			}
			if (AS.clip == clip && AS.isPlaying) {
				Anim.Play("PlayingMusic");
			} else {
				Anim.Play("DontPlayMusic");
			}
		}
	}
	
	public void TaskOnClick () {
		pitch = 1;
		AS.pitch = 1;
		Paused = false;
		AS.clip = clip;
		AS.Play();
	}
}
