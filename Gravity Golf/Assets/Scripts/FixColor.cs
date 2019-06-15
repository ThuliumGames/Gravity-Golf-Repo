using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixColor : MonoBehaviour {
	
	Color C;
	
	void Start () {
		C = GetComponent<MeshRenderer>().materials[0].GetColor ("_BaseColor");
	}
	
	void Update () {
		GetComponent<MeshRenderer>().material.SetFloat("_Mode", 0f);
		GetComponent<MeshRenderer>().material.SetInt("_SrcBlend", 5);
		GetComponent<MeshRenderer>().material.SetInt("_DstBlend", 10);
		GetComponent<MeshRenderer>().material.SetInt("_ZWrite", 1);
		GetComponent<MeshRenderer>().material.DisableKeyword("_ALPHATEST_ON");
		GetComponent<MeshRenderer>().material.DisableKeyword("_ALPHABLEND_ON");
		GetComponent<MeshRenderer>().material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
		GetComponent<MeshRenderer>().material.renderQueue = 2000;
		GetComponent<MeshRenderer>().materials[0].SetColor ("_BaseColor", C);
	}
}
