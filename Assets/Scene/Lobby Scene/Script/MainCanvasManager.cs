using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvasManager : MonoBehaviour {

	public static MainCanvasManager Instance = null;

	public GameObject loadingPrefabs = null;
	[SerializeField]LobbyCanvas _lobbyCanvas = null;
	public LobbyCanvas LobbyCanvas{
		get{return _lobbyCanvas; }
	}

	[SerializeField]CurrentRoomCanvas _currentRoomCanvas = null;
	public CurrentRoomCanvas CurrentRoomCanvas{
		get{return _currentRoomCanvas; }
	}
	public GameObject[] Bg;

	void Awake(){
		
		Instance = this;
		loadingPrefabs.SetActive (true);
	
	}


}
