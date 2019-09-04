using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinifSelect : MonoBehaviour {
	
	public Buttons B;
	float SpeedX = 90;
	float SpeedY = 0;
	float SpeedZ = 0;
	
	public bool isWaterP;
	public Renderer[] Rend;
	
	void Update () {
		if (B.isAc) {
			SpeedX += Random.Range(-5.01f, 5.01f);
			SpeedY += Random.Range(-5.01f, 5.01f);
			SpeedZ += Random.Range(-5.01f, 5.01f);
			SpeedX = Mathf.Clamp(SpeedX, -180, 180);
			SpeedY = Mathf.Clamp(SpeedY, -180, 180);
			SpeedZ = Mathf.Clamp(SpeedZ, -180, 180);
			transform.Rotate (new Vector3 (SpeedX, SpeedY, SpeedZ) * Time.deltaTime);
			
			if (isWaterP) {
				foreach (Renderer R in Rend) {
					R.materials[0].SetFloat("_AlphaCutoff", Mathf.Lerp(R.materials[0].GetFloat("_AlphaCutoff"), 0, 5*Time.deltaTime));
				}
			}
		} else {
			if (isWaterP) {
				foreach (Renderer R in Rend) {
					R.materials[0].SetFloat("_AlphaCutoff", Mathf.Lerp(R.materials[0].GetFloat("_AlphaCutoff"), 1, 10*Time.deltaTime));
				}
			}
		}
	}
}
