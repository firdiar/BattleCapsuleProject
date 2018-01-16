using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour {
	[SerializeField]GameObject[] spawnPlayerPos = null;
	int playerInScene;
	int playerInRoom;
	bool summon = true;

	// Use this for initialization
	void Awake () {
		
		playerInRoom = PhotonNetwork.playerList.Length;
	
		PhotonNetwork.RPC (GetComponent<PhotonView> (), "JoinedScene", PhotonTargets.MasterClient, false);


		Debug.Log (playerInRoom);

	}

	[PunRPC]
	void JoinedScene(){
		playerInScene += 1;
		Debug.Log (playerInScene);

	
	}

	[PunRPC]
	void SummonCharacter(){
		
		PhotonNetwork.Instantiate ("Player", spawnPlayerPos [Random.Range (0, spawnPlayerPos.Length - 1)].transform.position, Quaternion.identity, 0); 
	}
	
	// Update is called once per frame
	void Update () {
		if ((playerInRoom == playerInScene) && summon) {
			summon = false;
			PhotonNetwork.RPC (GetComponent<PhotonView> (), "SummonCharacter", PhotonTargets.All, false);
		}
		
	}
}
