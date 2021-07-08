using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomMusic : MonoBehaviour {
	
	public AudioClip[] ACs;
	
	AudioSource AS;
	
	public static int R;
	
	void Start () {
		
		transform.SetParent(null);
		
		AS = GetComponent<AudioSource>();
		
		if (name == "Music") {
			if (GameObject.Find("GabGolab")) {
				Destroy(gameObject);
			} else {
				DontDestroyOnLoad(gameObject);
				name = "GabGolab";
			}
		}
		
		R = Random.Range(0, 120*60);
	}
	
	void Update () {
		if (SceneManager.GetActiveScene().name == "Title Screen") {
			Destroy(gameObject);
		}
			
		if (!Global.touchingHole) {
			AS.volume = Mathf.Lerp(AS.volume, 0.5f, 0.01666f);
			if (R == 0) {
				AS.volume = 0;
				AS.clip = ACs[Random.Range(0, ACs.Length)];
				AS.Play();
				R = Random.Range(30*60, 120*60);
			}
			
			if (!AS.isPlaying) {
				R--;
			}
		} else {
			AS.volume = Mathf.Lerp(AS.volume, 0, 0.01666f);
		}
	}
}
