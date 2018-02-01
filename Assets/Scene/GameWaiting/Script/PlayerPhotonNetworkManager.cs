using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhotonNetworkManager : MonoBehaviour {
	[SerializeField]MonoBehaviour[] localScript = null;
	[SerializeField]Camera playerCamera= null;
	[SerializeField]AudioListener playerAudioListener = null;
	[SerializeField]GameObject Child;
	// Use this for initialization
	void Awake () {
		
		if (!GetComponent<PhotonView> ().isMine) {
			foreach (MonoBehaviour c in localScript) {
			
				c.enabled = false;

			}
			playerCamera.enabled = false;
			playerAudioListener.enabled = false;

		}else{
			playerCamera.enabled = true;
			playerAudioListener.enabled = true;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
