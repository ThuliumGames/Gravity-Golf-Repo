using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMusic : MonoBehaviour {
	
	public GameObject Go;
	
	void Update () {
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		if (!GetComponent<AudioSource>().isPlaying) {
			Go.SetActive(true);
			Destroy (this.gameObject);
		}
	}
}
