using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLayoutGroup : MonoBehaviour {

	[SerializeField]GameObject playerStatus = null;
	[SerializeField]PhotonView photonViews = null;

	[SerializeField]GameObject _playerListPrefab = null;
	private GameObject PlayerListPrefab{
		get{return _playerListPrefab; }
	}

	List<PlayerListing> _playerListings = new List<PlayerListing> ();
	private List<PlayerListing> PlayerListings {
		get{return _playerListings; }
	}

	public bool isAllReady{
		get{ 
			foreach (PlayerListing pl in PlayerListings) {
				if (pl.PhotonPlayer.isReady == false) {
					return false;
				}
			}
			return true;
		}
	}

	//called when you join a room
	void OnJoinedRoom(){

		foreach (Transform c in transform) {
			Destroy (c);
		}

		MainCanvasManager.Instance.CurrentRoomCanvas.transform.SetAsLastSibling ();

		PhotonPlayer[] photonPlayers = PhotonNetwork.playerList;
		foreach (PhotonPlayer player in photonPlayers) {
			if (player != null)
				PlayerJoinedRoom (player);
			
		}
			
	}

	void OnLeftRoom(){
		
		MainCanvasManager.Instance.LobbyCanvas.transform.SetAsLastSibling ();
	
		foreach (Transform c in transform) {
			Destroy (c.gameObject);
		}
		PlayerListings.Clear ();
		PhotonNetwork.player.isReady = false;
		playerStatus.GetComponent<PlayerStatus> ().SetToWaiting ();

	}

	//called when there are player who was get in to room
	void OnPhotonPlayerConnected(PhotonPlayer photonPlayer){
		PlayerJoinedRoom (photonPlayer);
		photonViews.RPC ("UpdatePlayerStatus", PhotonTargets.All, PhotonNetwork.player, PhotonNetwork.player.isReady);
	}


	//called when there are player who was get out from room
	void OnPhotonPlayerDisconnected(PhotonPlayer photonPlayer){
		PlayerLeftRoom (photonPlayer);
		//Debug.Log ("Ada yang keluar : " + photonPlayer.NickName);
	}


	void PlayerJoinedRoom(PhotonPlayer photonPlayer){
		PlayerLeftRoom (photonPlayer);

		GameObject playerListingObj = Instantiate (PlayerListPrefab);
		playerListingObj.transform.SetParent (transform, false);

		PlayerListing playerListing = playerListingObj.GetComponent<PlayerListing> ();
		playerListing.ApplyPhotonPlayer (photonPlayer);

		PlayerListings.Add (playerListing);

	
	}

	void PlayerLeftRoom(PhotonPlayer photonPlayer){
		Debug.Log (PlayerListings.Count);
		int index = PlayerListings.FindIndex (x => x.PhotonPlayer == photonPlayer);

		if (index != -1) {
			Debug.Log ("konfirmasi Player : " + photonPlayer.NickName +" Telah Keluar");

			Destroy (PlayerListings [index].gameObject);
			PlayerListings.RemoveAt (index);

			foreach (PlayerListing PL in PlayerListings) {
				PL.UpdateStatus ();
				PL.ColorUpdate ();
			}


		}
		
	}

	//dipanggil ketika roomstate di tekan
	public void PlayerStatusUpdate(){
		string status =playerStatus.GetComponent<UnityEngine.UI.Text> ().text;
		string myNickname = PhotonNetwork.player.NickName;


		if (status == "Ready") {
				photonViews.RPC ("UpdatePlayerStatus", PhotonTargets.All, PhotonNetwork.player, true);

		} else {
				photonViews.RPC ("UpdatePlayerStatus", PhotonTargets.All, PhotonNetwork.player, false);
		}

	}
		
	[PunRPC]
	void UpdatePlayerStatus(PhotonPlayer photonPlayer , bool status){
		Debug.Log ("terUpdate");
		int index = PlayerListings.FindIndex (x => x.PhotonPlayer == photonPlayer);
		if (index != -1) {
			PlayerListings [index].PhotonPlayer.isReady = status;
			PlayerListings [index].ColorUpdate ();
		}

	}
}
