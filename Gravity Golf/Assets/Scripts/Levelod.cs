using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelOD : MonoBehaviour
{
	public LODGroup Lgroup;

	private void Start()
	{
		LOD[] array = new LOD[1];
		Renderer[] renderers = new Renderer[1]
		{
			GetComponent<Renderer>()
		};
		array[0] = new LOD(0.025f, renderers);
		Lgroup.SetLODs(array);
		Lgroup.RecalculateBounds();
	}
}