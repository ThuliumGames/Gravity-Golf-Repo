using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnBallContact : MonoBehaviour {
	
	public bool Timer;
	public float[] MinMax;
	void Start () {
		if (Timer) {
			Invoke("Go", Random.Range(MinMax[0], MinMax[1]));
		}
	}
	
	void OnTriggerEnter () {
		GetComponent<AudioSource>().Play();
	}
	
	void Go () {
		GetComponent<AudioSource>().Play();
	}
}
