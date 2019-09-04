using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Orbs : MonoBehaviour {
	
	public bool onlyKnock;
	public bool BeenHit;

	void Update () {
		if (BeenHit) {
			gameObject.SetActive(false);
		}
	}

	void OnTriggerEnter(Collider Other) {
		if (Other.gameObject.layer == LayerMask.NameToLayer("Ball") && !BeenHit) {
			if (!onlyKnock) {
				BeenHit = true;
				SendMessageUpwards("Hit");
			}
			Other.GetComponent<Rigidbody>().AddExplosionForce(5000f, base.transform.position, 5000f);
		}
	}
}