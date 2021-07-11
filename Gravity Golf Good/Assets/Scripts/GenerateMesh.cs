using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class GenerateMesh : MonoBehaviour {
	
	public GameObject pointObj;
	public GameObject followObj;
	public Transform[] points;
	public Transform[] follow;
	public MeshFilter mf;
	public MeshCollider mc;
	List<Vector3> verts = new List<Vector3>();
	List<int> tris = new List<int>();
	List<int> mesh0 = new List<int>();
	List<int> mesh1 = new List<int>();
	List<int> mesh2 = new List<int>();
	List<int> mesh0tm = new List<int>();
	List<int> mesh1tm = new List<int>();
	List<int> mesh2tm = new List<int>();
	
	List<Vector2> uvs = new List<Vector2>();
	
	public float curve = 1;
	
	public bool Generate = false;
	
	int arrayLength;
	int followLength;
	
	public GameObject GravObj;
	public int linkNum = 0;
	public int priority = 1;
	
	Vector3[] contPosPre = {
		Vector3.zero,
		Vector3.zero,
		Vector3.zero
	};
	
	void Start () {
		if (Application.isPlaying) {
			Generate = false;	
		}
		contPosPre[0] = points[0].localPosition;
		contPosPre[1] = points[3].localPosition;
		contPosPre[2] = points[6].localPosition;
	}
	
	void Update () {
		
		points[1].localPosition += points[0].localPosition-contPosPre[0];
		
		points[2].localPosition += points[3].localPosition-contPosPre[1];
		points[4].localPosition = points[3].localPosition+(points[3].localPosition-points[2].localPosition);
		
		points[5].localPosition += points[6].localPosition-contPosPre[2];
		
		contPosPre[0] = points[0].localPosition;
		contPosPre[1] = points[3].localPosition;
		contPosPre[2] = points[6].localPosition;
		
		float fl = 0;
			
		fl += Vector3.Distance(points[0].localPosition, points[3].localPosition);
		fl += Vector3.Distance(points[0].localPosition, points[6].localPosition);
			
		float num = Mathf.Clamp(Mathf.Floor(fl), 1, Mathf.Infinity);
		
		System.Array.Resize(ref follow, (int)num);
		
		
		if (followLength != follow.Length) {
			foreach (Transform t in GetComponentsInChildren<Transform>()) {
				if (t.gameObject.name.Contains("Follow")) {
					DestroyImmediate(t.gameObject);
				}
			}
		}
		
		followLength = follow.Length;
		
		for (int f = 0; f < follow.Length; f++) {
			if (follow[f] == null) {
				follow[f] = Instantiate(followObj).transform;
				follow[f].SetParent(transform);
				follow[f].localPosition = new Vector3(0, 0, f);
				follow[f].gameObject.name = "Follower (" + f + ") : " + GetInstanceID();
			}
			//setup position
			
			//List<Vector3> lerPo = new List<Vector3>();
			//List<Vector3> lerRo = new List<Vector3>();
			
			/*for (int p = 0; p < points.Length-1; p++) {
				lerPo.Add(Vector3.Lerp(points[p].localPosition, points[p+1].localPosition, (((float)f)/((float)follow.Length))*curve));
				lerRo.Add(Vector3.Lerp(points[p].up, points[p+1].up, (((float)f)/((float)follow.Length))*curve));
			}
			
			while (lerPo.Count > 2) {
				for (int l = 0; l < lerPo.Count-1; l++) {
					Vector3 tmpP = Vector3.Lerp(lerPo[l], lerPo[l+1], (((float)f)/((float)follow.Length))*curve);
					Vector3 tmpR = Vector3.Lerp(lerRo[l], lerRo[l+1], (((float)f)/((float)follow.Length))*curve);
					lerPo[l] = tmpP;
					lerRo[l] = tmpR;
					lerPo.RemoveAt(l+1);
					lerRo.RemoveAt(l+1);
				}
			}*/
			Vector3 up = Vector3.zero;
			if (f < (float)(follow.Length)/2.0f) {
				follow[f].localPosition = Vector3.Lerp(Vector3.Lerp(Vector3.Lerp(points[0].localPosition, points[1].localPosition, (((float)f)/((float)follow.Length/2.0f))), points[2].localPosition, (((float)f)/((float)follow.Length/2.0f))), points[3].localPosition, (((float)f)/((float)follow.Length/2.0f)));
				up = Vector3.Lerp(Vector3.Lerp(Vector3.Lerp(points[0].up, points[1].up, (((float)f)/((float)follow.Length/2.0f))), points[2].up, (((float)f)/((float)follow.Length/2.0f))), points[3].up, (((float)f)/((float)follow.Length/2.0f))).normalized;
			} else {
				follow[f].localPosition = Vector3.Lerp(Vector3.Lerp(Vector3.Lerp(points[3].localPosition, points[4].localPosition, (((float)f-((float)follow.Length/2.0f))/((float)follow.Length/2.0f))), points[5].localPosition, (((float)f-((float)follow.Length/2.0f))/((float)follow.Length/2.0f))), points[6].localPosition, (((float)f-((float)follow.Length/2.0f))/((float)follow.Length/2.0f)));
				up = Vector3.Lerp(Vector3.Lerp(Vector3.Lerp(points[3].up, points[4].up, (((float)f-((float)follow.Length/2.0f))/((float)follow.Length/2.0f))), points[5].up, (((float)f-((float)follow.Length/2.0f))/((float)follow.Length/2.0f))), points[6].up, (((float)f-((float)follow.Length/2.0f))/((float)follow.Length/2.0f))).normalized;
			}
			
			if (f > 0) {
				if (follow[f-1] != null) {
					follow[f].LookAt(follow[f-1].position, up);
					follow[f].Rotate(0, 180, 0);
				}
			} else {
				if (follow[f+1] != null) {
					follow[f].LookAt(follow[f+1].position, up);
				}
			}
		}
		
		Debug.DrawLine(points[0].position, points[1].position, Color.white);
		Debug.DrawLine(points[3].position, points[2].position, Color.white);
		Debug.DrawLine(points[3].position, points[4].position, Color.white);
		Debug.DrawLine(points[6].position, points[5].position, Color.white);
		
		if (Generate) {
			
			Mesh mesh = new Mesh();
			
			verts.Clear();
			tris.Clear();
			uvs.Clear();
			
			mesh0.Clear();
			mesh1.Clear();
			mesh2.Clear();
			mesh0tm.Clear();
			mesh1tm.Clear();
			mesh2tm.Clear();
			
			for (int i = 0; i < follow.Length-1; i++) {
				
				Transform t1 = follow[i];
				Transform t2 = follow[i+1];
				if (i==0) {
					verts.Add(t1.localPosition-(t1.right*10));
					verts.Add(verts[verts.Count-1]+(t1.up*5f));
					verts.Add(verts[verts.Count-1]+(t1.right*4.5f));
					verts.Add(verts[verts.Count-1]+(t1.up*0.25f));
					verts.Add(verts[verts.Count-1]+(t1.right*0.5f));
					verts.Add(verts[verts.Count-1]-(t1.up*0.5f));
					verts.Add(verts[verts.Count-1]+(t1.right*2.5f));
					verts.Add(verts[verts.Count-1]+(t1.right*2.5f));
					verts.Add(verts[verts.Count-1]+(t1.right*2.5f));
					verts.Add(verts[verts.Count-1]+(t1.right*2.5f));
					verts.Add(verts[verts.Count-1]+(t1.up*0.5f));
					verts.Add(verts[verts.Count-1]+(t1.right*0.5f));
					verts.Add(verts[verts.Count-1]-(t1.up*0.25f));
					verts.Add(verts[verts.Count-1]+(t1.right*4.5f));
					verts.Add(verts[verts.Count-1]-(t1.up*5f));
					
					tris.Add(0); tris.Add(1); tris.Add(2);
					tris.Add(0); tris.Add(2); tris.Add(5);
					tris.Add(0); tris.Add(5); tris.Add(6);
					tris.Add(0); tris.Add(6); tris.Add(7);
					tris.Add(0); tris.Add(7); tris.Add(14);
					tris.Add(14); tris.Add(7); tris.Add(8);
					tris.Add(14); tris.Add(8); tris.Add(9);
					tris.Add(14); tris.Add(9); tris.Add(12);
					tris.Add(14); tris.Add(12); tris.Add(13);
					tris.Add(2); tris.Add(3); tris.Add(4);
					tris.Add(2); tris.Add(4); tris.Add(5);
					tris.Add(9); tris.Add(10); tris.Add(11);
					tris.Add(9); tris.Add(11); tris.Add(12);
				}
				
				verts.Add(t2.localPosition-(t2.right*10));
				verts.Add(verts[verts.Count-1]+(t2.up*5f));
				verts.Add(verts[verts.Count-1]+(t2.right*4.5f));
				verts.Add(verts[verts.Count-1]+(t2.up*0.25f));
				verts.Add(verts[verts.Count-1]+(t2.right*0.5f));
				verts.Add(verts[verts.Count-1]-(t2.up*0.5f));
				verts.Add(verts[verts.Count-1]+(t2.right*2.5f));
				verts.Add(verts[verts.Count-1]+(t2.right*2.5f));
				verts.Add(verts[verts.Count-1]+(t2.right*2.5f));
				verts.Add(verts[verts.Count-1]+(t2.right*2.5f));
				verts.Add(verts[verts.Count-1]+(t2.up*0.5f));
				verts.Add(verts[verts.Count-1]+(t2.right*0.5f));
				verts.Add(verts[verts.Count-1]-(t2.up*0.25f));
				verts.Add(verts[verts.Count-1]+(t2.right*4.5f));
				verts.Add(verts[verts.Count-1]-(t2.up*5f));
				
				for (int x = 0; x < 14; x++) {
					if (Vector3.Distance(verts[(0+(i*15))], verts[(15+(i*15))])<Vector3.Distance(verts[(14+(i*15))], verts[(29+(i*15))])) {
						tris.Add(0+(i*15)+x);
						tris.Add(15+(i*15)+x);
						tris.Add(1+(i*15)+x);
						
						tris.Add(1+(i*15)+x);
						tris.Add(15+(i*15)+x);
						tris.Add(16+(i*15)+x);
					} else {
						tris.Add(0+(i*15)+x);
						tris.Add(16+(i*15)+x);
						tris.Add(1+(i*15)+x);
						
						tris.Add(0+(i*15)+x);
						tris.Add(15+(i*15)+x);
						tris.Add(16+(i*15)+x);
					}
				}
				
				tris.Add(0+(i*15));
				tris.Add(29+(i*15));
				tris.Add(15+(i*15));
					
				tris.Add(14+(i*15));
				tris.Add(29+(i*15));
				tris.Add(0+(i*15));
				
				mesh0tm.Add(0+(i*30)+13);
				mesh0tm.Add(1+(i*30)+13);
				mesh0tm.Add(2+(i*30)+13);
				mesh0tm.Add(3+(i*30)+13);
				mesh2tm.Add(4+(i*30)+13);
				mesh2tm.Add(5+(i*30)+13);
				mesh2tm.Add(6+(i*30)+13);
				mesh2tm.Add(7+(i*30)+13);
				mesh2tm.Add(8+(i*30)+13);
				mesh2tm.Add(9+(i*30)+13);
				mesh1tm.Add(10+(i*30)+13);
				mesh1tm.Add(11+(i*30)+13);
				mesh1tm.Add(12+(i*30)+13);
				mesh1tm.Add(13+(i*30)+13);
				mesh1tm.Add(14+(i*30)+13);
				mesh1tm.Add(15+(i*30)+13);
				mesh1tm.Add(16+(i*30)+13);
				mesh1tm.Add(17+(i*30)+13);
				mesh2tm.Add(18+(i*30)+13);
				mesh2tm.Add(19+(i*30)+13);
				mesh2tm.Add(20+(i*30)+13);
				mesh2tm.Add(21+(i*30)+13);
				mesh2tm.Add(22+(i*30)+13);
				mesh2tm.Add(23+(i*30)+13);
				mesh0tm.Add(24+(i*30)+13);
				mesh0tm.Add(25+(i*30)+13);
				mesh0tm.Add(26+(i*30)+13);
				mesh0tm.Add(27+(i*30)+13);
				mesh0tm.Add(28+(i*30)+13);
				mesh0tm.Add(29+(i*30)+13);
				
				uvs.Add(new Vector2(1.0f, i));
				uvs.Add(new Vector2(0.0f, i));
				uvs.Add(new Vector2(1.0f, i));
				uvs.Add(new Vector2(0.0f, i));
				uvs.Add(new Vector2(1.0f, i));
				uvs.Add(new Vector2(0.0f, i));
				uvs.Add(new Vector2(1.0f, i));
				uvs.Add(new Vector2(0.0f, i));
				uvs.Add(new Vector2(1.0f, i));
				uvs.Add(new Vector2(0.0f, i));
				uvs.Add(new Vector2(1.0f, i));
				uvs.Add(new Vector2(0.0f, i));
				uvs.Add(new Vector2(1.0f, i));
				uvs.Add(new Vector2(0.0f, i));
				uvs.Add(new Vector2(1.0f, i));
				
			}
			
			tris.Add(2+(15*(follow.Length-1))); tris.Add(1+(15*(follow.Length-1))); tris.Add(0+(15*(follow.Length-1)));
			tris.Add(5+(15*(follow.Length-1))); tris.Add(2+(15*(follow.Length-1))); tris.Add(0+(15*(follow.Length-1)));
			tris.Add(6+(15*(follow.Length-1))); tris.Add(5+(15*(follow.Length-1))); tris.Add(0+(15*(follow.Length-1)));
			tris.Add(7+(15*(follow.Length-1))); tris.Add(6+(15*(follow.Length-1))); tris.Add(0+(15*(follow.Length-1)));
			tris.Add(14+(15*(follow.Length-1))); tris.Add(7+(15*(follow.Length-1))); tris.Add(0+(15*(follow.Length-1)));
			tris.Add(8+(15*(follow.Length-1))); tris.Add(7+(15*(follow.Length-1))); tris.Add(14+(15*(follow.Length-1)));
			tris.Add(9+(15*(follow.Length-1))); tris.Add(8+(15*(follow.Length-1))); tris.Add(14+(15*(follow.Length-1)));
			tris.Add(12+(15*(follow.Length-1))); tris.Add(9+(15*(follow.Length-1))); tris.Add(14+(15*(follow.Length-1)));
			tris.Add(13+(15*(follow.Length-1))); tris.Add(12+(15*(follow.Length-1))); tris.Add(14+(15*(follow.Length-1)));
			tris.Add(4+(15*(follow.Length-1))); tris.Add(3+(15*(follow.Length-1))); tris.Add(2+(15*(follow.Length-1)));
			tris.Add(5+(15*(follow.Length-1))); tris.Add(4+(15*(follow.Length-1))); tris.Add(2+(15*(follow.Length-1)));
			tris.Add(11+(15*(follow.Length-1))); tris.Add(9+(15*(follow.Length-1))); tris.Add(12+(15*(follow.Length-1)));
			tris.Add(10+(15*(follow.Length-1))); tris.Add(9+(15*(follow.Length-1))); tris.Add(11+(15*(follow.Length-1)));
			
			//
			for (int a = 0; a < 39; a++) {
				if (a < 27) {
					mesh0.Add(tris[a]);
				} else {
					mesh2.Add(tris[a]);
				}
			}
			for (int a = 0; a < 39; a++) {
				if (a < 27) {
					mesh0.Add(tris[((tris.Count)-39)+a]);
				} else {
					mesh2.Add(tris[((tris.Count)-39)+a]);
				}
			}
			foreach (int z in mesh0tm) {
				mesh0.Add(tris[(z*3)+0]);
				mesh0.Add(tris[(z*3)+1]);
				mesh0.Add(tris[(z*3)+2]);
			}
			foreach (int z in mesh1tm) {
				mesh1.Add(tris[(z*3)+0]);
				mesh1.Add(tris[(z*3)+1]);
				mesh1.Add(tris[(z*3)+2]);
			}
			foreach (int z in mesh2tm) {
				mesh2.Add(tris[(z*3)+0]);
				mesh2.Add(tris[(z*3)+1]);
				mesh2.Add(tris[(z*3)+2]);
			}
			//
			
			uvs.Add(new Vector2(1.0f, follow.Length-1));
			uvs.Add(new Vector2(0.0f, follow.Length-1));
			uvs.Add(new Vector2(1.0f, follow.Length-1));
			uvs.Add(new Vector2(0.0f, follow.Length-1));
			uvs.Add(new Vector2(1.0f, follow.Length-1));
			uvs.Add(new Vector2(0.0f, follow.Length-1));
			uvs.Add(new Vector2(1.0f, follow.Length-1));
			uvs.Add(new Vector2(0.0f, follow.Length-1));
			uvs.Add(new Vector2(1.0f, follow.Length-1));
			uvs.Add(new Vector2(0.0f, follow.Length-1));
			uvs.Add(new Vector2(1.0f, follow.Length-1));
			uvs.Add(new Vector2(0.0f, follow.Length-1));
			uvs.Add(new Vector2(1.0f, follow.Length-1));
			uvs.Add(new Vector2(0.0f, follow.Length-1));
			uvs.Add(new Vector2(1.0f, follow.Length-1));
			
			mesh.vertices = verts.ToArray();
			mesh.triangles = tris.ToArray();
			mesh.uv = uvs.ToArray();
			
			mesh.subMeshCount=3;
			
			mesh.SetTriangles(mesh0, 0, true, 0);
			mesh.SetTriangles(mesh1, 1, true, 0);
			mesh.SetTriangles(mesh2, 2, true, 0);
			
			mesh.Optimize();
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			
			mf.sharedMesh = mesh;
			mc.sharedMesh = mesh;
			//Generate = false;
		}
		
		if (Application.isPlaying) {
			foreach (Gravity g in GetComponentsInChildren<Gravity>()) {
				g.onLinePlanet = GravObj;
				g.linkNum = linkNum;
				g.priority = priority;
			}
		}
	}
}
