using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DoSomethingWhenEnabled : MonoBehaviour
{
	private void OnEnabled()
	{
		GetComponent<Animator>().Play("ModeSelect");
	}
}
