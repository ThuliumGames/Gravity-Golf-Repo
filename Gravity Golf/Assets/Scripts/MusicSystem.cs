using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MusicSystem : MonoBehaviour {
	public AudioSource BackgroundSound;

	public AudioSource Music;

	public AudioClip[] Songs;

	public static int PrevSong = -1;
	public static int PrevPrevSong = -1;

	public float MinTimeBetweenSongs = 60f;

	public float MaxTimeBetweenSongs = 300f;

	private float TimeToWait;

	private float T;
	
	bool Stay = false;

	private void Start() {
		if (SceneManager.GetActiveScene.name == "Intro") {
			Stay = true;
		} else {
			if (GameObject.Find("GlubGloGabGalab") == null) {
				base.name = "GlubGloGabGalab";
				transform.parent = null;
				Application.DontDestroyOnLoad(base.gameObject);
			} else {
				Object.Destroy(base.gameObject);
			}
		}
	}

	private void Update() {
		
		if (!Stay) {
			if (SceneManager.GetActiveScene().name == "Intro" || SceneManager.GetActiveScene().name == "Boss") {
				Object.Destroy(base.gameObject);
			}
			if (Input.GetKeyDown(KeyCode.Escape)) {
				SceneManager.LoadScene("Intro");
			}
		}
		
		if (T == 0f) {
			TimeToWait = Random.Range(MinTimeBetweenSongs, MaxTimeBetweenSongs);
		}
		
		if (!Music.isPlaying) {
			T += Time.deltaTime;
		}
		
		if (T >= TimeToWait) {
			int num = Random.Range(0, Songs.Length);
			if (num == PrevSong || num == PrevPrevSong)
			{
				return;
			}
			T = 0f;
			PrevPrevSong = PrevSong;
			PrevSong = num;
			Music.clip = Songs[num];
			Music.Play();
		}
		
		if (Music.isPlaying) {
			Music.volume = Mathf.Lerp(Music.volume, 0.75f, 0.05f * Time.deltaTime);
			BackgroundSound.volume = Mathf.Lerp(BackgroundSound.volume, 0f, 0.5f * Time.deltaTime);
		} else {
			Music.volume = Mathf.Lerp(Music.volume, 0f, 0.5f * Time.deltaTime);
			BackgroundSound.volume = Mathf.Lerp(BackgroundSound.volume, 0.5f, 0.05f * Time.deltaTime);
		}
	}
}
