using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour {
	public float Speed;
	public float OpeningSpace;

	void Start() {
		foreach (Transform T in GetComponentInChildren<Transform>()) {
			if (T.transform != transform) {
				T.transform.position += -T.transform.up * OpeningSpace;
			}
		}
	}
	

	void Update () {
		transform.Rotate(0, 0, Speed , Space.World);
	}
}
