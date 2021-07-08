using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour {
	
	public float tm = 1;
	
	void Start () {
		Invoke("Die", tm);
	}
	
	void Die () {
		Destroy(gameObject);
	}
}
