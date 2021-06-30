using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayWhenDone : MonoBehaviour {
	
	public AudioSource auso;
	
	void Update () {
		if (!auso.isPlaying || !auso.enabled) {
			if (!GetComponent<AudioSource>().isPlaying) {
				GetComponent<AudioSource>().Play();
			}
		}
	}
}
