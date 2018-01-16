using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLayoutGroup : MonoBehaviour {

	[SerializeField]GameObject _roomListingPrefab = null;
	GameObject RoomListingPrefab{
		get{ return _roomListingPrefab;}
	}

	private List<RoomListing> _roomListingButton = new List<RoomListing>();
	public List<RoomListing> RoomListingButton{
		get{ return _roomListingButton;}

	}

	void OnReceivedRoomListUpdate(){
		Debug.Log ("Room Update : "+PhotonNetwork.GetRoomList ());
		RoomInfo[] rooms = PhotonNetwork.GetRoomList ();

		foreach (RoomInfo room in rooms) {
			RoomRechived (room);
		}
		RemoveOldRooms ();
	}

	void RoomRechived(RoomInfo room){
		
		int index = RoomListingButton.FindIndex (x => x.RoomName == room.Name);
		Debug.Log ("Room received : "+room.Name);
		if (index == -1) {
			if (room.IsVisible && room.PlayerCount < room.MaxPlayers) {
				
				GameObject roomListingObj = Instantiate (RoomListingPrefab);
				Debug.Log ("Room display : "+ roomListingObj.transform.position);
				roomListingObj.transform.SetParent (transform, false);


				RoomListing roomListing = roomListingObj.GetComponent<RoomListing> ();
				RoomListingButton.Add (roomListing);

				index = (RoomListingButton.Count - 1);
				Debug.Log ("index : " + index);
			}
		} 

		if (index != -1){
			RoomListing roomListing = RoomListingButton [index];
			roomListing.SetRoomNameText (room.Name);
			roomListing.Updated = true;
		
		}
	}

	void RemoveOldRooms(){
		List<RoomListing> removeRooms = new List<RoomListing> ();

		foreach(RoomListing roomlisting in RoomListingButton){
			if (!roomlisting.Updated) {
				removeRooms.Add (roomlisting);
			} else {
				roomlisting.Updated = false;
			
			}

		}


		foreach (RoomListing roomlisting in removeRooms) {
			GameObject roomListingObj = roomlisting.gameObject;
			RoomListingButton.Remove (roomlisting);
			Destroy (roomListingObj);
		}

	}
}
