using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Boss1_AI : MonoBehaviour
{
	public float Speed;

	public float RotSpeed;

	private Rigidbody RB;

	private Transform Ball;

	public int BossHealth = 8;

	public int PlayerHealth = 10;

	public int Phase = 1;

	public bool AngryMode;

	public bool Hit;

	public GameObject ObjToInst;

	public Vector3 PlaceToGo;

	public Renderer[] Rend;

	public GameObject[] Orbs;

	public ParticleSystem AngerPart;

	public ParticleSystem DirtPart;

	public Collider BlastHitBox;

	public Collider BlastHitBox2;

	public Collider AngryHitBox;

	public Animator Anim;

	public AudioSource Scream;

	private bool KeepGoing;

	private bool KeepGoing2 = true;

	private bool isAnimating;

	private void Start()
	{
		RB = GetComponent<Rigidbody>();
		Ball = GameObject.Find("All Level Objects/Golf Ball").transform;
	}

	private void Update()
	{
		RaycastHit RCF;
		RaycastHit RCB;
		Physics.Raycast (transform.position + transform.forward, -transform.up, out RCF, 100);
		Physics.Raycast (transform.position - transform.forward, -transform.up, out RCB, 100);
		RB.AddRelativeTorque ((RCF.distance-RCB.distance) * -1800000000 * Time.deltaTime, 0, 0);
		Physics.Raycast (transform.position + transform.right, -transform.up, out RCF, 100);
		Physics.Raycast (transform.position - transform.right, -transform.up, out RCB, 100);
		RB.AddRelativeTorque (0, 0, (RCF.distance-RCB.distance) * -1800000000 * Time.deltaTime);
		if (PlayerHealth <= -1)
		{
			SceneManager.LoadScene("Golf 1-Boss");
		}
		if (BossHealth == 1 && KeepGoing2)
		{
			KeepGoing2 = false;
			Invoke("AsteroidEnd", 0.75f);
		}
		if (!Anim.enabled)
		{
			isAnimating = false;
			RB.isKinematic = false;
		}
		else
		{
			isAnimating = true;
			RB.isKinematic = true;
		}
		GameObject[] orbs = Orbs;
		foreach (GameObject gameObject in orbs)
		{
			gameObject.SetActive(value: false);
		}
		AngryHitBox.enabled = false;
		BlastHitBox.enabled = false;
		BlastHitBox2.enabled = false;
		Renderer[] rend = Rend;
		foreach (Renderer renderer in rend)
		{
			renderer.materials[0].color = Color.white;
		}
		if (BossHealth > 0)
		{
			if (!Hit)
			{
				if (!AngryMode)
				{
					if (Phase == 1)
					{
						BlastHitBox.enabled = true;
						Anim.enabled = false;
						for (int k = 0; k < 4; k++)
						{
							if (!Orbs[k].GetComponent<Orbs>().BeenHit)
							{
								Orbs[k].SetActive(value: true);
							}
						}
						Speed = 25f;
						RotSpeed = 50f;
						Vector3 vector = base.transform.InverseTransformPoint(Ball.position);
						if (vector.x > 0.1f)
						{
							base.transform.Rotate(0f, RotSpeed * Time.deltaTime, 0f);
						}
						else
						{
							Vector3 vector2 = base.transform.InverseTransformPoint(Ball.position);
							if (vector2.x < -0.1f)
							{
								base.transform.Rotate(0f, (0f - RotSpeed) * Time.deltaTime, 0f);
							}
						}
						RB.velocity = base.transform.forward * Speed;
					}
					else if (Phase == 2)
					{
						AngryHitBox.enabled = true;
						Anim.enabled = true;
						if (KeepGoing)
						{
							StartCoroutine("AstShoot");
						}
					}
					else
					{
						if (Phase != 3)
						{
							return;
						}
						Anim.enabled = false;
						if (Vector3.Distance(base.transform.position, PlaceToGo) < 1f)
						{
							Renderer[] rend2 = Rend;
							foreach (Renderer renderer2 in rend2)
							{
								renderer2.materials[0].color = Color.red;
							}
							BlastHitBox2.enabled = true;
							if (RotSpeed != 25f)
							{
								DirtPart.Play();
								Invoke("PickNewSpot", 10f);
							}
							Orbs[4].SetActive(value: true);
							Speed = 0f;
							RotSpeed = 25f;
							Vector3 vector3 = base.transform.InverseTransformPoint(Ball.position);
							if (vector3.x < -8f)
							{
								base.transform.Rotate(0f, RotSpeed * Time.deltaTime, 0f);
								return;
							}
							Vector3 vector4 = base.transform.InverseTransformPoint(Ball.position);
							if (vector4.x > 8f)
							{
								base.transform.Rotate(0f, (0f - RotSpeed) * Time.deltaTime, 0f);
							}
							return;
						}
						BlastHitBox.enabled = true;
						if (RotSpeed != 101f)
						{
							Invoke("TooLong", 5f);
						}
						Speed = 25f;
						RotSpeed = 101f;
						Vector3 vector5 = base.transform.InverseTransformPoint(PlaceToGo);
						if (vector5.x > 0.1f)
						{
							base.transform.Rotate(0f, RotSpeed * Time.deltaTime, 0f);
						}
						else
						{
							Vector3 vector6 = base.transform.InverseTransformPoint(PlaceToGo);
							if (vector6.x < -0.1f)
							{
								base.transform.Rotate(0f, (0f - RotSpeed) * Time.deltaTime, 0f);
							}
						}
						RB.velocity = base.transform.forward * Speed;
					}
					return;
				}
				Renderer[] rend3 = Rend;
				foreach (Renderer renderer3 in rend3)
				{
					renderer3.materials[0].color = Color.red;
				}
				AngryHitBox.enabled = true;
				Anim.enabled = false;
				if (RotSpeed != 100f && Speed != 500f)
				{
					if (BossHealth < 7)
					{
						Phase = 2;
						if (BossHealth < 6)
						{
							Phase = 3;
						}
					}
					KeepGoing = false;
					Invoke("Continue", 1f);
					Scream.Play();
				}
				Speed = 500f;
				if (!KeepGoing)
				{
					return;
				}
				if (RotSpeed != 100f)
				{
					Invoke("EndAnger", 10f);
					AngerPart.Play();
				}
				Speed = 50f;
				RotSpeed = 100f;
				Vector3 vector7 = base.transform.InverseTransformPoint(Ball.position);
				if (vector7.x > 0.1f)
				{
					base.transform.Rotate(0f, RotSpeed * Time.deltaTime, 0f);
				}
				else
				{
					Vector3 vector8 = base.transform.InverseTransformPoint(Ball.position);
					if (vector8.x < -0.1f)
					{
						base.transform.Rotate(0f, (0f - RotSpeed) * Time.deltaTime, 0f);
					}
				}
				RB.velocity = base.transform.forward * Speed;
			}
			else if (!Orbs[5].GetComponent<Orbs>().BeenHit)
			{
				Invoke("FlipHit", 1f);
				Anim.enabled = true;
				if (!isAnimating)
				{
					Anim.Play("FlyBack");
				}
			}
			else
			{
				Invoke("Back", 1f);
				Anim.enabled = true;
				Anim.Play("Return");
			}
		}
		else
		{
			Anim.enabled = true;
			SceneManager.LoadScene("Boss_1 End");
		}
	}

	private void EndAnger()
	{
		AngryMode = false;
	}

	private void Asteroid()
	{
		GameObject gameObject = Object.Instantiate(ObjToInst, Ball.position, Quaternion.identity);
		gameObject.transform.LookAt(new Vector3(0f, -10f, 0f));
		gameObject.transform.Translate(0f, 0f, -20f);
	}

	private void AsteroidEnd()
	{
		GameObject gameObject = Object.Instantiate(ObjToInst, Ball.position, Quaternion.identity);
		gameObject.transform.LookAt(new Vector3(0f, -10f, 0f));
		gameObject.transform.Translate(0f, 0f, -20f);
		KeepGoing2 = true;
	}

	private IEnumerator AstShoot()
	{
		KeepGoing = false;
		for (int i = 0; i < 10; i++)
		{
			Anim.Play("JumpMode");
			yield return new WaitForSeconds(1f);
			Asteroid();
		}
		Anim.Play("JumpMode");
		yield return new WaitForSeconds(7f);
		Anim.Play("StayStill");
		Phase = 1;
		KeepGoing = true;
	}

	private void Continue()
	{
		KeepGoing = true;
	}

	private void FlipHit()
	{
		Orbs[5].SetActive(value: true);
	}

	private void Back()
	{
		Anim.Play("StayStill");
		Orbs[5].GetComponent<Orbs>().BeenHit = false;
		Orbs[4].GetComponent<Orbs>().BeenHit = false;
		Hit = false;
		Phase = 2;
	}

	private void PickNewSpot()
	{
		PlaceToGo = new Vector3(0f, 0f, 0f) + Random.onUnitSphere * 20f;
	}

	private void TooLong()
	{
		PlaceToGo = base.transform.position;
	}
}