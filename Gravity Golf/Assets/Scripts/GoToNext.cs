using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToNext : MonoBehaviour {

	public bool isSceneSwitcher;
	public Animator Anim;
	public float MaxTime;
	public string AName;

	public float T;
	
	public bool NoSkip;
	
	private void Update() {
		T += Time.deltaTime;
		
		if ((!NoSkip && (Input.GetButtonDown("Fire2") || Input.GetButtonDown("Fire1") || Input.GetButtonDown("Submit") || Input.GetButtonDown("Cancel"))) || (MaxTime != 0 && T >= MaxTime)) {
			if (isSceneSwitcher) {
				SceneManager.LoadScene(AName);
			} else {
				Anim.Play (AName);
			}
		}
	}
}