using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[ExecuteInEditMode]
public class GrassPlace : MonoBehaviour {
	
	public bool Flat;
	
	public GameObject Grass;

	public Transform[] NoPlace;

	public float NoPlDis = 1f;

	public int Density = 100;

	public float Min = 0.01f;

	public float Max = 0.05f;
	
	public float DistAway;
	
	public bool JustObject;
	
	public LayerMask LM;

	private void Start() {
		
		if (!JustObject && Application.isPlaying) {
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
				
				Go.transform.Translate (0, 0, DistAway);
				
				RaycastHit H;
				if (Physics.Raycast (Go.transform.position, -Go.transform.forward, out H, Mathf.Infinity, LM)) {
					if (Vector3.Distance (Go.transform.position, H.point) > 0.5f) {
						Destroy (Go);
					}
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
		} else if (JustObject) {
			if (Application.isPlaying) {
				float num = Random.Range(Min, Max);
				transform.localScale = new Vector3(num, num, num);
			}
		}
	}
	
	void Update () {
		if (JustObject && !Application.isPlaying) {
			Planet Pl = null;
			float Dist = Mathf.Infinity;
			foreach (Planet P in GameObject.FindObjectsOfType<Planet>()) {
				if (Vector3.Distance (transform.position, P.transform.position) < Dist) {
					Dist = Vector3.Distance (transform.position, P.transform.position);
					Pl = P;
				}
			}
			transform.LookAt (Pl.transform.position);
			RaycastHit H;
			if (Physics.Raycast (transform.position, transform.forward, out H, Mathf.Infinity, LM)) {
				transform.position = H.point;
				transform.Translate(0, 0, DistAway);
			}
		}
	}
}
