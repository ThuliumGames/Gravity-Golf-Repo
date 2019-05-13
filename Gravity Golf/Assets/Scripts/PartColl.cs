using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PartColl : MonoBehaviour {
	private void OnParticleCollision(GameObject Hit) {
		if (Hit.name == "Golf Ball") {
			GameObject.Find("Golf Ball").GetComponent<GolfHit>().Die();
		}
	}
}
