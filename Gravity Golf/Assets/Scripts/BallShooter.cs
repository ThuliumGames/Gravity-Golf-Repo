using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BallShooter : MonoBehaviour {
	
	public GameObject GO;
	public GameObject GO2;
	public int Score;
	float TimeLeft;
	
	public Text ST;
	public Text TT;
	
	Vector3 Point = new Vector3 (0, 0, 50);
	
	void Start () {
		TimeLeft = 60;
	}
	
	void Update () {
		
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		
		TimeLeft -= Time.deltaTime;
		
		ST.text = "Score: " + Score;
		int TL = (int)TimeLeft;
		TT.text = "Time Left: " + TL;
		
		if (TL <= 0) {
			SceneManager.LoadScene("Intro");
		}
		
		RaycastHit H;
		Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out H);
		transform.LookAt(H.point);
		
		GetComponentInParent<TimedKill>().transform.RotateAround(Point, GetComponentInParent<TimedKill>().transform.right, Input.GetAxis("Vertical1"));
		GetComponentInParent<TimedKill>().transform.RotateAround(Point, -GetComponentInParent<TimedKill>().transform.up, Input.GetAxis("Horizontal1"));
		
		if (Input.GetButtonDown("Fire1")) {
			GetComponentInChildren<AudioSource>().pitch = Random.Range(0.8f, 1.2f);
			GetComponentInChildren<AudioSource>().Play();
			GameObject G = Instantiate (GO, transform.position+(transform.forward*5f), Quaternion.identity);
			GameObject G2 = Instantiate (GO2, transform.position+(transform.forward*5f), transform.rotation);
			G2.transform.SetParent(transform);
			G.transform.forward = transform.forward;
		}
	}
}
