using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ExtendedTMPro;

public class Win : MonoBehaviour {
	public GameObject Ball;

	public GameObject canvas;

	public ExtendedTextMeshProUGUI WinText;

	public string[] WinTexts;

	public int Par;

	public Text ParText;

	public bool EndThing;

	public bool EndThing2;

	private void Start() {
		Ball = GameObject.Find("Golf Ball");
		canvas = GameObject.Find("All Level Objects/Win");
		WinText = GameObject.Find("All Level Objects/Win/Text").GetComponent<ExtendedTextMeshProUGUI>();
		ParText = GameObject.Find("ParText").GetComponent<Text>();
		if (GameObject.Find ("BossPlanet")) {
			ParText.text = "5";
		} else {
			ParText.text = Par.ToString();
		}
		canvas.SetActive(value: false);
	}

	private void LateUpdate() {
		
		if (Input.GetKey (KeyCode.J)) {
			PlayerPrefs.DeleteAll();
		}
		
		if (Ball != null && EndThing) {
			if (EndThing2) {
				Ball.GetComponent<Rigidbody>().velocity = -base.transform.up * 2f;
				Ball.GetComponent<Collider>().enabled = false;
			} else {
				Ball.GetComponent<Rigidbody>().drag = 10f;
				Ball.GetComponent<Rigidbody>().AddExplosionForce(-75f, base.transform.position + (base.transform.up*2), 100f);
			}
		} else if (Ball == null) {
			WinText.lineSpacing++;
			WinText.lineSpacing--;
		}
	}

	private void OnTriggerEnter() {
		if (!EndThing) {
			Ball.GetComponent<GolfHit>().enabled = false;
			GetComponent<AudioSource>().Play();
			EndThing = true;
			Invoke("T2", 1.5f);
			Invoke("Wn", 2f);
		}
	}

	private void T2() {
		EndThing2 = true;
		Ball.transform.position = base.transform.position + -base.transform.up;
	}

	private void Wn() {
		Ball.GetComponent<GolfHit>().enabled = true;
		canvas.SetActive(value: true);
		if (Ball.GetComponent<GolfHit>().Strokes <= 1) {
			WinText.text = "Hole In One!\n-" + (Par - Ball.GetComponent<GolfHit>().Strokes);
			GameObject.Find("HIOWin").GetComponent<AudioSource>().Play();
			Ball.GetComponent<GolfHit>().HoleIOConfetti.SetActive(true);
			GolfHit.GoodInARow++;
		} else if (Ball.GetComponent<GolfHit>().Strokes == Par) {
			WinText.text = "Par\n0";
			GolfHit.GoodInARow = 0;
		} else if (Ball.GetComponent<GolfHit>().Strokes > Par) {
			if (Ball.GetComponent<GolfHit>().Strokes == Par + 1) {
				WinText.text = "Bogey\n+" + (Ball.GetComponent<GolfHit>().Strokes - Par);
			} else if (Ball.GetComponent<GolfHit>().Strokes >= Par + 21) {
				WinText.text = "Like A Billion-Bogey\n+" + (Ball.GetComponent<GolfHit>().Strokes - Par);
			} else {
				WinText.text = WinTexts[Ball.GetComponent<GolfHit>().Strokes - Par - 2] + "-Bogey\n+" + (Ball.GetComponent<GolfHit>().Strokes - Par);
			}
			GolfHit.GoodInARow = 0;
		} else if (Ball.GetComponent<GolfHit>().Strokes < Par) {
			if (Ball.GetComponent<GolfHit>().Strokes == Par - 1) {
				WinText.text = "Birdie\n-" + (Par - Ball.GetComponent<GolfHit>().Strokes);
			} else if (Ball.GetComponent<GolfHit>().Strokes == Par - 2) {
				WinText.text = "Eagle\n-" + (Par - Ball.GetComponent<GolfHit>().Strokes);
			} else if (Ball.GetComponent<GolfHit>().Strokes == Par - 3) {
				WinText.text = "Albatross\n-" + (Par - Ball.GetComponent<GolfHit>().Strokes);
			} else {
				WinText.text = WinTexts[Par - Ball.GetComponent<GolfHit>().Strokes - 2] + "-Albatross\n-" + (Par - Ball.GetComponent<GolfHit>().Strokes);
			}
			GolfHit.GoodInARow++;
		}
		WinText.lineSpacing = Mathf.Clamp(((WinText.text.ToCharArray().Length-7)*4), 1, 250);
		WinText.transform.localPosition = new Vector3 (0, Mathf.Clamp(-((WinText.text.ToCharArray().Length)*6)+50, -100+50, -30+50), 0);
		
		PlayerPrefs.SetInt("ScoreTemp", PlayerPrefs.GetInt("ScoreTemp", 0)+(Ball.GetComponent<GolfHit>().Strokes - Par));
		print (PlayerPrefs.GetInt("ScoreTemp", 0));
		
		print (SceneManager.GetActiveScene().path);
		if (SceneManager.GetActiveScene().name == "Boss" || SceneManager.GetActiveScene().path == "Assets/Scenes/World 1/Mini-Boss.unity") {
			char[] cTemp = SceneManager.GetActiveScene().path.ToCharArray();
			if (PlayerPrefs.GetInt("ScoreTemp", 0) < PlayerPrefs.GetInt("Score" + cTemp[20], 0)) {
				PlayerPrefs.SetInt("Score" + cTemp[20], PlayerPrefs.GetInt("ScoreTemp", 0));
				PlayerPrefs.SetInt("NumOB" + cTemp[20], PlayerPrefs.GetInt("NumOBTemp", 0));
				PlayerPrefs.SetInt("NumReset" + cTemp[20], PlayerPrefs.GetInt("NumResetTemp", 0));
			}
			PlayerPrefs.SetInt("ScoreTemp", 0);
			PlayerPrefs.SetInt("NumOBTemp", 0);
			PlayerPrefs.SetInt("NumResetTemp", 0);
			PlayerPrefs.SetString("SaveLevel", "");
		}
		Object.Destroy(Ball);
	}
}