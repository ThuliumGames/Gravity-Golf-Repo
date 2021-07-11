using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleCam : MonoBehaviour {
	
	public bool inTitle;
	bool shrink;
	
	public Transform[] levels;
	public Text level;
	public Transform titlePlanet;
	
	public Animator anim;
	
	int movingRFor;
	int movingLFor;
	
	void Update () {
		if (inTitle) {
			if (Input.GetButtonDown("Fire1")) {
				if (GameObject.Find("TitleCam").GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name == "Idle") {
					GameObject.Find("TitleCam").GetComponentInChildren<Canvas>().gameObject.SetActive(false);
					anim.Play("Play");
					Invoke ("Go", 0.5f);
					shrink = true;
				} else {
					GameObject.Find("TitleCam").GetComponent<Animator>().Play("Idle");
				}
			}
		} else {
			
			if (Input.GetButtonDown("Submit")) {
				DoPlay();
			}
			
			bool moving = false;
			if (Input.GetButton("Fire1") || Mathf.Abs(Input.GetAxis("LStick X")) > 0) {
				moving = true;
				movingRFor += Convert.ToInt32(-Input.GetAxis("Mouse X")+Input.GetAxis("LStick X") > 0);
				movingLFor += Convert.ToInt32(-Input.GetAxis("Mouse X")+Input.GetAxis("LStick X") < 0);
				transform.position += new Vector3 ((-(Input.GetAxis("Mouse X")-(Input.GetAxis("LStick X")*4f)))*Time.deltaTime*150, 0, 0);
				if (movingLFor > 0 && movingRFor > 0 && Mathf.Abs(Input.GetAxis("LStick X")) > 0) {
					transform.position += new Vector3 (75*((Convert.ToInt32(movingRFor>movingLFor)*2)-1), 0, 0);
					movingLFor = 0;
					movingRFor = 0;
				}
			} else {
				if (movingRFor < 15 && movingRFor > 0 && Mathf.Abs((Mathf.Clamp((Mathf.Floor((transform.position.x+75)/150.0f)*150), 0, ((levels.Length-1)*150))+76)-transform.position.x)<75) {
					transform.position = new Vector3 (Mathf.Clamp((Mathf.Floor((transform.position.x+75)/150.0f)*150), 0, ((levels.Length-1)*150))+76, 0, 0);
				}
				if (movingLFor < 15 && movingLFor > 0 && Mathf.Abs((Mathf.Clamp((Mathf.Floor((transform.position.x+75)/150.0f)*150), 0, ((levels.Length-1)*150))-76)-transform.position.x)<75) {
					transform.position = new Vector3 (Mathf.Clamp((Mathf.Floor((transform.position.x+75)/150.0f)*150), 0, ((levels.Length-1)*150))-76, 0, 0);
				}
				movingLFor = 0;
				movingRFor = 0;
			}
			float pos = Mathf.Clamp((Mathf.Floor((transform.position.x+75)/150.0f)*150), 0, ((levels.Length-1)*150));
			transform.position = new Vector3 (Mathf.Lerp(transform.position.x, pos, 10*(Convert.ToInt32(!moving)+Convert.ToInt32(transform.position.x<0)+Convert.ToInt32(transform.position.x>((levels.Length-1)*150)))*Time.deltaTime), 0, 0);
			for(int i = 0; i < levels.Length; i++) {
				levels[i].localScale = Vector3.Lerp(levels[i].localScale, (Vector3.one*0.8f*Convert.ToInt32(pos==(i*150)))+(Vector3.one*0.2f), 10*Time.deltaTime);
			}
			level.text = "Hole #" + ((pos/150.0f)+1);
		}
		if (shrink) {
			titlePlanet.localScale = Vector3.Lerp(titlePlanet.localScale, Vector3.zero, 10*Time.deltaTime);
			if (inTitle) {
				levels[0].localScale = Vector3.Lerp(levels[0].localScale, Vector3.one, 10*Time.deltaTime);
			}
		}
	}
	
	void Go () {
		GameObject.Find("TitleCam").SetActive(false);
		GetComponentInChildren<Canvas>().targetDisplay = 0;
		anim.enabled = false;
		inTitle = false;
	}
	
	void DoPlay () {
		SceneManager.LoadScene("Level " + (Mathf.Clamp((Mathf.Floor((transform.position.x+75)/150.0f)*150), 0, ((levels.Length-1)*150))/150.0f));
	}
}