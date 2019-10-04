using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateBall : MonoBehaviour
{
	public Camera Cam;

	public GameObject OTI;

	private void Update()
	{
		int num = 0;
		Transform[] array = Object.FindObjectsOfType<Transform>();
		foreach (Transform transform in array)
		{
			if (transform.gameObject.name == "Ball(Clone)")
			{
				num++;
			}
		}
		if (Input.GetButtonDown("Fire1"))
		{
			Camera cam = Cam;
			Vector3 mousePosition = Input.mousePosition;
			float x = mousePosition.x;
			Vector3 mousePosition2 = Input.mousePosition;
			RaycastHit hitInfo;
			Physics.Raycast(cam.ScreenPointToRay(new Vector3(x, mousePosition2.y, 0f)), out hitInfo, float.PositiveInfinity);
			if (hitInfo.collider.gameObject.GetComponent<Buttons>() == null)
			{
				Object.Instantiate(OTI, hitInfo.point, cam.transform.rotation);
			}
		}
	}
}