using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: AssemblyVersion("0.0.0.0")]
public class Asteroid : MonoBehaviour
{
	public bool Spawner;

	public float SpawnTime;

	private float RandomSpawn;

	public float Range;

	private float T;

	public float InitialSpeed;

	public GameObject Disable;

	public SphereCollider ChangeSize;

	private void Start()
	{
		if (!Spawner)
		{
			GetComponent<Rigidbody>().velocity = base.transform.forward * InitialSpeed;
		}
		else
		{
			RandomSpawn = SpawnTime + (float)Random.Range(-2, 2);
		}
	}

	private void Update()
	{
		if (Spawner)
		{
			T += Time.deltaTime;
			if (T >= RandomSpawn)
			{
				RandomSpawn = SpawnTime + (float)Random.Range(-2, 2);
				T = 0f;
				Object.Instantiate(Disable, Random.insideUnitSphere * Range + base.transform.position, Quaternion.identity);
			}
		}
		else if (!GetComponent<Rigidbody>().isKinematic)
		{
			base.transform.LookAt(base.transform.position + GetComponent<Rigidbody>().velocity);
		}
	}

	private void OnCollisionEnter()
	{
		if (Physics.Raycast(base.transform.position, base.transform.forward, 50f) || GetComponent<Rigidbody>().velocity.magnitude <= 0.5f)
		{
			Disable.SetActive(value: false);
			GetComponent<Rigidbody>().isKinematic = true;
			ChangeSize.radius = 1f;
			Invoke("Die", 5f);
		}
	}

	private void Die()
	{
		Object.Destroy(base.gameObject);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(base.transform.position, Range);
	}
}