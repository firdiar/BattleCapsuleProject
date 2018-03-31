using System.Collections;
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
		if ((PhotonNetwork.isMasterClient)&&(PLG.isAllReady)&&(PLG.PlayerCount >= 1)) {
			PhotonNetwork.room.IsOpen = false;
			PhotonNetwork.room.IsVisible = false;
			PhotonNetwork.LoadLevel (2);
		}
	}

	public void On_ClickOutRoom(){
		MainCanvasManager.Instance.LobbyCanvas.gameObject.SetActive (true);
		MainCanvasManager.Instance.CurrentRoomCanvas.gameObject.SetActive (false);
		foreach (GameObject c in MainCanvasManager.Instance.Bg) {
			c.SetActive (true);
		}
		PhotonNetwork.LeaveRoom (true);
	
	}
		
}
