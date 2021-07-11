using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Tree : MonoBehaviour {
	
	public bool inactive = true;
	public bool lookNormals = true;
	
	public LayerMask lm;
	
	void Update () {
		if (!inactive) {
			if (!lookNormals) {
				if (transform.parent!= null) {
					transform.LookAt(transform.parent);
					transform.Rotate(-90, 0, 0);
				}
			} else {
				RaycastHit h;
				if (Physics.Raycast(transform.position, -transform.up, out h, 10, lm)) {
					transform.LookAt(transform.position+h.normal);
					transform.Rotate(90, 0, 0);
				}
			}
		}
	}
}
