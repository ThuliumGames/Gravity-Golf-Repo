using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[ExecuteInEditMode]
public class RotateTo : MonoBehaviour
{
	private void Update()
	{
		if (Application.isPlaying)
		{
			return;
		}
		float num = 1000000f;
		Planet[] array = Object.FindObjectsOfType<Planet>();
		foreach (Planet planet in array)
		{
			if (Vector3.Distance(base.transform.position, planet.gameObject.transform.position) < num)
			{
				num = Vector3.Distance(base.transform.position, planet.gameObject.transform.position);
				base.transform.LookAt(planet.gameObject.transform.position);
				base.transform.Rotate(-90f, 0f, 0f);
			}
		}
	}
}
