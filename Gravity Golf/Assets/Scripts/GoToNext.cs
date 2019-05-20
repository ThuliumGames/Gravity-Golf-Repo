using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToNext : MonoBehaviour {

	public Animator Anim;
	public string AName;

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Return)) {
			Anim.Play (AName);
		}
	}
}