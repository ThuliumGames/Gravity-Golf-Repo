using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Collectable : MonoBehaviour
{
	public static int ItemsCollected;

	public bool isDestructable;

	public bool AcDest;

	public GameObject ObjToEnable;

	public int MaxItems;

	public GameObject Keys;

	public Text text;

	private void Start()
	{
		ItemsCollected = 0;
	}

	private void Update()
	{
		if (!isDestructable)
		{
			return;
		}
		Keys.SetActive(value: true);
		text.text = ItemsCollected + " of " + MaxItems;
		if (ItemsCollected >= MaxItems)
		{
			if (AcDest)
			{
				base.gameObject.SetActive(value: false);
			}
			else
			{
				GetComponent<Collider>().enabled = false;
				GetComponent<MeshRenderer>().enabled = false;
			}
			if (ObjToEnable != null)
			{
				ObjToEnable.SetActive(value: true);
			}
		}
	}

	private void OnTriggerEnter(Collider C)
	{
		if (!isDestructable && C.gameObject.name == "Golf Ball")
		{
			ItemsCollected++;
			GameObject.Find("Cage").GetComponent<AudioSource>().Play();
			Object.Destroy(base.gameObject);
		}
	}
}