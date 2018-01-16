using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyNetwork : MonoBehaviour {

	public string GAME_VERSION = "0.0.1";
	[SerializeField]Text[] status = null;
	[SerializeField]Button connectButton = null;

	// Use this for initialization
	void Start () {
		Debug.Log ("Connecting To Server..");
		PhotonNetwork.ConnectUsingSettings (GAME_VERSION);
	}
	void FixedUpdate(){
		foreach (Text c in status) {
			c.text = PhotonNetwork.connectionStateDetailed.ToString ();
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


}
