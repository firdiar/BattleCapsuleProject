using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour {

	[SerializeField]Text _statusText = null;
	Text StatusText{
		get{return _statusText;}
	}
	[SerializeField]Color[] color = new Color[2];


	public void ChangeStatusPlayer(){
		
		if (StatusText.text == "Ready") {
			
			SetToWaiting ();
		} else {
			SetToReady ();

		}
	}

	public void SetToReady(){
		StatusText.text = "Ready";
		GetComponent<Image> ().color = color[1];

	}

	public void SetToWaiting(){
		StatusText.text = "Waiting";
		GetComponent<Image> ().color = color[0];

	}

}
