using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Portal : MonoBehaviour {
	public Transform OtherPortal;

	public static bool CanTele = true;

	private void TeleDo () {
		CanTele = true;
	}

	private void OnTriggerEnter(Collider coll) {
		if (CanTele) {
			CanTele = false;
			Vector3 position = base.transform.InverseTransformPoint(coll.transform.position);
			Vector3 direction = -base.transform.InverseTransformDirection(coll.GetComponent<Rigidbody>().velocity);
			coll.GetComponent<Rigidbody>().velocity = OtherPortal.transform.TransformDirection(direction);
			coll.transform.position = OtherPortal.transform.TransformPoint(position) + coll.GetComponent<Rigidbody>().velocity.normalized;
			Invoke ("TeleDo", 0.25f);
		}
	}
}