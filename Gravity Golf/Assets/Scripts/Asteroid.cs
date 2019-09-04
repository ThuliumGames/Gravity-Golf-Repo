using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

	private void Start() {
		if (!Spawner) {
			GetComponent<Rigidbody>().velocity = transform.forward * InitialSpeed;
		}
		else {
			RandomSpawn = SpawnTime + (float)Random.Range(-2, 2);
		}
	}

	private void Update() {
		if (Spawner) {
			T += Time.deltaTime;
			if (T >= RandomSpawn) {
				RandomSpawn = SpawnTime + (float)Random.Range(-2, 2);
				T = 0f;
				GameObject G = Object.Instantiate(Disable, Random.insideUnitSphere * Range + base.transform.position, Quaternion.identity);
				G.transform.LookAt(transform.position);
				G.transform.SetParent(transform);
			}
		} else if (!GetComponent<Rigidbody>().isKinematic) {
			transform.LookAt(transform.position + GetComponent<Rigidbody>().velocity);
		}
	}

	private void OnCollisionEnter() {
		if (Physics.Raycast(base.transform.position, base.transform.forward, 50f) || GetComponent<Rigidbody>().velocity.magnitude <= 0.5f) {
			Disable.SetActive(value: false);
			GetComponent<Rigidbody>().isKinematic = true;
			ChangeSize.radius = 1.5f;
			Invoke("Die", 5f);
		}
	}

	private void Die() {
		Object.Destroy(base.gameObject);
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(base.transform.position, Range);
	}
}