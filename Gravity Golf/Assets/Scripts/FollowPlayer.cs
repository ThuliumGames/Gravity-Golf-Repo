using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FollowPlayer : MonoBehaviour
{
	public bool scaleandClamp;

	public Vector3 clamp;

	public bool ObjFind;

	public string Name;

	public Transform ObjToFollow;

	private void Update()
	{
		if (ObjFind)
		{
			ObjToFollow = GameObject.Find(Name).transform;
		}
		if (ObjToFollow != null)
		{
			base.transform.position = ObjToFollow.position;
		}
		if (!scaleandClamp)
		{
			return;
		}
		if (Vector3.Distance(base.transform.position, Camera.main.gameObject.GetComponentInParent<CameraControl>().transform.position) < 5f)
		{
			Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
			foreach (Renderer renderer in componentsInChildren)
			{
				renderer.enabled = false;
			}
			Collider[] componentsInChildren2 = GetComponentsInChildren<Collider>();
			foreach (Collider collider in componentsInChildren2)
			{
				collider.enabled = false;
			}
		}
		else
		{
			Renderer[] componentsInChildren3 = GetComponentsInChildren<Renderer>();
			foreach (Renderer renderer2 in componentsInChildren3)
			{
				renderer2.enabled = true;
			}
			Collider[] componentsInChildren4 = GetComponentsInChildren<Collider>();
			foreach (Collider collider2 in componentsInChildren4)
			{
				collider2.enabled = true;
			}
		}
	}
}