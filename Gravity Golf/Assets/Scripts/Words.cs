using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Words : MonoBehaviour {
	
	public Animator AnimObject;
	
	public GameObject Next;
	char[] Word;
	
	public string SceneName;
	
	int i = 0;
	
	void Start () {
		Word = GetComponent<Text>().text.ToCharArray();
		GetComponent<Text>().text = "";
	}
	
	void Update () {
		
		if (Word[i] != '$') {
			if (Word[i] != '&') {
				if (Word[i] != '@') {
					Next.SetActive(false);
					GetComponent<Text>().text += Word[i];
					i++;
				} else {
					Next.SetActive(true);
					if (Input.GetButtonDown("Fire1")) {
						GetComponent<Text>().text = "";
						i++;
					}
				}
			} else {
				
				int length = 0;
				string AnimName = "";
				bool isDone = false;
				for (int a = 1; a < Word.Length-i; a++) {
					if (Word[i+a] != '&' && !isDone) {
						length++;
						AnimName += Word[i+a];
					} else {
						isDone = true;
					}
				}
				
				print (AnimName);
				AnimObject.Play(AnimName);
				i += length+2;
			}
		} else {
			Next.SetActive(true);
			if (Input.GetButtonDown("Fire1")) {
				SceneManager.LoadScene (SceneName);
			}
		}
	}
}
