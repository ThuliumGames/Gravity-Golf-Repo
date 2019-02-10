using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GrassPlace : MonoBehaviour
{
	public GameObject Grass;

	public Transform[] NoPlace;

	public float NoPlDis = 1f;

	public int Density = 100;

	public float Min = 0.01f;

	public float Max = 0.05f;

	private void Start()
	{
		for (int i = 0; i < Density; i++)
		{
			GameObject grass = Grass;
			Vector3 position = base.transform.position;
			Vector3 onUnitSphere = Random.onUnitSphere;
			Vector3 localScale = base.transform.localScale;
			GameObject gameObject = Object.Instantiate(grass, position + onUnitSphere * localScale.x, new Quaternion(0f, 0f, 0f, 0f));
			gameObject.transform.SetParent(base.transform);
			gameObject.transform.LookAt(base.transform.position);
			gameObject.transform.Rotate(180f, 0f, 0f);
			float num = Random.Range(Min, Max);
			Transform transform = gameObject.transform;
			float num2 = num;
			Vector3 localScale2 = base.transform.localScale;
			float x = num2 / localScale2.x;
			float num3 = num;
			Vector3 localScale3 = base.transform.localScale;
			float y = num3 / localScale3.x;
			float num4 = num;
			Vector3 localScale4 = base.transform.localScale;
			transform.localScale = new Vector3(x, y, num4 / localScale4.x);
			if (NoPlace.Length == 0)
			{
				continue;
			}
			Transform[] noPlace = NoPlace;
			foreach (Transform transform2 in noPlace)
			{
				if (Vector3.Distance(gameObject.transform.position, transform2.position) < NoPlDis)
				{
					Object.Destroy(gameObject);
					i--;
				}
			}
		}
	}
}
