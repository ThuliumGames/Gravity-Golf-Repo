using UnityEngine;
using UnityEngine.UI;

public class RemoveText : MonoBehaviour {
	private bool Fade;

	private void Update() {
		
		if (GameObject.Find("Golf Ball").GetComponent<GolfHit>().Strokes > 0) {
			Fade = true;
		}
		
		if (Fade) {
			Text component = GetComponent<Text>();
			Color color = GetComponent<Text>().color;
			component.color = new Vector4(1f, 1f, 1f, Mathf.Lerp(color.a, 0f, 2*Time.deltaTime));
		}
		
		Color color2 = GetComponent<Text>().color;
		if (color2.a < 0.001f) {
			gameObject.SetActive(false);
		}
	}
}
