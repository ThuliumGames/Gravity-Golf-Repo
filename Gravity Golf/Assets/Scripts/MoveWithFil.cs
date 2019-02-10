using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MoveWithFill : MonoBehaviour
{
	private void Update()
	{
		GetComponent<Animator>().SetFloat("Blend", GetComponent<Image>().fillAmount);
	}
}