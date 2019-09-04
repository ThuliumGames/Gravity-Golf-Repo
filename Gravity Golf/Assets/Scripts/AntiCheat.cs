using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AntiCheat : MonoBehaviour {
	
	void Start () {
		if (!Application.isEditor) {
			if (GameObject.Find("Didn't Cheat")) {
				Destroy(this.gameObject);
			} else {
				if (SceneManager.GetActiveScene().name == "TG") {
					name = "Didn't Cheat";
					DontDestroyOnLoad(this.gameObject);
				} else {
					SceneManager.LoadScene("TG");
				}
			}
		}
	}
}
