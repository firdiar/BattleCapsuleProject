using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class LobbyCanvas : MonoBehaviour {

	[SerializeField]RoomLayoutGroup _roomLayoutGroup = null;
	private RoomLayoutGroup RoomLayoutGroup {
		get{return _roomLayoutGroup;}
	}

	[SerializeField]InputField inputRoomName = null;
	[SerializeField]Dropdown createOrJoin = null;
	[SerializeField]InputField gold = null;
	[SerializeField]InputField diamond = null;
	[SerializeField]GameObject shop = null;

	public void OnClickJoinRoom(string roomName){
		
		createOrJoin.value = 1;
		inputRoomName.text = roomName;

	}

	public void OnClickShop(){
		if (shop.activeInHierarchy) {
			shop.SetActive (false);
		} else {
			shop.SetActive (true);
		}
	}

	public void OnCLickGacthaByAds(){
		if (Advertisement.IsReady ()) {
			Advertisement.Show ("rewardedVideo" , new ShowOptions(){resultCallback = HandleAdResult });
		} else {
			Debug.Log ("Not Ready Now");
		}


	
	}

	void HandleAdResult(ShowResult res){
		switch (res) {
		case ShowResult.Failed:
			Debug.Log ("Fail");
			break;

		case ShowResult.Skipped:
			Debug.Log ("Skip");
			break;
		case ShowResult.Finished:
			Debug.Log ("Finish");
			OnClickGatcha ();
			break;

		}

	}
	public void OnCLickGacthaByCoin(){
		int coin = System.Convert.ToInt32 (MainCanvasManager.Instance.LobbyCanvas.gold.text);
		if (coin < 100) {
			return;

		}
		coin -= 100;
		FirebaseHandlerScriptManager.SetCoin (coin.ToString ());
		OnClickGatcha ();
	}

	void OnClickGatcha(){
		UnityEngine.SceneManagement.SceneManager.LoadScene (3);
	}
	void Start(){
		Input.compass.enabled = true;
	}
	void Update(){
		float a = Input.compass.magneticHeading;
	}
	#if !UNITY_EDITOR
	void OnEnable(){
		
		FirebaseHandlerScriptManager.GetGoldDatabaseRef.ValueChanged += goldUpdate;
		FirebaseHandlerScriptManager.GetDiamondDatabaseRef.ValueChanged += diamondUpdate;

	}

	void OnDisable(){
		FirebaseHandlerScriptManager.GetGoldDatabaseRef.ValueChanged -= goldUpdate;
		FirebaseHandlerScriptManager.GetDiamondDatabaseRef.ValueChanged -= diamondUpdate;
	}
	#endif

	void goldUpdate(object sender , Firebase.Database.ValueChangedEventArgs arg){
		if (arg.DatabaseError != null) {
			gold.text = arg.DatabaseError.Message;
			return;
		}

		gold.text = arg.Snapshot.Value.ToString();
	
	}
	void diamondUpdate(object sender , Firebase.Database.ValueChangedEventArgs arg){
		if (arg.DatabaseError != null) {
			Debug.Log (arg.DatabaseError.Message);
			return;
		}

		diamond.text = arg.Snapshot.Value.ToString();

	}
}
