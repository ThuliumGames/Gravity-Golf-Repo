using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HitExplode : MonoBehaviour
{
	private void OnTriggerStay(Collider Other)
	{
		if (!(Other.tag == "Player"))
		{
		}
	}
}