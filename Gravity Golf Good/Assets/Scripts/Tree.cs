using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Tree : MonoBehaviour {
	
	void Update () {
		if (transform.parent!= null) {
			transform.LookAt(transform.parent);
			transform.Rotate(-90, 0, 0);
			RaycastHit h;
			if (Physics.Raycast(transform.position, -transform.up, out h, 10)) {
				transform.position = h.point;
			}
		}
	}
}
