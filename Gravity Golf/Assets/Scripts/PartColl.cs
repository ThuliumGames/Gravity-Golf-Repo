using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PartColl : MonoBehaviour
{
	private void OnParticleCollision()
	{
		GameObject.Find("GolfBall").GetComponent<GolfHit>().Die();
	}
}
