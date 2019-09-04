using UnityEngine;
using UnityEngine.UI;

public class RemoveText : MonoBehaviour {
	private bool Fade;
	public string KeyToPress;
	public GameObject[] ObjectsToDisable;
	public Vector3 EnableAtPlace;
	public float Range;

	void Start () {
		if (ObjectsToDisable == null) {
			System.Array.Resize (ref ObjectsToDisable, 1);
			ObjectsToDisable[0] = this.gameObject;
		}
	}
	
	private void Update() {
		
		if (EnableAtPlace != new Vector3 (0, 0, 0) && GetComponent<Text>().color.a == 0) {
			if (Vector3.Distance (GameObject.Find("Golf Ball").transform.position, EnableAtPlace) < Range) {
				GetComponent<Text>().color = new Vector4(1f, 1f, 1f, 1f);
				foreach (GameObject G in ObjectsToDisable) {
					G.SetActive(true);
				}
			}
		}
		if (((GameObject.Find("Golf Ball").GetComponent<GolfHit>().Strokes > 0 && KeyToPress == "") || (Input.GetButtonDown(KeyToPress))) && ObjectsToDisable[0].activeSelf == true) {
			Fade = true;
		}
		
		if (Fade) {
			Text component = GetComponent<Text>();
			Color color = GetComponent<Text>().color;
			component.color = new Vector4(1f, 1f, 1f, Mathf.Lerp(color.a, 0f, 7*Time.deltaTime));
		}
		
		Color color2 = GetComponent<Text>().color;
		if (color2.a < 0.001f && (EnableAtPlace == new Vector3 (0, 0, 0) || Vector3.Distance (GameObject.Find("Golf Ball").transform.position, EnableAtPlace) < Range)) {
			foreach (GameObject G in ObjectsToDisable) {
				G.SetActive(false);
			}
		}
	}
}
