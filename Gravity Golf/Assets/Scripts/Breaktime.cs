using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Breaktime : MonoBehaviour {
	
	public float Time2Wait;
	public float T;
	
	public GameObject Obj2Ena;
	
	void Start () {
		if (GameObject.Find("GlubiBoiJr")) {
			Destroy(this.gameObject);
		} else {
			name = "GlubiBoiJr";
			DontDestroyOnLoad (this.gameObject);
		}
	}
	
	void Update () {
		T += Time.deltaTime;
		if (T >= Time2Wait) {
			Obj2Ena.SetActive(true);
		}
	}
}
