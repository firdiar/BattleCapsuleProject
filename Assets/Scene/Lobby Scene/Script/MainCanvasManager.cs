using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvasManager : MonoBehaviour {

	public static MainCanvasManager Instance = null;

	[SerializeField]LobbyCanvas _lobbyCanvas = null;
	public LobbyCanvas LobbyCanvas{
		get{return _lobbyCanvas; }
	}

	[SerializeField]CurrentRoomCanvas _currentRoomCanvas = null;
	public CurrentRoomCanvas CurrentRoomCanvas{
		get{return _currentRoomCanvas; }
	}

	void Awake(){
		Instance = this;
	
	}
}
