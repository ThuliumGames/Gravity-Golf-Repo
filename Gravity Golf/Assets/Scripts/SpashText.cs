using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpashText : MonoBehaviour {
	
	public string[] Texts;
	public static int Num = -1;
	public int Num2 = 0;
	
	public string[] TechnologicTexts;
	
	public AudioSource Drum;
	public AudioSource Dub;
	
	bool doDrum;
	bool doDub;
	
	float t = 0;
	
	void Update () {
		
		t += Time.deltaTime;
		
		if (Num == -1) {
			Num = Random.Range(0, Texts.Length);
		}
		
		if (Num == 22) {
			if (t >= ((float)((float)1/((float)127/(float)60))*(float)8)) {
				t = 0;
				Num2++;
				Num2 = Num2%TechnologicTexts.Length;
				if (Num2 == 4) {
					doDrum = true;
				}
				if (Num2 == 0) {
					doDub = true;
				}
				if (doDrum) {
					Drum.Play();
				}
				if (doDub) {
					Dub.Play();
				}
			}
			
			GetComponent<Text>().text = TechnologicTexts[Num2];
		} else {
			GetComponent<Text>().text = Texts[Num];
		}
	}
}
