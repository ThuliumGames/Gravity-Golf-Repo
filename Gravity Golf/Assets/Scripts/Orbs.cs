using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Orbs : MonoBehaviour
{
	private Boss1_AI AI;

	public bool AttackHit;

	public bool BeenHit;

	private void Start()
	{
		AI = GetComponentInParent<Boss1_AI>();
	}

	private void OnTriggerEnter(Collider Other)
	{
		if (Other.tag == "Player" && !BeenHit)
		{
			AI.BossHealth--;
			if (AttackHit)
			{
				AI.Hit = true;
			}
			else
			{
				AI.AngryMode = true;
			}
			Other.GetComponent<Rigidbody>().AddExplosionForce(1000f, base.transform.position, 1000f);
			BeenHit = true;
		}
	}
}