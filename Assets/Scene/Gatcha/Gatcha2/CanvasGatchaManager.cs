using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGatchaManager : MonoBehaviour {

	public GameObject btn;

	void Start(){
		Invoke ("Active", 5);
	}

	void Active(){
		btn.SetActive (true);
	}


	public void OnCLickBack(){
		UnityEngine.SceneManagement.SceneManager.LoadScene (1);
	}
}
