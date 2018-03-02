
using UnityEngine;

public class DDOL : MonoBehaviour {

	// Use this for initialization
	void Awake () {

		DontDestroyOnLoad (this);
		
	}
	public void Destroy(){
		Destroy (gameObject);
	}

}
