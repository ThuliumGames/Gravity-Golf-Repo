using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RemoveText : MonoBehaviour
{
	private bool Fade;

	private void Update()
	{
		if (GameObject.Find("GolfBall").GetComponent<GolfHit>().Strokes > 0)
		{
			Fade = true;
		}
		if (Fade)
		{
			Text component = GetComponent<Text>();
			Color color = GetComponent<Text>().color;
			component.color = new Vector4(1f, 1f, 1f, Mathf.Lerp(color.a, 0f, 0.05f));
		}
		Color color2 = GetComponent<Text>().color;
		if (color2.a < 0.01f)
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
