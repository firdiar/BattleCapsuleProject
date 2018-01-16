using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : MonoBehaviour {
	[SerializeField]Text _roomName = null;
	Text RoomName{
		get{return _roomName; }
	}

	[SerializeField]Dropdown createOrJoin = null;
	[SerializeField]InputField playerName = null;
	[SerializeField]RoomLayoutGroup listRoom = null;

	public void OnClick_CreateRoomOrJoinRoom(){

		if (playerName.text != "") {
			PhotonNetwork.playerName = playerName.text;
		
		} else {
			Debug.Log ("Nama Kosong");
			Debug.Log ("Menngunakan Nama Default : "+PhotonNetwork.player.NickName);

		}
		int index = -1;
		switch (createOrJoin.value) {
		//Jika Opsi Saat Ini Adalah Membuat Room
		case 0:
			index = listRoom.RoomListingButton.FindIndex (x => x.RoomName == RoomName.text);


				//jika room belum dibuat sebelumnya
			if (index == -1) {
				RoomOptions roomOptions = new RoomOptions (){ IsVisible = true, IsOpen = true, MaxPlayers = 10 };

				if (PhotonNetwork.CreateRoom (RoomName.text, roomOptions, TypedLobby.Default)) {
					Debug.Log ("Success to Send Create");
				} else {
					Debug.Log ("Fail to Send Create");
				}
			}

			break;

		//Jika Opsi saat ini adalah join Room
		case 1:
			index = listRoom.RoomListingButton.FindIndex (x => x.RoomName == RoomName.text);

			if (index != -1) {

				if (PhotonNetwork.JoinRoom(RoomName.text) ){
					Debug.Log ("Success to Send Create");
				} else {
					Debug.Log ("Fail to Send Create");
				}
			}


			break;

		}	
	
	}

	void OnPhotonCreatedRoomFailed(object[] codeAndMessege){
		Debug.Log ("Fail to Create : "+codeAndMessege[1]);
	}
	void OnCreatedRoom(){
	
		Debug.Log ("Success to Create Room");
	}


}
