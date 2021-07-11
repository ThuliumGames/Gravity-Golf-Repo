using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squids : MonoBehaviour {
	
	public Animator anim;
	public AudioSource aS;
	public AudioClip[] aC;
	int onFrame;
	int doClap;
	
	float rotAng;
	
	public LayerMask lm;
	
	public SkinnedMeshRenderer[] mrs;
	
	public Texture2D[] ShirtTex;
	
	void Start () {
		transform.localScale = new Vector3 ((Random.Range(0.0145f, 0.0175f)*30.0f)/transform.parent.localScale.x, (Random.Range(0.0145f, 0.0175f)*30.0f)/transform.parent.localScale.z, (Random.Range(0.0145f, 0.0175f)*30.0f)/transform.parent.localScale.y);
		
		float randHue = Random.Range(350.0f, 360.0f);
		int randCol = Random.Range(0, 3);
		for (int i=0; i < mrs.Length-1; i++) {
			float V = 93.0f;
			switch (randCol) {
				case 0:
				V = 93.0f;
				break;
				case 1:
				V = 93.0f-((93.0f-55.0f)/2.0f);
				break;
				case 2:
				V = 55.0f;
				break;
				default:
				V = 100.0f;
				break;
			};
			
			mrs[i].materials[0].SetColor("Color_9FBF7BB4", Color.HSVToRGB(randHue/360.0f, 44.0f/100.0f, V/100.0f));
			if (i==0) {
				mrs[i].materials[2].SetColor("Color_9FBF7BB4", Color.HSVToRGB(randHue/360.0f, 44.0f/100.0f, V/100.0f));
			}
		}
		int tex = Random.Range(0, ShirtTex.Length);
		randHue = Random.Range(0.0f, 100.0f);
		mrs[mrs.Length-1].materials[0].SetColor("Color_9FBF7BB4", Color.HSVToRGB(randHue/360.0f, (59.0f/((float)tex+1.0f))/100.0f, 80.0f/100.0f));
		mrs[mrs.Length-1].materials[0].SetTexture("Texture2D_73669F84", ShirtTex[tex]);
	}
	
	void Update () {
		
		/*if (Global.didAGood) {
			if (doClap == 1) {
				Invoke("Clap", Mathf.Pow(Random.Range(0.0f, 1.0f), 2));
				if (onFrame<Time.frameCount-1) {
					Global.didAGood = false;
				}
			}
		} else {
			onFrame = Time.frameCount;
			doClap = Random.Range(0, 2);
		}*/
		
		RaycastHit h;
		if (Physics.Raycast (transform.position+(transform.forward), -transform.forward, out h, Mathf.Infinity, lm)) {
			//Vector3 V = transform.position;
			transform.position = h.point;
			transform.LookAt (transform.position+h.normal);
			//transform.position = V;
			if (GameObject.FindObjectOfType<Balls>()) {
				Vector3 relBallPos = transform.InverseTransformPoint(GameObject.FindObjectOfType<Balls>().gameObject.transform.position);
				rotAng = Mathf.Atan2(relBallPos.x, -relBallPos.y)*Mathf.Rad2Deg;
			} else {
				rotAng = 0;
			}
			transform.Rotate(0, 0, rotAng);
		}
	}
	
	/*void Clap () {
		anim.Play("Clap " + Random.Range(0, 4));
	}*/
	
	void PlaySound () {
		aS.clip = aC[Random.Range(0, aC.Length)];
		aS.volume = Random.Range(0.125f, 0.5f);
		aS.pitch = Random.Range(0.9f, 1.1f);
		aS.Play();
	}
}
