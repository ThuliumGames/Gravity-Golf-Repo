using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class WaterWaveEffect : MonoBehaviour {
	
	PostProcessVolume PPP;
	float GoPoint;
	float GoSpeed;
	void Start () {
		PPP = GetComponent<PostProcessVolume>();
	}
	
	void Update () {
		LensDistortion LD;
		PPP.profile.TryGetSettings(out LD);
		if (Mathf.Abs (LD.intensity.value - GoPoint) < 1) {
			if (GoPoint != 60) {
				GoPoint = 60;
			} else {
				GoPoint = 70;
			}
		}
		GoSpeed += Random.Range (-5f*Time.deltaTime, 5f*Time.deltaTime);
		GoSpeed = Mathf.Clamp (GoSpeed, -0.05f, 0.05f);
		LD.centerX.value = GoSpeed;
		LD.intensity.value = Mathf.Lerp (LD.intensity.value, GoPoint, 0.1f*Time.deltaTime);
	}
}
