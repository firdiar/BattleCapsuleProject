using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCanvas : MonoBehaviour {

	[SerializeField]RoomLayoutGroup _roomLayoutGroup = null;
	private RoomLayoutGroup RoomLayoutGroup {
		get{return _roomLayoutGroup;}
	}

	[SerializeField]InputField inputRoomName = null;
	[SerializeField]Dropdown createOrJoin = null;

	public void OnClickJoinRoom(string roomName){
		
		createOrJoin.value = 1;
		inputRoomName.text = roomName;

	}
}
