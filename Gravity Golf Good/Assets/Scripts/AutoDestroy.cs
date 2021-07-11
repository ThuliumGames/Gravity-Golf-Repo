using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour {
	
	public float tm = 1;
	public bool animated;
	Animator anim;
	void Start () {
		if (GetComponent<Animator>()) {
			anim = GetComponent<Animator>();
		}
		if (!animated) {
			Invoke("Die", tm);
		} else {
			anim.enabled = false;
			Invoke("Animate", Random.Range(0.00f, tm));
		}
	}
	
	void Die () {
		Destroy(gameObject);
	}
	
	void Animate () {
		anim.enabled = true;
	}
}
