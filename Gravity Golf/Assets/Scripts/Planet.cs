using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Planet : MonoBehaviour
{
	private Rigidbody ObjToPull;

	public bool isDirectional;

	public Vector3 Direction;

	public float Range;

	public float pullForce;

	private void Update()
	{
		if (Application.isPlaying)
		{
			Rigidbody[] array = Object.FindObjectsOfType<Rigidbody>();
			foreach (Rigidbody rigidbody in array)
			{
				if (!(rigidbody != GetComponent<Rigidbody>()))
				{
					continue;
				}
				ObjToPull = rigidbody;
				float num = Vector3.Distance(ObjToPull.gameObject.transform.position, base.transform.position);
				float range = Range;
				Vector3 localScale = base.transform.localScale;
				if (!(num < range + localScale.x / 2f))
				{
					float num2 = Vector3.Distance(ObjToPull.gameObject.transform.position, base.transform.position);
					float num3 = Range * 20f;
					Vector3 localScale2 = base.transform.localScale;
					if (!(num2 < num3 + localScale2.x / 2f) || !(ObjToPull.tag == "Asteroid"))
					{
						float num4 = Vector3.Distance(ObjToPull.gameObject.transform.position, base.transform.position);
						float num5 = Range * 4f;
						Vector3 localScale3 = base.transform.localScale;
						if (num4 < num5 + localScale3.x / 2f && !isDirectional)
						{
							GameObject gameObject = new GameObject();
							gameObject.transform.position = base.transform.position;
							gameObject.transform.LookAt(ObjToPull.gameObject.transform.position);
							ObjToPull.AddForce(-gameObject.transform.forward * (pullForce / 2f) * Time.deltaTime);
							Object.Destroy(gameObject);
						}
						continue;
					}
				}
				GameObject gameObject2 = new GameObject();
				if (isDirectional)
				{
					gameObject2.transform.position = ObjToPull.gameObject.transform.position + Direction;
				}
				else
				{
					gameObject2.transform.position = base.transform.position;
				}
				gameObject2.transform.LookAt(ObjToPull.gameObject.transform.position);
				if (ObjToPull.gameObject.layer == LayerMask.NameToLayer("Boss"))
				{
					ObjToPull.AddForce(-gameObject2.transform.forward * (pullForce * 1E+07f) * Time.deltaTime);
				}
				else
				{
					ObjToPull.AddForce(-gameObject2.transform.forward * pullForce * Time.deltaTime);
				}
				Object.Destroy(gameObject2);
			}
		}
		else
		{
			Transform transform = base.transform;
			Vector3 localScale4 = base.transform.localScale;
			float x = localScale4.x;
			Vector3 localScale5 = base.transform.localScale;
			float x2 = localScale5.x;
			Vector3 localScale6 = base.transform.localScale;
			transform.localScale = new Vector3(x, x2, localScale6.x);
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Vector3 position = base.transform.position;
		float range = Range;
		Vector3 localScale = base.transform.localScale;
		Gizmos.DrawWireSphere(position, range + localScale.x / 2f);
		Gizmos.color = new Color(0.5f, 0.5f, 0f);
		Vector3 position2 = base.transform.position;
		float num = Range * 4f;
		Vector3 localScale2 = base.transform.localScale;
		Gizmos.DrawWireSphere(position2, num + localScale2.x / 2f);
		Gizmos.color = Color.green;
		Vector3 position3 = base.transform.position;
		float num2 = Range * 20f;
		Vector3 localScale3 = base.transform.localScale;
		Gizmos.DrawWireSphere(position3, num2 + localScale3.x / 2f);
	}
}
