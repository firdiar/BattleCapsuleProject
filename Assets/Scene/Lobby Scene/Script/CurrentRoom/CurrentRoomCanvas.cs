﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentRoomCanvas : MonoBehaviour {

	[SerializeField]Button btnStartMatch = null;
	public Button ButtonStartMatch{
		get{ return btnStartMatch; }
	}

	[SerializeField]PlayerLayoutGroup PLG = null;

	public void On_ClickOpenGame(){
		if ((PhotonNetwork.isMasterClient)&&(PLG.isAllReady)) {
			PhotonNetwork.room.IsOpen = false;
			PhotonNetwork.room.IsVisible = false;
			PhotonNetwork.LoadLevel (1);
		}
	}

	public void On_ClickOutRoom(){
		PhotonNetwork.LeaveRoom (true);
	
	}
		
}