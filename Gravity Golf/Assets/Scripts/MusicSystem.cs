using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MusicSystem : MonoBehaviour
{
	public AudioSource BackgroundSound;

	public AudioSource Music;

	public AudioClip[] Songs;

	private int PrevSong = -1;

	public float MinTimeBetweenSongs = 60f;

	public float MaxTimeBetweenSongs = 300f;

	private float TimeToWait;

	private float T;

	private void Start()
	{
		if (GameObject.Find("GlubGloGabGalab") == null)
		{
			base.name = "GlubGloGabGalab";
			transform.parent = null;
			Application.DontDestroyOnLoad(base.gameObject);
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
		if (T == 0f)
		{
			TimeToWait = Random.Range(MinTimeBetweenSongs, MaxTimeBetweenSongs);
		}
		if (!Music.isPlaying)
		{
			T += Time.deltaTime;
		}
		if (T >= TimeToWait)
		{
			T = 0f;
			int num = Random.Range(0, Songs.Length);
			if (num == PrevSong)
			{
				return;
			}
			PrevSong = num;
			Music.clip = Songs[num];
			Music.Play();
		}
		if (Music.isPlaying)
		{
			Music.volume = Mathf.Lerp(Music.volume, 0.75f, 0.05f * Time.deltaTime);
			BackgroundSound.volume = Mathf.Lerp(BackgroundSound.volume, 0f, 0.5f * Time.deltaTime);
		}
		else
		{
			Music.volume = Mathf.Lerp(Music.volume, 0f, 0.5f * Time.deltaTime);
			BackgroundSound.volume = Mathf.Lerp(BackgroundSound.volume, 0.5f, 0.05f * Time.deltaTime);
		}
	}
}
