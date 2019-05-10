using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MoveWithFill : MonoBehaviour {
	
	public bool WithFill;
	
	private void Update() {
		if (WithFill) {
			GetComponent<Animator>().SetFloat("Blend", GetComponent<Image>().fillAmount);
		} else {
			if (GameObject.Find("Golf Ball") != null) {
				GetComponent<Animator>().SetFloat("Blend", Vector3.Distance(GameObject.Find("Golf Ball").transform.position, transform.position)/50);
			}
		}
	}
}