using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayStats : MonoBehaviour {
	
	public int World;
	
	public Text Score;
	
	void Update () {
		Score.text = "Best Score : " + PlayerPrefs.GetInt("Score"+World, 0) + "\nNumber of OB's: " + PlayerPrefs.GetInt("NumOB"+World, 0) + "\nNumber of Resets: " + PlayerPrefs.GetInt("NumReset"+World, 0);
	}
}
