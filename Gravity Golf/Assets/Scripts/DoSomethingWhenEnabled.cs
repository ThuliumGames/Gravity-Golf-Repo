using UnityEngine;

public class DoSomethingWhenEnabled : MonoBehaviour {
	
	public string animName;
	
	private void OnEnabled() {
		GetComponent<Animator>().Play(animName);
	}
}
