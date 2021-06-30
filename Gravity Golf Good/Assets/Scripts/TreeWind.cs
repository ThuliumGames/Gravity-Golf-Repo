using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeWind : MonoBehaviour {
	
	public MeshRenderer mr;
	
	void Update () {
		mr.materials[1].SetVector("Vector2_FA3338B1", new Vector4 (Mathf.PerlinNoise(transform.position.x+(float)(Time.frameCount/50.0f), transform.position.z+(float)(Time.frameCount/50.0f))/10.0f, 0, 0, 0));
	}
}
