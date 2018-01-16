using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerListing : MonoBehaviour {

	public PhotonPlayer PhotonPlayer{ get; private set;}



	[SerializeField]Text _playerName = null;
	private Text PlayerName{
		get{ return _playerName; }
	}

	[SerializeField]Text _playerStatus = null;
	private Text PlayerStatus{
		get{ return _playerStatus; }
	}

	[Header("Warna Status")]
	[SerializeField]Color[] color = new Color[2];

	public void ApplyPhotonPlayer(PhotonPlayer photonPlayer){
		PlayerName.text = photonPlayer.NickName;
		PhotonPlayer = photonPlayer;

		UpdateStatus ();

		ColorUpdate ();

	}
	public void UpdateStatus(){
		if (PhotonPlayer.IsMasterClient) {
			PlayerStatus.text = "RM";

		} else {
			PlayerStatus.text = "M";

		}

		if (PhotonNetwork.isMasterClient) {
			MainCanvasManager.Instance.CurrentRoomCanvas.ButtonStartMatch.interactable = true;
		} else {
			MainCanvasManager.Instance.CurrentRoomCanvas.ButtonStartMatch.interactable = false;
		}
	}



	public void ColorUpdate(){
		if (PhotonPlayer.isReady) {
			PlayerStatus.GetComponentInParent<Image> ().color = color [1];
		} else {
			PlayerStatus.GetComponentInParent<Image> ().color = color [0];
		}
	}
}
