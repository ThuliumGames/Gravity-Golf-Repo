using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GrassPlace : MonoBehaviour {
	
	public bool Flat;
	
	public GameObject Grass;

	public Transform[] NoPlace;

	public float NoPlDis = 1f;

	public int Density = 100;

	public float Min = 0.01f;

	public float Max = 0.05f;

	private void Start() {
		for (int i = 0; i < Density; i++) {
			GameObject Go;
			Vector3 localScale = transform.localScale;
			if (!Flat) {
				Go = Object.Instantiate(Grass, transform.position + (Random.onUnitSphere * localScale.x), new Quaternion(0f, 0f, 0f, 0f));
				Go.transform.SetParent(transform);
				Go.transform.LookAt(transform.position);
				Go.transform.Rotate(180f, 0f, Random.Range(0, 360));
			} else {
				Go = Object.Instantiate(Grass, transform.position + new Vector3 (Random.Range (-localScale.x/2, localScale.x/2), Random.Range (-localScale.y/2, localScale.y/2), Random.Range (-localScale.z/2, localScale.z/2)), new Quaternion(0f, 0f, 0f, 0f));
				Go.transform.Rotate(0f, Random.Range(0, 360), 0f);
			}
			float num = Random.Range(Min, Max);
			if (!Flat) {
				float num2 = num;
				Vector3 localScale2 = base.transform.localScale;
				float x = num2 / localScale2.x;
				float num3 = num;
				Vector3 localScale3 = base.transform.localScale;
				float y = num3 / localScale3.x;
				float num4 = num;
				Vector3 localScale4 = base.transform.localScale;
				Go.transform.localScale = new Vector3(x, y, num4 / localScale4.x);
			} else {
				Go.transform.localScale = new Vector3(num, num, num);
			}
			if (NoPlace.Length > 0) {
				Transform[] noPlace = NoPlace;
				foreach (Transform transform2 in noPlace) {
					if (Vector3.Distance(Go.transform.position, transform2.position) < NoPlDis) {
						Object.Destroy(Go);
						i--;
					}
				}
			}
		}
	}
}
