using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
	public Camera Cam;

	public string LevelName;
	
	public KeyCode KeyToPress;

	public GameObject ObjToEnable;

	public Shadow[] Ss;

	public bool ColorChanger;

	public Color DefColor = new Color(0.5f, 0.5f, 0.5f);

	public Color HighColor = new Color(1f, 1f, 1f);

	private bool ButtonPressed;
	
	void Start () {
		Cam = Camera.main;
	}
	
	private void Update()
	{
		Camera cam = Cam;
		Vector3 mousePosition = Input.mousePosition;
		float x = mousePosition.x;
		Vector3 mousePosition2 = Input.mousePosition;
		RaycastHit hitInfo;
		if (Physics.Raycast(cam.ScreenPointToRay(new Vector3(x, mousePosition2.y, 0f)), out hitInfo, float.PositiveInfinity) || ButtonPressed || KeyToPress != null)
		{
			if (hitInfo.collider.gameObject == base.gameObject || ButtonPressed || KeyToPress != null)
			{
				if (ColorChanger)
				{
					Text[] componentsInChildren = GetComponentsInChildren<Text>();
					foreach (Text text in componentsInChildren)
					{
						GetComponent<Text>().color = HighColor;
						text.color = HighColor;
					}
					Shadow[] ss = Ss;
					foreach (Shadow shadow in ss)
					{
						GetComponent<Shadow>().enabled = true;
						shadow.enabled = true;
					}
				}
				if (Input.GetButtonDown("Fire1") || ButtonPressed || Input.GetKeyDown(KeyToPress))
				{
					if (LevelName == "Quit")
					{
						Application.Quit();
					}
					else if (LevelName == "Ena")
					{
						if (GameObject.Find(ObjToEnable.name) == null)
						{
							ObjToEnable.SetActive(value: true);
						}
						else
						{
							ObjToEnable.SetActive(value: false);
						}
					}
					else
					{
						SceneManager.LoadScene(LevelName);
					}
				}
			}
			else if (ColorChanger)
			{
				Text[] componentsInChildren2 = GetComponentsInChildren<Text>();
				foreach (Text text2 in componentsInChildren2)
				{
					GetComponentInChildren<Text>().color = DefColor;
					text2.color = DefColor;
				}
				Shadow[] ss2 = Ss;
				foreach (Shadow shadow2 in ss2)
				{
					GetComponent<Shadow>().enabled = false;
					shadow2.enabled = false;
				}
			}
		}
		else if (ColorChanger)
		{
			Text[] componentsInChildren3 = GetComponentsInChildren<Text>();
			foreach (Text text3 in componentsInChildren3)
			{
				GetComponent<Text>().color = DefColor;
				text3.color = DefColor;
			}
			Shadow[] ss3 = Ss;
			foreach (Shadow shadow3 in ss3)
			{
				GetComponent<Shadow>().enabled = false;
				shadow3.enabled = false;
			}
		}
		ButtonPressed = false;
	}

	private void MEnter()
	{
		ButtonPressed = true;
	}
}