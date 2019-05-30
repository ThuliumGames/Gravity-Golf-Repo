using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Orbs : MonoBehaviour {
	private Boss1_AI AI;

	public bool AttackHit;

	public bool BeenHit;

	void Start() {
		AI = GameObject.FindObjectOfType<Boss1_AI>();
	}

	void OnTriggerEnter(Collider Other) {
		if (Other.gameObject.layer == LayerMask.NameToLayer("Ball") && !BeenHit) {
			AI.BossHealth--;
			if (AttackHit) {
				AI.Hit = true;
			} else {
				AI.AngryMode = true;
			}
			Other.GetComponent<Rigidbody>().AddExplosionForce(5000f, base.transform.position, 5000f);
			BeenHit = true;
		}
	}
}