using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimator : MonoBehaviour {
	
	public Sprite[] I;
	public float SwitchTime;
	int i;
	float T;
	
	public bool FillWPar;
	
	void Update () {
		T += Time.deltaTime;
		if (T > SwitchTime) {
			T = 0;
			i++;
			if (i >= I.Length) {
				i = 0;
			}
		}
		GetComponent<Image>().sprite = I[i];
		
		if (FillWPar) {
			GetComponent<Image>().fillAmount = (GetComponentInParent<MoveWithFill>().GetComponent<Image>().fillAmount-0.6f)*2.5f;
		}
	}
}
