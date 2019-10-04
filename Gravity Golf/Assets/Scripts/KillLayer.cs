using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KillLayer : MonoBehaviour {
	
	public float MaxLiveTime;
	float t;
	
	public GameObject Ex;
	
	void Update () {
		if (MaxLiveTime > 0) {
			t += Time.deltaTime;
			if (t > MaxLiveTime) {
				Destroy(this.gameObject);
			}
		}
	}
	private void OnTriggerEnter(Collider Hit) {
		if (name != "Ball(Clone)") {
			if (Hit.gameObject.name == ("Golf Ball")) {
				Hit.gameObject.GetComponent<GolfHit>().Die();
			} else if (Hit.gameObject.name == ("Ball(Clone)")) {
				++GameObject.FindObjectOfType<BallShooter>().Score;
				Instantiate (Ex, transform.position, transform.rotation);
				Destroy (Hit.gameObject);
				Destroy (this.gameObject);
			}
		}
	}
}
