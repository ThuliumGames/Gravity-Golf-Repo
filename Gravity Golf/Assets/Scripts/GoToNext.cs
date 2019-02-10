using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToNext : MonoBehaviour
{
	private float T;

	public float length;

	public string LevelName;

	public bool Buttonable;

	private void Update()
	{
		T += Time.deltaTime;
		if (T >= length || (Buttonable && Input.GetButtonDown("Fire1")))
		{
			SceneManager.LoadScene(LevelName);
		}
	}
}