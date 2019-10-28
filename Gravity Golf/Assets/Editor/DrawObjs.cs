using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TreePlacer))]
public class DrawObjsEditor : Editor {
	
	int i = 0;
	int i2 = 0;
	Vector3 Pos;
	Vector3 Rot;
	
	GameObject TreeInstance;
	
	
	void OnSceneGUI () {
		
		if (Event.current.type == EventType.MouseUp) {
			RaycastHit H;
			if (Physics.Raycast (HandleUtility.GUIPointToWorldRay( Event.current.mousePosition ), out H, Mathf.Infinity)) {
				GameObject Tree = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Drag Into Level/TreeLook.prefab", typeof(GameObject)) as GameObject;
				TreeInstance = Instantiate(Tree) as GameObject;
				TreeInstance.transform.position = H.point;
				i = TreeInstance.GetComponent<GrassPlace>().Density+1;
				i2 = TreeInstance.GetComponent<GrassPlace>().Density;
				Pos = H.point;
			}
		}
		
		if (i > 0) {
			if (i <= i2) {
				if (i == i2) {
					Rot = TreeInstance.transform.eulerAngles;	
				}
				GameObject Tree1 = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Drag Into Level/TreeLook.prefab", typeof(GameObject)) as GameObject;
				GameObject TreeInstance1 = Instantiate(Tree1) as GameObject;
				TreeInstance1.transform.position = Pos;
				TreeInstance1.transform.eulerAngles = Rot;
				TreeInstance1.transform.Translate (Random.Range(-15, 15), Random.Range(-15, 15), 0);
			}
			i--;
		}
	}
}
