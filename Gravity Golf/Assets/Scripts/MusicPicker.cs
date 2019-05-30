using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicPicker : MonoBehaviour {
	
	AudioSource AS;
	public AudioClip clip;
	
	void Start () {
		AS = GameObject.FindObjectOfType<AudioSource>();
		GetComponent<Button>().onClick.AddListener(TaskOnClick);
	}
	
	void TaskOnClick () {
		AS.clip = clip;
		AS.Play();
	}
}
