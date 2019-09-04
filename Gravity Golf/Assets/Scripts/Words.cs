using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Words : MonoBehaviour {
	
	public GameObject Next;
	char[] Word;
	
	public string SceneName;
	
	public AudioSource AS;
	
	int i = 0;
	
	float T = 0;
	
	void Start () {
		Word = GetComponent<Text>().text.ToCharArray();
		GetComponent<Text>().text = "";
	}
	
	void Update () {
		
		if (Word[i] != '$') {
			if (Word[i] != '@') {
				Next.SetActive(false);
				T += Time.deltaTime;
				if ((T >= 0.016f && (Word[(int)Mathf.Clamp (i-1, 0, Mathf.Infinity)] != '.' && Word[(int)Mathf.Clamp (i-1, 0, Mathf.Infinity)] != '\n')) || T >= 0.5f) {
					T = 0;
					GetComponent<Text>().text += Word[i];
					i++;
					AS.Play();
				}
			} else {
				Next.SetActive(true);
				if (Input.GetButtonDown("Fire1")) {
					GetComponent<Text>().text = "";
					i++;
				}
			}
		} else {
			Next.SetActive(true);
			if (Input.GetButtonDown("Fire1")) {
				SceneManager.LoadScene (SceneName);
			}
		}
	}
}
