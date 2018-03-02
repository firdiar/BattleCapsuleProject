using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyNetwork : MonoBehaviour {

	public string GAME_VERSION = "0.0.1";
	[SerializeField]Text[] status = null;
	[SerializeField]Button connectButton = null;
	[SerializeField]GameObject Panel = null;
	[SerializeField]GameObject PanelExit = null;

	// Use this for initialization
	void Awake() {
		if (!PhotonNetwork.connected) {
			Debug.Log ("Connecting To Server..");
			PhotonNetwork.ConnectUsingSettings (GAME_VERSION);
		}
	}
	void FixedUpdate(){
		foreach (Text c in status) {
			c.text = PhotonNetwork.connectionStateDetailed.ToString ();
			if (c.text == "PeerCreated") {
				Debug.Log ("Disconnected");
				connectButton.interactable = false;
				Panel.SetActive (true);
			}
			if (c.text == "JoinedLobby") {
				connectButton.interactable = true;
			}
		}
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.Escape)){
			PanelExit.SetActive(true);
		}
	}
	
	// Update is called once per frame
	void OnConnectedToMaster () {
		Debug.Log ("Connected to Master..");

		PhotonNetwork.automaticallySyncScene = true;

		PhotonNetwork.playerName = PlayerNetwork.instance.PlayerName;

		PhotonNetwork.JoinLobby (TypedLobby.Default);
		
	}

	void OnJoinedLobby(){
		connectButton.interactable = true;
		Debug.Log ("Joined Lobby..");
	}

	public void ReConnect(){
		UnityEngine.SceneManagement.SceneManager.LoadScene (UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name);
	}

	public void Exit(){
		Application.Quit ();
	}

	public void NonActive(GameObject c){
		c.SetActive (false);
	}



}
